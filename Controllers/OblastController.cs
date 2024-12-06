using FakeCET.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FakeCET.Controllers
{
    public class OblastController : Controller
    {
        private ApplicationDbContext _context;

        public OblastController()
        {
            _context = new ApplicationDbContext();

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Oblast
        [Authorize(Roles = "Admin, Predavac, Polaznik")]
        public ActionResult Index()
        {
            var oblasti = _context.Oblasti.ToList();
            return View(oblasti);
        }

        //ADMIN
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        //admin dodaje nova mesta
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "OblastID, Naziv")] Oblast oblast)
        {
            if (ModelState.IsValid)
            {
                _context.Oblasti.Add(oblast);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oblast);
        }


        //admin moze da ih izmeni
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Oblast oblast = _context.Oblasti.Find(id);

            if (oblast == null)
            {
                return HttpNotFound();
            }

            return View(oblast);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "OblastID, Naziv")] Oblast oblast)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(oblast).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseEntity = (Oblast)entry.GetDatabaseValues().ToObject();
                    if (databaseEntity == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. The record was deleted by another user.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        // Update the values from the database to the entity being edited
                        entry.CurrentValues.SetValues(databaseEntity);
                    }
                }
            }
            return View(oblast);
        }

        //ADMIN MOZE DA BRISE
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Oblast oblast = _context.Oblasti.Find(id);

            if (oblast == null)
            {
                return HttpNotFound();
            }

            return View(oblast);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConf(int id)
        {
            Oblast oblast = _context.Oblasti.Find(id);

            if (oblast == null)
            {
                return HttpNotFound();
            }

            // Pronađi sve kurseve koji pripadaju ovoj oblasti
            var kursevi = _context.Kursevi.Where(k => k.OblastID == id).ToList();

            // Obriši sve kurseve koji pripadaju oblasti
            foreach (var kurs in kursevi)
            {
                _context.Kursevi.Remove(kurs);
            }

            // Obriši oblast
            _context.Oblasti.Remove(oblast);

            // Sačuvaj izmene u bazi
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}