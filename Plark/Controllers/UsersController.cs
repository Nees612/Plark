using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plark.Enums;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.UnitOfWorkInterfaces;
using Plark.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllUsers()
        {
            return Ok();
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserViewModel model)
        {
            var errorCodes = new List<string>();
            if (model == null || !ModelState.IsValid) return BadRequest(ControllerErrorCode.InvalidUser.ToString());

            if (model.Password != model.PasswordAgain) errorCodes.Add(ControllerErrorCode.PasswordsDoNotMatch.ToString());

            if (await _repository.UsersRepoitory.IsUserExistWithPhoneNumber(model.PhoneNumber)) errorCodes.Add(ControllerErrorCode.PhoneNumberAlreadyExists.ToString());

            var User = await _repository.UsersRepoitory.GetUserByEmail(model.EmailAddress);

            if (User != null) errorCodes.Add(ControllerErrorCode.EmailAddressIsAlreadyTaken.ToString());
            if (errorCodes.Count > 0) return BadRequest(errorCodes);

            User = _userManager.CreateUser(model);
            var passwordHash = _userManager.CreateHash(model.Password);
            User.PasswordHash = passwordHash;
            await _repository.UsersRepoitory.Add(User);
            await _repository.Complete();
            var emailToken = _userManager.GenerateEmailVerificationToken(User);
            _emailManager.SendVerificationEmail(User.EmailAddress, emailToken);

            return Ok();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] UserLoginViewModel model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest(ControllerErrorCode.InvalidUserNameOrPassword.ToString());

            var User = await _repository.UsersRepoitory.GetUserByEmail(model.Email);

            if (User == null) return BadRequest(ControllerErrorCode.InvalidUserNameOrPassword.ToString());
            if (User.IsEmailVerified && _userManager.IsValidPasssword(User.PasswordHash, model.Password))
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
            var userTickets = await _repository.TicketRepository.Find(t => t.User.Equals(User));
            _repository.TicketRepository.RemoveRange(userTickets);
            _repository.UsersRepoitory.Remove(User);
            await _repository.Complete();

            return Ok();
        }
    }
}
