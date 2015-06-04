using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CardReality.Data.Data;
using CardReality.Data.Models;

namespace CardReality.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IApplicationData data) : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        public ActionResult Contact(string n = null)
        {
            var name = this.Request.Form.Get("name");
            var email = this.Request.Form.Get("email");
            var subject = this.Request.Form.Get("subject");
            var message = this.Request.Form.Get("message");

            var contact = new Contact()
            {
                Email = email,
                Message = message,
                Name = name,
                Subject = subject
            };

            this.Data.Contacts.Add(contact);
            this.Data.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}