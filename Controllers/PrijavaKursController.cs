using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FakeCET.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace FakeCET.Controllers
{
    public class PrijavaKursController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        [Authorize(Roles ="Polaznik")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var polaznik = _context.Polaznici
                                   .Include(p => p.PrijavaKurs)
                                   .FirstOrDefault(p => p.MejlAdresa == User.Identity.Name);

            if (polaznik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Polaznik not found.");
            }

            //            var termini = _context.Termini.Where(t=>t.Status == false)

            var termini = _context.Termini
                            .Include(t => t.PredavacKurs)
                            .Include(t => t.PredavacKurs.Kurs)
                            .Include(t => t.PredavacKurs.Predavac)
                            .ToList();

            var prijave = _context.PrijaveKursevi
                            .Where(p => p.PolaznikID == polaznik.PolaznikID)
                            .ToList();

            var viewModel = termini.Select(t => new PrijavaKursViewModel
            {
                TerminID = t.TerminID,
                NazivKursa = t.PredavacKurs.Kurs.Naziv,
                ImePredavaca = t.PredavacKurs.Predavac.Ime,
                PrezimePredavaca = t.PredavacKurs.Predavac.Prezime,
                DatumPocetka = t.DatumPocetka.Value,
                DatumZavrsetka = t.DatumZavrsetka.Value,
                JePrijavljen = prijave.Any(p => p.TerminID == t.TerminID)
            }).ToList();

            return View(viewModel);
        }

        // Akcija za prijavu na termin
        [HttpPost]
        [Authorize(Roles = "Polaznik")]
        public ActionResult PrijaviSe(int terminId)
        {
            var userId = User.Identity.GetUserId();
            var polaznik = _context.Polaznici
                                   .Include(p => p.PrijavaKurs)
                                   .FirstOrDefault(p => p.MejlAdresa == User.Identity.Name);

            if (polaznik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Polaznik not found.");
            }

            if (_context.PrijaveKursevi.Any(p => p.TerminID == terminId && p.PolaznikID == polaznik.PolaznikID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Već ste prijavljeni na ovaj termin.");
            }

            var prijava = new PrijavaKurs
            {
                TerminID = terminId,
                PolaznikID = polaznik.PolaznikID,
                DatumPrijave = DateTime.Now,
                Status = -1 // Default status
            };

            _context.PrijaveKursevi.Add(prijava);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Pregled prijava za polaznika
        [Authorize(Roles = "Polaznik")]
        public ActionResult MojePrijave()
        {
            var userId = User.Identity.GetUserId();
            var polaznik = _context.Polaznici
                                   .Include(p => p.PrijavaKurs)
                                   .FirstOrDefault(p => p.MejlAdresa == User.Identity.Name);

            if (polaznik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Polaznik not found.");
            }

            var viewModel = _context.PrijaveKursevi
                                .Where(p => p.PolaznikID == polaznik.PolaznikID)
                                .Include(p => p.Termin)
                                .Include(p => p.Termin.PredavacKurs)
                                .Include(p => p.Termin.PredavacKurs.Kurs)
                                .Include(p => p.Termin.PredavacKurs.Predavac)
                                .Select(p => new PrijavaKursViewModel
                                {
                                    TerminID = p.TerminID,
                                    NazivKursa = p.Termin.PredavacKurs.Kurs.Naziv,
                                    ImePredavaca = p.Termin.PredavacKurs.Predavac.Ime,
                                    PrezimePredavaca = p.Termin.PredavacKurs.Predavac.Prezime,
                                    DatumPocetka = p.Termin.DatumPocetka.Value,
                                    DatumZavrsetka = p.Termin.DatumZavrsetka.Value,
                                    Status = p.Status  // iz bp u vievmodel
                                })
                                .ToList();

            return View(viewModel);
        }


        [Authorize(Roles = "Admin, Predavac")]
        public ActionResult SvePrijave()
        {
            var prijave = _context.PrijaveKursevi
                                  .Include(p => p.Polaznik)
                                  .Include(p => p.Termin)
                                  .Include(p => p.Termin.PredavacKurs)
                                  .Include(p => p.Termin.PredavacKurs.Kurs)
                                  .Include(p => p.Termin.PredavacKurs.Predavac)
                                  .ToList();

            var viewModel = prijave.Select(p => new PrijavaKursViewModel
            {
                PrijavaID = p.PrijavaID,
                TerminID = p.TerminID,
                NazivKursa = p.Termin.PredavacKurs.Kurs.Naziv,
                ImePredavaca = p.Termin.PredavacKurs.Predavac.Ime,
                PrezimePredavaca = p.Termin.PredavacKurs.Predavac.Prezime,
                DatumPocetka = p.Termin.DatumPocetka.Value,
                DatumZavrsetka = p.Termin.DatumZavrsetka.Value,
                ImePolaznika = p.Polaznik.Ime,
                PrezimePolaznika = p.Polaznik.Prezime,
                Status = p.Status
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PromeniStatus(int prijavaId, int status)
        {
            var prijava = _context.PrijaveKursevi.SingleOrDefault(p => p.PrijavaID == prijavaId);

            if (prijava == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Prijava not found.");
            }

            prijava.Status = status;
            _context.SaveChanges();

            return RedirectToAction("SvePrijave");
        }

        [HttpPost]
        [Authorize(Roles = "Polaznik")]
        public ActionResult ObrisiPrijavu(int terminId)
        {
            var userId = User.Identity.GetUserId();
            var polaznik = _context.Polaznici
                                   .Include(p => p.PrijavaKurs)
                                   .FirstOrDefault(p => p.MejlAdresa == User.Identity.Name);

            if (polaznik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Polaznik not found.");
            }

            var prijava = _context.PrijaveKursevi
                                .FirstOrDefault(p => p.TerminID == terminId && p.PolaznikID == polaznik.PolaznikID);

            if (prijava == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Prijava not found.");
            }

            _context.PrijaveKursevi.Remove(prijava);
            _context.SaveChanges();

            return RedirectToAction("MojePrijave");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
