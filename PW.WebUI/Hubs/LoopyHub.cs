using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections;

namespace PW.Hubs
{
    [Authorize]
    public class NotifyHub : Hub<ITypedHubClient>
    {
        //private UserInfoInMemory _userInfoInMemory;

        //public NotifyHub(UserInfoInMemory userInfoInMemory)
        //{
        //    _userInfoInMemory = userInfoInMemory;
        //}

        //public override Task OnConnectedAsync()
        //{
        //    //Clients.All.SendAsync("Send", $"{Context.ConnectionId} joined the conversation");
        //    _userInfoInMemory.AddUpdate(Context.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value, Context.ConnectionId);
        //    return base.OnConnectedAsync();
        //}

        //public Task Notify(string email, string data)
        //{
        //    //var userInfoSender = _userInfoInMemory.GetUserInfo(email);
        //    return Clients.Client(userInfoSender.ConnectionId).SendAsync("TransactionNotify", data);
        //}

        //public async Task Join()
        //{
        //    if (!_userInfoInMemory.AddUpdate(Context.User.Identity.Name, Context.ConnectionId))
        //    {
        //        // new user
        //        //var list = _userInfoInMemory.GetAllUsersExceptThis(Context.User.Identity.Name).ToList();
        //        //await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync(
        //        //    "NewOnlineUser",
        //        //    _userInfoInMemory.GetUserInfo(Context.User.Identity.Name)
        //        //    );
        //    }
        //    else
        //    {
        //        // existing user joined again
        //    }
        //}

        //public async Task Leave()
        //{
        //    _userInfoInMemory.Remove(Context.User.Identity.Name);
        //    //await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync(
        //    //       "UserLeft",
        //    //       Context.User.Identity.Name
        //    //       );
        //}

        //public override Task OnDisconnectedAsync(System.Exception exception)
        //{
        //    //Clients.All.SendAsync("Send", $"{Context.ConnectionId} left the conversation");
        //    return base.OnDisconnectedAsync(exception);
        //}
    }
}
