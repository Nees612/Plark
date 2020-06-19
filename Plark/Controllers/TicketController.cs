using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Plark.Enums;
using Plark.Hubs;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.UnitOfWorkInterfaces;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Controllers
{
    [ApiController]
    [Route("Tickets")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TicketController : ControllerBase
    {
        private readonly IUnitOfWork _repository;
        private readonly ITicketManager _ticketManager;
        private readonly IUserManager _userManager;
        private readonly IHubContext<TicketsHub> _hub;

        public TicketController(IUnitOfWork unitOfWork, ITicketManager ticketManager, IUserManager userManager, IHubContext<TicketsHub> hub)
        {
            _repository = unitOfWork;
            _ticketManager = ticketManager;
            _userManager = userManager;
            _hub = hub;
        }

        [HttpGet("{carId}/{time}")]
        public async Task<IActionResult> GetTicketId(long carId, double time)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null) return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());

            var Car = User.Cars.FirstOrDefault(c => c.Id.Equals(carId));

            if (Car == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            var Ticket = await _repository.TicketRepository.GetTicketByUser(User);
            if (Ticket == default)
            {
                Ticket = new Ticket { User = User, ParkingTimeInHours = time, Car = Car };
                await _repository.TicketRepository.Add(Ticket);
                await _repository.Complete();
            }

            Bitmap qrCodeImage = await _ticketManager.CreateQrcode(Ticket.Id.ToString());
            byte[] imgAsByte = await _ticketManager.BitmapToBytes(qrCodeImage);

            return File(imgAsByte, "image/png");
        }

        [HttpGet("TicketIdQrImage")]
        public async Task<IActionResult> GetTicketIdQrImage()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var userId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetUserByIdWithCars(userId);

            if (User == null) return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());

            var Ticket = await _repository.TicketRepository.GetTicketByUser(User);
            if (Ticket == default) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            Bitmap qrCodeImage = await _ticketManager.CreateQrcode(Ticket.Id.ToString());
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

            if (Ticket.Token == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

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

            if (Ticket.Token == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

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

            if (Ticket.Token == null) return BadRequest(ControllerErrorCode.CouldNotFindItem.ToString());

            Bitmap qrCodeImage = await _ticketManager.CreateQrcode(Ticket.Token);
            byte[] imgAsByte = await _ticketManager.BitmapToBytes(qrCodeImage);
            System.Console.WriteLine("Teszting");
            return File(imgAsByte, "image/png");
        }

        [HttpGet("ActivateTicket/{ticketId}")]
        public async Task<IActionResult> ActivateTicket(long ticketId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotActivateTicket.ToString());
            }

            //var wardenId = _userManager.GetUserIdFromToken(Request.Headers["Authorization"]);
            //var Warden = await _repository.WardenRepository.GetById(wardenId);

            //if (Warden == null) return BadRequest(ControllerErrorCode.CouldNotActivateTicket.ToString());

            var Ticket = await _repository.TicketRepository.GetTicketWithCarAndUser(ticketId);

            if (Ticket == null) return BadRequest(ControllerErrorCode.CouldNotActivateTicket.ToString());

            var token = await _ticketManager.GenerateTicketJwtToken(Ticket.User, Ticket.Car, Ticket.ParkingTimeInHours);
            Ticket.Token = token;
            var User = Ticket.User;

            await _repository.Complete();
            await _hub.Clients.Client(User.ConnectionId).SendAsync("ActivatedTicket");
            System.Console.WriteLine(User.ConnectionId);

            return Ok();
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
