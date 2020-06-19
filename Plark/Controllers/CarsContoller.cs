using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plark.Enums;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.UnitOfWorkInterfaces;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Plark.Controllers
{
    [ApiController]
    [Route("Cars")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarsContoller : Controller
    {

        private readonly IUnitOfWork _repository;
        private readonly IUserManager _userManager;

        public CarsContoller(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _repository = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserCars(long userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            return Ok(User.Cars);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddCarToUser([FromBody] Car car, long userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var User = await _repository.UsersRepoitory.GetById(userId);

            if (User == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            var Car = await _repository.CarRepository.GetCarBynumberPlate(car.NumberPlate);

            if (Car != null) return BadRequest(ControllerErrorCode.NumberPlateIsAlreadyExists.ToString());

            User.Cars.Add(car);

            await _repository.Complete();

            return Ok();
        }

        [HttpDelete("{carId}")]
        public async Task<IActionResult> DeleteCar(long carId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            var Car = User.Cars.FirstOrDefault(c => c.Id.Equals(carId));

            if (Car == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            _repository.CarRepository.Remove(Car);
            await _repository.Complete();

            return Ok();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserCar([FromBody]Car model, long userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null) return BadRequest(ControllerErrorCode.InvalidUser.ToString());

            var Car = User.Cars.FirstOrDefault(c => c.Id.Equals(model.Id));

            if (Car == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            Car.NickName = model.NickName;
            Car.NumberPlate = model.NumberPlate;
            await _repository.Complete();

            return Ok();
        }

    }
}
