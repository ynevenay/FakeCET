using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FakeCET.Models;
using FakeCET.ViewModels;
using PagedList;
using System.Data.Entity;

namespace FakeCET.Controllers
{
    public class KursController : Controller
    {
        private ApplicationDbContext _context;

        public KursController()
        {
            _context = new ApplicationDbContext();

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [AllowAnonymous]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? oblastId)
        {

            var kursevi = from k in _context.Kursevi.Include(k => k.Oblast)
                          select k;

            if (!String.IsNullOrEmpty(searchString))
            {
                kursevi = kursevi.Where(k => k.Naziv.Contains(searchString));
            }

            if (oblastId.HasValue)
            {
                kursevi = kursevi.Where(k => k.OblastID == oblastId.Value);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    kursevi = kursevi.OrderByDescending(k => k.Naziv);
                    break;
                case "Duration":
                    kursevi = kursevi.OrderBy(k => k.Trajanje);
                    break;
                case "duration_desc":
                    kursevi = kursevi.OrderByDescending(k => k.Trajanje);
                    break;
                case "Price":
                    kursevi = kursevi.OrderBy(k => k.Cena);
                    break;
                case "price_desc":
                    kursevi = kursevi.OrderByDescending(k => k.Cena);
                    break;
                case "Area":
                    kursevi = kursevi.OrderBy(k => k.Oblast.Naziv);
                    break;
                case "area_desc":
                    kursevi = kursevi.OrderByDescending(k => k.Oblast.Naziv);
                    break;
                default:
                    kursevi = kursevi.OrderBy(k => k.Naziv);
                    break;
            }

            ViewBag.Oblasti = new SelectList(_context.Oblasti, "OblastID", "Naziv");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(kursevi.ToPagedList(pageNumber, pageSize));
        }


        //[AllowAnonymous]
        //public ActionResult Index()
        //{
        //    var kursevi = _context.Kursevi.Include("Oblast").ToList();
        //    return View(kursevi);
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var viewModel = new KursViewModel
            {
                Oblasti = _context.Oblasti.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(KursViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var kurs = new Kurs
                {
                    Naziv = viewModel.Naziv,
                    Trajanje = viewModel.Trajanje,
                    Cena = viewModel.Cena,
                    Opis = viewModel.Opis,
                    OblastID = viewModel.OblastID
                };

                _context.Kursevi.Add(kurs);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Oblasti = _context.Oblasti.ToList();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var kurs = _context.Kursevi.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }

            var viewModel = new KursViewModel
            {
                KursID = kurs.KursID,
                Naziv = kurs.Naziv,
                Trajanje = kurs.Trajanje,
                Cena = kurs.Cena,
                Opis = kurs.Opis,
                OblastID = kurs.OblastID,
                Oblasti = _context.Oblasti.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(KursViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var kurs = _context.Kursevi.Find(viewModel.KursID);
                if (kurs == null)
                {
                    return HttpNotFound();
                }

                kurs.Naziv = viewModel.Naziv;
                kurs.Trajanje = viewModel.Trajanje;
                kurs.Cena = viewModel.Cena;
                kurs.Opis = viewModel.Opis;
                kurs.OblastID = viewModel.OblastID;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.Oblasti = _context.Oblasti.ToList();
            return View(viewModel);
        }


        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var kurs = _context.Kursevi.Include("Oblast").SingleOrDefault(k => k.KursID == id);
            if (kurs == null)
            {
                return HttpNotFound();
            }

            return View(kurs);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var kurs = _context.Kursevi.Include("Oblast").SingleOrDefault(k => k.KursID == id);
            if (kurs == null)
            {
                return HttpNotFound();
            }

            return View(kurs);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var kurs = _context.Kursevi.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }

            _context.Kursevi.Remove(kurs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
