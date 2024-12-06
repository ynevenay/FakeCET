using FakeCET.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FakeCET.ViewModels;
using System.Net;
using System.Diagnostics;

namespace FakeCET.Controllers
{
    public class PredavacController : Controller
    {
        private ApplicationDbContext _context;

        public PredavacController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Api()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            // Prikazujemo samo odobrene predavače
            var predavaci = _context.Predavaci.Where(p => p.Odobrenje).ToList();
            return View(predavaci);
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var predavac = _context.Predavaci.SingleOrDefault(p => p.PredavacID == id);
            if (predavac == null)
                return HttpNotFound();

            return View(predavac);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(Predavac predavac)
        {
            if (ModelState.IsValid)
            {
                // Kreiranje novog usera u AspNetUsers tabeli
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var newUser = new ApplicationUser
                {
                    UserName = predavac.MejlAdresa,
                    Email = predavac.MejlAdresa
                };
                var result = userManager.Create(newUser, "Password123!Predavac");

                if (result.Succeeded)
                {
                    userManager.AddToRole(newUser.Id, "Predavac");

                    // Sačuvaj predavača u Predavac tabelu sa odobrenjem na false
                    predavac.Odobrenje = false;
                    _context.Predavaci.Add(predavac);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Predavac"); 
                }
                else
                {
                    ModelState.AddModelError("", "Greška prilikom kreiranja korisnika.");
                    return View(predavac);
                }
            }

            return View(predavac);
        }



        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var predavac = _context.Predavaci.SingleOrDefault(p => p.PredavacID == id);
            if (predavac == null)
                return HttpNotFound();

            return View(predavac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, string kontaktTelefon, string zvanje, string oblastRada)
        {
            var predavacInDb = _context.Predavaci.Single(p => p.PredavacID == id);
            if (predavacInDb == null)
                return HttpNotFound();

            predavacInDb.KontaktTelefon = kontaktTelefon;
            predavacInDb.Zvanje = zvanje;
            predavacInDb.OblastRada = oblastRada;

            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var predavac = _context.Predavaci.SingleOrDefault(p => p.PredavacID == id);
            if (predavac == null)
                return HttpNotFound();

            return View(predavac);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var predavac = _context.Predavaci.SingleOrDefault(p => p.PredavacID == id);
            if (predavac == null)
                return HttpNotFound();

            // brisanje svih ref u PredavacKurs
            var predavacKursevi = _context.PredavaciKursevi.Where(pk => pk.PredavacID == id).ToList();
            foreach (var pk in predavacKursevi)
            {
                // ako ima termine obrisi i njih
                var termini = _context.Termini.Where(t => t.KursID == pk.KursID && t.PredavacID == pk.PredavacID).ToList();
                foreach (var termin in termini)
                {
                    _context.Termini.Remove(termin);
                }
                _context.PredavaciKursevi.Remove(pk);
            }

            // Ako je taj pREDAVAC User brisi i njega
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var user = userManager.FindByEmail(predavac.MejlAdresa); // email mi je jedinstveni identifikator tu
            if (user != null)
            {
                userManager.Delete(user);
            }

            _context.Predavaci.Remove(predavac);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        [Authorize(Roles ="Admin")]
        public ActionResult Neodobreni()
        {
            var neodobreniPredavaci = _context.Predavaci.Where(p => p.Odobrenje == false).ToList();
            return View(neodobreniPredavaci);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult SacuvajOdobrenje(List<Predavac> predavci)
        {
            if (predavci == null)
            {
                return RedirectToAction("Neodobreni");
            }

            try
            {
                foreach (var predavac in predavci)
                {
                    var predavacInDb = _context.Predavaci.Single(p => p.PredavacID == predavac.PredavacID);
                    if (predavacInDb != null)
                    {
                        predavacInDb.Odobrenje = predavac.Odobrenje;
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "Predavac");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do greške prilikom čuvanja promena.");
                return View("Neodobreni", predavci); 
            }
        }

    }
}
