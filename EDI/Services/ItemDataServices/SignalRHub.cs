using EDI.Utilities.ItemDataUtilities;
using Microsoft.AspNetCore.SignalR;

namespace EDI.Services.ItemDataServices
{
    public class SignalRHub : Hub
    {
        IHttpContextAccessor _httpContextAccessor;
        public SignalRHub(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //public override Task OnConnected()
        //{
        //    Console.WriteLine("User connected.");
        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    if (stopCalled)
        //    {
        //        // Client explicitly stopped the connection
        //    }
        //    else
        //    {
        //        // Client disconnected unexpectedly (likely due to network issues)
        //    }
        //    string user = _httpContextAccessor.HttpContext?.Session?.GetString("UserEmail");
        //    Constants.LOCKED_ITEMS.Remove(user);

        //    return base.OnDisconnected(stopCalled);
        //}

        public  async Task OnUnload()
        {
            try 
            {
                string user = _httpContextAccessor.HttpContext?.Session?.GetString("UserEmail");
                Constants.LOCKED_ITEMS.Remove(user);
            }
            catch 
            {
            }
        }
    }
}
