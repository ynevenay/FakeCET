using FakeCET.Models;
using FakeCET.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FakeCET.Controllers
{
    public class TerminController : Controller
    {
        private ApplicationDbContext _context;

        public TerminController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var termini = _context.Termini
                .Select(t => new TerminViewModel
                {
                    Termin = t,
                    KursNaziv = t.PredavacKurs.Kurs.Naziv,
                    PredavacIme = t.PredavacKurs.Predavac.Ime,
                    PredavacPrezime = t.PredavacKurs.Predavac.Prezime,
                    MestoNaziv = t.MestoOdrzavanja.Naziv
                })
                .ToList();

            return View(termini);
        }


        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var termin = _context.Termini
                .Include(t => t.PredavacKurs)
                .Include(t => t.MestoOdrzavanja)
                .SingleOrDefault(t => t.TerminID == id);

            if (termin == null)
                return HttpNotFound();

            return View(termin);
        }

        //predavac moze da zakaze
        [Authorize(Roles = "Predavac")]
        public ActionResult Create()
        {

            var viewModel = new TerminViewModel
            {
                Kursevi = _context.Kursevi.ToList(),
                Predavaci = _context.Predavaci.ToList(),
                MestaOdrzavanja = _context.MestaOdrzavanja.ToList()
            };

            return View(viewModel);
        }

        //predavac moze da zakaze
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Predavac")]
        public ActionResult Create(Termin termin)
        {
            if (ModelState.IsValid)
            {
                _context.Termini.Add(termin);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var viewModel = new TerminViewModel
            {
                Kursevi = _context.Kursevi.ToList(),
                Predavaci = _context.Predavaci.ToList(),
                MestaOdrzavanja = _context.MestaOdrzavanja.ToList()
            };

            return View(viewModel);
        }

        //predavac
        [Authorize(Roles = "Predavac")]
        public ActionResult Edit(int id)
        {
            var loggedInUserEmail = User.Identity.Name; 

            var termin = _context.Termini
                .Include(t => t.PredavacKurs)
                .SingleOrDefault(t => t.TerminID == id && t.PredavacKurs.Predavac.MejlAdresa == loggedInUserEmail);

            if (termin == null)
            {
                return HttpNotFound();
            }

            var viewModel = new TerminViewModel
            {
                Termin = termin,
                Kursevi = _context.Kursevi.ToList(),
                Predavaci = _context.Predavaci.ToList(),
                MestaOdrzavanja = _context.MestaOdrzavanja.ToList(),
                KursID = termin.PredavacKurs.KursID,
                PredavacID = termin.PredavacKurs.PredavacID,
                MestoID = termin.MestoID
            };

            return View(viewModel);
        }

        // predavac moze da menja
        [Authorize(Roles ="Predavac")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TerminViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Kursevi = _context.Kursevi.ToList();
                viewModel.Predavaci = _context.Predavaci.ToList();
                viewModel.MestaOdrzavanja = _context.MestaOdrzavanja.ToList();
                return View(viewModel);
            }

            var terminInDb = _context.Termini
                .Include(t => t.PredavacKurs)
                .SingleOrDefault(t => t.TerminID == viewModel.Termin.TerminID);

            if (terminInDb == null)
            {
                return HttpNotFound();
            }

            // iz ViwvModela u db entitete
            terminInDb.DatumPocetka = viewModel.Termin.DatumPocetka;
            terminInDb.DatumZavrsetka = viewModel.Termin.DatumZavrsetka;
            terminInDb.MestoID = viewModel.MestoID;
            terminInDb.Status = viewModel.Termin.Status;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // admin
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var termin = _context.Termini.SingleOrDefault(t => t.TerminID == id);
            if (termin == null)
                return HttpNotFound();

            return View(termin);
        }

        // admin
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var termin = _context.Termini.SingleOrDefault(t => t.TerminID == id);
            if (termin == null)
                return HttpNotFound();

            _context.Termini.Remove(termin);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
