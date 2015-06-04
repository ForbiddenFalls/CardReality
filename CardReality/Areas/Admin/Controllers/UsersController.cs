using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Controllers;
using CardReality.Data.Data;

namespace CardReality.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IApplicationData data) : base(data)
        {
        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            var players = this.Data.Players.All();
            return View(players);
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
