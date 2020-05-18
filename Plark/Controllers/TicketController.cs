using Microsoft.AspNetCore.Mvc;
using Plark.ErrorCodes;
using Plark.UnitOfWorkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plark.Models;
using QRCoder;
using System.Drawing;
using Plark.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Plark.Controllers
{
    [ApiController]
    [Route("Tickets")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TicketController : ControllerBase
    {
        private readonly IUnitOfWork _repository;

        public TicketController(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTicket(long userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ControllerErrorCode.CouldNotCreateItem.ToString());
            }

            var User = await _repository.UsersRepoitory.GetById(userId);

            if (User == null)
            {
                return NotFound(userId);
            }

            var Ticket = new Ticket { User = User, Created = DateTime.Now };

            QRCodeGenerator codeGenerator = new QRCodeGenerator();
            var TicketQrData = codeGenerator.CreateQrCode(Ticket.ToString(), QRCodeGenerator.ECCLevel.Q);
            var TicketQr = new QRCode(TicketQrData);
            Bitmap qrCodeImage = TicketQr.GetGraphic(20);


            await _repository.TicketRepository.Add(Ticket);
            await _repository.Complete();

            return Ok(TicketManager.BitmapToBytes(qrCodeImage));
        }
    }
}
