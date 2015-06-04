using System.Linq;
using System.Web.Mvc;
using CardReality.Controllers;
using CardReality.Data.Data;
using CardReality.Data.Models;

namespace CardReality.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LettersController : BaseController
    {
        // GET: Admin/Letters
        public LettersController(IApplicationData data)
            : base(data)
        {
        }

        [HttpGet]
        // GET: Admin/Letter
        public ActionResult Index()
        {
            var letters = this.Data.Letters.All();
            return View(letters);
        }

        // GET: Admin/Letters/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Letter/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Letter letter = new Letter
                {
                    Char = collection[1],
                    Weight = int.Parse(collection[2])
                };

                this.Data.Letters.Add(letter);
                this.Data.SaveChanges();
                var letters = this.Data.Letters.All();
                return RedirectToAction("Index", letters);
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Letter/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var letter = this.Data.Letters.All().FirstOrDefault(l => l.Id == id);
            if (letter == null)
            {
                return HttpNotFound();
            }
            return View(letter);
        }

        // POST: Admin/Letter/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var letter = this.Data.Letters.All().FirstOrDefault(l => l.Id == id);
            if (letter == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(collection[2]))
            {
                letter.Weight = int.Parse(collection[2]);
            }

            this.Data.Letters.Update(letter);
            this.Data.SaveChanges();

            var letters = this.Data.Letters.All();
            return RedirectToAction("Index", letters);

        }

        // GET: Admin/Letter/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var letter = this.Data.Letters.All().FirstOrDefault(l => l.Id == id);

            return View(letter);
        }

        // POST: Admin/Letter/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var letter = this.Data.Letters.All().FirstOrDefault(l => l.Id == id);
                if (letter == null)
                {
                    return HttpNotFound();
                }
                this.Data.Letters.Delete(letter);
                this.Data.SaveChanges();
                var letters = this.Data.Letters.All();
                return RedirectToAction("Index", letters);
            }
            catch
            {
                return View();
            }
        }
    }
}
