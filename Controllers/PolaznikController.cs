using FakeCET.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace FakeCET.Controllers
{
    public class PolaznikController : Controller
    {
        private ApplicationDbContext _context;

        public PolaznikController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Authorize(Roles = "Admin, Predavac")]
        public ActionResult Index()
        {
            var polaznici = _context.Polaznici.ToList();
            return View(polaznici);
        }

        [Authorize(Roles ="Admin, Predavac")]
        public ActionResult Details(int id)
        {
            var polaznik = _context.Polaznici.SingleOrDefault(p => p.PolaznikID == id);
            if (polaznik == null)
                return HttpNotFound();

            return View(polaznik);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(Polaznik polaznik)
        {
            if (ModelState.IsValid)
            {
                var pass = polaznik.Lozinka;
                var confPass = polaznik.PotvrdiLozinku;

                if (pass != confPass)
                {
                    ModelState.AddModelError("PotvrdaLozinke", "Lozinke se nee poklapaju");
                    return View(polaznik);
                }
                // Polaznik postaje user
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var newUser = new ApplicationUser
                {
                    UserName = polaznik.MejlAdresa,
                    Email = polaznik.MejlAdresa
                };
                var result = userManager.Create(newUser, pass);

                if (result.Succeeded)
                {
                    // Dobija rolu polaznik posto je user
                    userManager.AddToRole(newUser.Id, "Polaznik");

                    // cuva se i u polaznik tabeli, mozda je lose jer se sifra koja nije heshovana cuva
                    _context.Polaznici.Add(polaznik);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Greška prilikom kreiranja korisnika.");
                    return View(polaznik);
                }
            }

            return View(polaznik);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var polaznik = _context.Polaznici.SingleOrDefault(p => p.PolaznikID == id);
            if (polaznik == null)
                return HttpNotFound();

            return View(polaznik);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, string kontaktTelefon)
        {
            var polaznikInDb = _context.Polaznici.SingleOrDefault(p => p.PolaznikID == id);
            if (polaznikInDb == null)
                return HttpNotFound();

            polaznikInDb.KontaktTelefon = kontaktTelefon;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [Authorize (Roles ="Admin")]
        public ActionResult Delete(int id)
        {
            var polaznik = _context.Polaznici.SingleOrDefault(p => p.PolaznikID == id);
            if (polaznik == null)
                return HttpNotFound();

            return View(polaznik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var polaznik = _context.Polaznici.SingleOrDefault(p => p.PolaznikID == id);
            if (polaznik == null)
                return HttpNotFound();

            // Pronađi sve prijave koje pripadaju ovom polazniku i obriši ih
            var prijave = _context.PrijaveKursevi.Where(p => p.PolaznikID == id).ToList();
            foreach (var prijava in prijave)
            {
                _context.PrijaveKursevi.Remove(prijava);
            }

            // Sačuvaj izmene u bazi
            _context.SaveChanges();

            // Sada obriši korisnika iz AspNetUsers tabele
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var user = userManager.FindByEmail(polaznik.MejlAdresa);
            if (user != null)
            {
                userManager.Delete(user);
            }

            // Na kraju obriši polaznika iz Polaznici tabele
            _context.Polaznici.Remove(polaznik);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
