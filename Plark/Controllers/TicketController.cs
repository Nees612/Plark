using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plark.ErrorCodes;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.UnitOfWorkInterfaces;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Plark.Controllers
{
    [ApiController]
    [Route("Tickets")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TicketController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private readonly ITicketManager _ticketManager;
        private readonly IUserManager _userManager;

        public TicketController(IUnitOfWork unitOfWork, ITicketManager ticketManager, IUserManager userManager)
        {
            _repository = unitOfWork;
            _ticketManager = ticketManager;
            _userManager = userManager;
        }

        [HttpGet("{carId}/{time}")]
        public async Task<IActionResult> GetTicket(long carId, double time)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var Car = User.Cars.FirstOrDefault(c => c.Id.Equals(carId));

            if (Car == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            var Ticket = await _repository.TicketRepository.GetTicketByUser(User);
            if (Ticket == default)
            {
                var token = await _ticketManager.GenerateTicketJwtToken(User, Car, time);
                Ticket = new Ticket { User = User, Token = token };
                await _repository.TicketRepository.Add(Ticket);
                await _repository.Complete();
            }
            Bitmap qrCodeImage = await _ticketManager.CreateQrcodeFromTicket(Ticket);
            byte[] imgAsByte = await _ticketManager.BitmapToBytes(qrCodeImage);

            return File(imgAsByte, "image/png");
        }

        [HttpGet("TicketCreated")]
        public async Task<IActionResult> GetTicketCreationTime()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetById(userId);

            if (User == null)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            var Ticket = await _repository.TicketRepository.GetTicketByUser(User);

            if (Ticket == default)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            return Ok(await _ticketManager.GetCreatedFromToken(Ticket.Token));
        }
        [HttpGet("TicketClosed")]
        public async Task<IActionResult> GetTicketClosedTime()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetById(userId);

            if (User == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            var Ticket = await _repository.TicketRepository.GetTicketByUser(User);

            if (Ticket == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            return Ok(await _ticketManager.GetExpireFromToken(Ticket.Token));
        }

        //[HttpGet("CloseTicket/{userId}")]
        //public async Task<IActionResult> CloseTicket(long userId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
        //    }

        //    var User = await _repository.UsersRepoitory.GetById(userId);

        //    if (User == null)
        //    {
        //        return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
        //    }

        //    var Ticket = await _repository.TicketRepository.GetTicketByUser(User);

        //    if (Ticket == default)
        //    {
        //        return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
        //    }

        //    Ticket.Closed = DateTime.Now;
        //    await _repository.Complete();
        //    return Ok(Ticket.Closed.ToString());
        //}

        [HttpGet("ActiveTicket")]
        public async Task<IActionResult> GetActiveTicket()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null)
            {
                return NotFound(userId);
            }

            var Ticket = await _repository.TicketRepository.GetTicketByUser(User);

            if (Ticket == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            Bitmap qrCodeImage = await _ticketManager.CreateQrcodeFromTicket(Ticket);
            byte[] imgAsByte = await _ticketManager.BitmapToBytes(qrCodeImage);

            return File(imgAsByte, "image/png");
        }

        [HttpGet("ArchivedTickets")]
        public async Task<IActionResult> GetArchivedTickets()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetById(userId);

            if (User == null)
            {
                return NotFound(userId);
            }

            var Tickets = await _repository.ArchivedTicketRepository.GetTicketsByUserId(userId);

            if (Tickets == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            return Ok(Tickets);
        }

        [HttpGet("Archive")]
        public async Task<IActionResult> ArchiveUserTicket()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null)
            {
                return NotFound(userId);
            }

            var Ticket = await _repository.TicketRepository.GetTicketByUser(User);

            if (Ticket == default)
            {
                return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());
            }

            await _repository.ArchivedTicketRepository.Add(new ArchivedTicket
            {
                TransactionId = 0,
                User = Ticket.User,
                Token = Ticket.Token,
            });
            _repository.TicketRepository.Remove(Ticket);
            await _repository.Complete();
            return Ok();

        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicket(long ticketId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotDeleteTicket.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetById(userId);

            if (User == null) return BadRequest(ControllerErrorCode.CouldNotDeleteTicket.ToString());

            var Tickets = await _repository.ArchivedTicketRepository.GetTicketsByUserId(userId);
            var Ticket = Tickets.FirstOrDefault(t => t.Id.Equals(ticketId));

            if (Ticket == null) return BadRequest(ControllerErrorCode.CouldNotDeleteTicket.ToString());

            Ticket.User = null;

            _repository.ArchivedTicketRepository.Remove(Ticket);
            await _repository.Complete();
            return Ok();
        }

        //[HttpGet("Pay/{ticketId}")]
        //public async Task<IActionResult> PayTicket(long ticketId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
        //    }

        //    var Ticket = await _repository.TicketRepository.GetById(ticketId);

        //    if (Ticket == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

        //    _repository.TicketRepository.Remove(Ticket);
        //    await _repository.ArchivedTicketRepository.Add(new ArchivedTicket { 
        //        User= Ticket.User,
        //        Car=Ticket.Car,
        //        Created=Ticket.Created,
        //        Closed = Ticket.Closed,
        //        Price = Ticket.
        //    });

        //}
    }
}
