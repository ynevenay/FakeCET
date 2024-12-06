using FakeCET.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FakeCET.Startup))]
namespace FakeCET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Kreiraj role ako ne postoje
                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole { Name = "Admin" };
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("Polaznik"))
                {
                    var role = new IdentityRole { Name = "Polaznik" };
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("Predavac"))
                {
                    var role = new IdentityRole { Name = "Predavac" };
                    roleManager.Create(role);
                }

                var adminEmail = "admin@cet.rs";
                var adminUser = userManager.FindByEmail(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail
                    };
                    var result = userManager.Create(adminUser, "Password123!Admin"); 
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }

                var predavacEmail = "predavac@cet.rs";
                var predavacUser = userManager.FindByEmail(predavacEmail);
                if (predavacUser == null)
                {
                    predavacUser = new ApplicationUser
                    {
                        UserName = predavacEmail,
                        Email = predavacEmail
                    };
                    var result = userManager.Create(predavacUser, "Password123!Predavac");
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(predavacUser.Id, "Predavac");
                    }
                }

                var polaznikEmail = "polaznik@cet.rs";
                var polaznikUser = userManager.FindByEmail(polaznikEmail);
                if (polaznikUser == null)
                {
                    polaznikUser = new ApplicationUser
                    {
                        UserName = polaznikEmail,
                        Email = polaznikEmail
                    };
                    var result = userManager.Create(polaznikUser, "Password123!Polaznik");
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(polaznikUser.Id, "Polaznik");
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
