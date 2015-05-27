using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Data;
using CardReality.Data.Data;

namespace CardReality.Controllers
{
    public class BaseController : Controller
    {
        private IApplicationData data;

        //public ApplicationDbContext Data { get; private set; }

        public BaseController(IApplicationData data)
        {
            this.data = data;
            //this.Data = ApplicationDbContext.Create();
        }

        protected IApplicationData Data { get { return this.data; } }
    }
}