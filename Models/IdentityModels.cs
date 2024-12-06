using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FakeCET.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Predavac> Predavaci { get; set; }
        public DbSet<Oblast> Oblasti { get; set; }
        public DbSet<Kurs> Kursevi { get; set; }
        public DbSet<PredavacKurs> PredavaciKursevi { get; set; }
        public DbSet<MestoOdrzavanja> MestaOdrzavanja {  get; set; }
        public DbSet<Polaznik> Polaznici { get; set; }
        public DbSet<Termin> Termini { get; set; }
        public DbSet<PrijavaKurs> PrijaveKursevi { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}