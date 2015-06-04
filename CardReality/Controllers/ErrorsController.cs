using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardReality.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Regular Error";
            ViewData["isSubscribed"] = false;
            return View();
        }

        public ActionResult NotFound404()
        {
            ViewBag.Title = "Error 404 - File not Found";
            ViewData["isSubscribed"] = false;
            return View();
        }
    }
}