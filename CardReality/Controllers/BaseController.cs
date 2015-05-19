using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Data;

namespace CardReality.Controllers
{
    public class BaseController : Controller
    {
        public ApplicationDbContext Data { get; private set; }

        public BaseController()
        {
            this.Data = ApplicationDbContext.Create();
        }
    }
}