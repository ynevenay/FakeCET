using FakeCET.Models;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FakeCET.Controllers
{
    public class MestoOdrzavanjaController : Controller
    {
        private ApplicationDbContext _context;

        public MestoOdrzavanjaController()
        {
            _context = new ApplicationDbContext();

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: MestoOdrzavanja
        [Authorize(Roles ="Admin, Predavac")]
        public ActionResult Index()
        {
            var mesta = _context.MestaOdrzavanja.ToList();
            return View(mesta);
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
        public ActionResult Create([Bind(Include ="MestoID,Naziv")] MestoOdrzavanja mestoOdrzavanja)
        {
            if (ModelState.IsValid)
            {
                _context.MestaOdrzavanja.Add(mestoOdrzavanja);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mestoOdrzavanja);
        }

        //admin moze da ih izmeni
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            MestoOdrzavanja mesto = _context.MestaOdrzavanja.Find(id);

            if(mesto == null)
            {
                return HttpNotFound();
            }

            return View(mesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "MestoID, Naziv")] MestoOdrzavanja mesto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(mesto).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseEntity = (MestoOdrzavanja)entry.GetDatabaseValues().ToObject();
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
            return View(mesto);
        }

        //ADMIN MOZE DA BRISE
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            MestoOdrzavanja mesto = _context.MestaOdrzavanja.Find(id);

            if (mesto == null)
            {
                return HttpNotFound();
            }

            return View(mesto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConf(int id)
        {
            MestoOdrzavanja mesto = _context.MestaOdrzavanja.Find(id);
            _context.MestaOdrzavanja.Remove(mesto);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //admin i predavac moge da vide detalje i ako ih nema hahaha
        [Authorize(Roles ="Admin, Predavac")]
        public ActionResult Details(int id) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MestoOdrzavanja mesto = _context.MestaOdrzavanja.Find(id);
            if (mesto == null)
            {
                return HttpNotFound();
            }
            return View(mesto);
        }

    }
}