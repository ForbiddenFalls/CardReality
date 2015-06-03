using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CardReality.Data;
using CardReality.Data.Data;
using CardReality.Data.Models;
using CardReality.Models;
using Microsoft.AspNet.Identity;

namespace CardReality.Controllers
{
    public class BaseController : Controller
    {
        private IApplicationData data;

        //public ApplicationDbContext Data { get; private set; }

        public BaseController(IApplicationData data)
        {
            this.Data = data;
            //this.Data = ApplicationDbContext.Create();
        }

        protected IApplicationData Data 
        { 
            get { return this.data; }
            private set { this.data = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var identity = filterContext.RequestContext.HttpContext.User.Identity;
            bool isSubscribed = false;
            if (identity.IsAuthenticated)
            {
                Player player = this.Data.Players.Find(identity.GetUserId());
                isSubscribed = player.BattleSubscribed;
            }

            this.ViewData["isSubscribed"] = isSubscribed;

            base.OnActionExecuting(filterContext);
        }
    }
}