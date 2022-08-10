using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Hubs
{

    [HubName("test")]
    public class TestHub : Hub
    {
        static int count = 0;

        public static int TotalUsers { get; set; } = 0;




        public override Task OnConnected()
        {
            TotalUsers++;
            Clients.All.sessionMethod(TotalUsers);

            // the next call doesn't work idk why :) 
            //Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();

            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            TotalUsers--;
            Clients.All.sessionMethod(TotalUsers);
            


            // the next call doesn't work idk why :) 
            //Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();

            return base.OnDisconnected(stopCalled);
        }







        // senario#1 :: client sends data to server to process and then respond back
        public void serverMethod()
        {
            Clients.All.clientMethod(++count);

            
            //Another syntax  
            //Clients.All.Send("clientMethod", msg);
        }
        // senario#1 END 

        public string serverMethod2()
        {
            return "data from the server"; 
        }

       



    }
}