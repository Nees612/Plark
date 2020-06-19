using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.UnitOfWorkInterfaces;
using System.Threading.Tasks;

namespace Plark.Hubs
{

    public class TicketsHub : Hub
    {
        private readonly IUserManager _userManager;
        private readonly IUnitOfWork _repository;
        public TicketsHub(IUserManager userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _repository = unitOfWork;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var userId = _userManager.GetUserIdFromToken(Context.GetHttpContext().Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetById(userId);
            User.ConnectionId = Context.ConnectionId;

            await _repository.Complete();

        }
        public async void DeleteConnectionId()
        {
            var userId = _userManager.GetUserIdFromToken(Context.GetHttpContext().Request.Headers["Authorization"]);
            var User = await _repository.UsersRepoitory.GetById(userId);
            User.ConnectionId = null;

        }
    }
}
