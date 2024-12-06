using System.Linq;
using System.Web.Mvc;
using FakeCET.Models;
using FakeCET.ViewModels;

namespace FakeCET.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var viewModel = new PredavacKursViewModel();

            // join radi prikaza podataka
            var predavacKursevi = (from pk in _context.PredavaciKursevi
                                   join p in _context.Predavaci on pk.PredavacID equals p.PredavacID
                                   join k in _context.Kursevi on pk.KursID equals k.KursID
                                   select new
                                   {
                                       Predavac = p,
                                       Kurs = k
                                   }).ToList();

            viewModel.PredavacKursevi = predavacKursevi.Select(x => new PredavacKursViewModel
            {
                Predavac = x.Predavac,
                Kurs = x.Kurs
            }).ToList();

            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult DodeliPredavaca()
        {
            var viewModel = new DodeliPredavacaViewModel
            {
                Predavaci = _context.Predavaci.ToList(),
                Kursevi = _context.Kursevi.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DodeliPredavaca(DodeliPredavacaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var predavacKurs = new PredavacKurs
                {
                    KursID = viewModel.KursID,
                    PredavacID = viewModel.PredavacID
                };

                _context.PredavaciKursevi.Add(predavacKurs);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Predavaci = _context.Predavaci.ToList();
            viewModel.Kursevi = _context.Kursevi.ToList();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Kursevi()
        {
            var kursevi = _context.Kursevi.ToList();
            return View(kursevi);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Predavaci()
        {
            var predavaci = _context.Predavaci.ToList();
            return View(predavaci);
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
