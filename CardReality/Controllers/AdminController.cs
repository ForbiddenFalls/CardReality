using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CardReality.Data.Data;
using CardReality.Data.Models;

namespace CardReality.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        public AdminController(IApplicationData data) : base(data)
        {
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Cards
        public ActionResult Cards()
        {
            var cards = this.Data.Cards.All().Select(c => c);
            return View(cards);
        }

        // GET: Admin/Users
        public ActionResult Users()
        {
            var users = this.Data.Players.All();
            return View(users);
        }

        // GET: Admin/Letters/Index
        public ActionResult Letters()
        {
            var letters = this.Data.Letters.All();
            return View("~/Views/Admin/Letters/Index.cshtml", letters);
        }

        // POST: Admin/Letters/Edit/1
        //[ValidateAntiForgeryToken]
        public ActionResult LettersEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var letter = this.Data.Letters.All().FirstOrDefault(l => l.Id == id);
            if (letter == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/Admin/Letters/Edit.cshtml", letter);
        }

        //// GET: Admin/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Admin/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Admin/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Admin/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Admin/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Admin/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
