using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardReality.Data;
using CardReality.Data.Data;
using Microsoft.AspNet.SignalR;

namespace CardReality.Hubs
{
    public class BaseHub : Hub
    {
        protected BaseHub()
        {
            this.Data = new ApplicationData(ApplicationDbContext.Create());
        }

        protected IApplicationData Data { get; private set; }
    }
}