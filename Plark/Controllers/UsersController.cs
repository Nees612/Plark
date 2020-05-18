using Microsoft.AspNetCore.Mvc;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.UnitOfWorkInterfaces;
using Plark.ViewModels;
using System.Threading.Tasks;
using Plark.ErrorCodes;


namespace Plark.Controllers
{
    [ApiController]
    [Route("Users")] 
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private readonly IEmailManager _emailManager;
        private readonly IUserManager _userManager;
        private readonly ICookieManager _cookieManager;
        public UsersController(IUnitOfWork repository, IEmailManager emailManager, IUserManager userManager, ICookieManager cookieManager)
        {
            _repository = repository;
            _emailManager = emailManager;
            _userManager = userManager;
            _cookieManager = cookieManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var Users = await _repository.UsersRepoitory.GetAll();

            return Ok(Users);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.InvalidUser.ToString());
            }

            if (model.Password != model.PasswordAgain)
            {
                return BadRequest(ControllerErrorCode.PasswordsDoNotMatch.ToString());
            }

            if (await _repository.UsersRepoitory.CanCreateUser(model))
            {
                var User = _userManager.CreateUser(model);
                var passwordHash = _userManager.CreateHash(model.Password);
                User.PasswordHash = passwordHash;
                await _repository.UsersRepoitory.Add(User);
                await _repository.Complete();
                var emailToken = _userManager.GenerateEmailVerificationToken(User);
                _emailManager.SendVerificationEmail(User.EmailAddress, emailToken);
                return Ok();
            }

            return BadRequest(ControllerErrorCode.CouldNotCreateUser.ToString());
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] UserLoginViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.InvalidUserNameOrPassword.ToString());
            }

            var User = await _repository.UsersRepoitory.GetUserByEmail(model.Email);

            if (User == null)
            {
                return BadRequest(ControllerErrorCode.InvalidUserNameOrPassword.ToString());
            }

            if (_userManager.IsValidPasssword(User.PasswordHash, model.Password))
            {
                var tokenString = _userManager.GenerateLoginJwtToken(User);
                var cookieOption = _cookieManager.CreateCookieOption();
                Response.Cookies.Append("PlarkToken", tokenString, cookieOption);
                return Ok();
            }

            return BadRequest(ControllerErrorCode.InvalidUserNameOrPassword.ToString());
        }

        [HttpGet("Verify/{emailToken}")]
        public async Task<IActionResult> VerifyEmail(string emailToken)
        {
            if (emailToken == null || !ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.InvalidToken.ToString());
            }

            if (!_userManager.IsEmailTokenValid(emailToken))
            {
                return BadRequest(ControllerErrorCode.TokenIsExpeired.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(emailToken);
            var User = await _repository.UsersRepoitory.GetById(userId);

            if (User == null)
            {
                return NotFound(userId);
            }

            User.IsEmailVerified = true;

            await _repository.Complete();
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(long Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotDeleteUser.ToString());
            }
             
            var User = await _repository.UsersRepoitory.GetById(Id);

            if (User == null)
            {
                return BadRequest(ControllerErrorCode.CouldNotDeleteUser.ToString());
            }

            _repository.UsersRepoitory.Remove(User);
            await _repository.Complete();

            return Ok();
        }
    }
}
