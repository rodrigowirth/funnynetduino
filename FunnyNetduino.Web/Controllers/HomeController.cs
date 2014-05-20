using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Client;

namespace FunnyNetduino.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public void SendCommand(Int64 commandId)
        {
            String signalRServer = ConfigurationManager.AppSettings["SignalRServer"];
            HubConnection hubConnection = new HubConnection(signalRServer);
            IHubProxy myHubProxy = hubConnection.CreateHubProxy("CommandHub");
            hubConnection.Start().Wait();

            String commandLine = null;

            switch (commandId)
            {
                case 0:
                    commandLine = "1;0";
                    break;
                case 1:
                    commandLine = "2;1";
                    break;
                case 2:
                    commandLine = "3;1";
                    break;
            }

            myHubProxy.Invoke("sendCommand", commandLine).Wait();
        }

    }
}
