using Cubes.Web.Migrations;
using Cubes.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Cubes.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            this.VerifierSuperUser();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void VerifierSuperUser()
        {
            var db = new DataContext();
            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            this.VerifierRole("Administrateur", userContext);
            this.VerifierRole("Moderateur", userContext);
            this.VerifierRole("Utilisateur", userContext);

            // on définit l'administrateur
            var user = db.Users.Where(u => u.Email.ToLower().Equals("borjessatwood@gmail.com")).FirstOrDefault();
            // si l'utilisateur n'existe pas dans la BDD, on le crée
            if (user == null)
            {
                user = new User
                {
                    Prenom = "Borgeah",
                    Nom = "Matongo",
                    Email = "borjessatwood@gmail.com",
                    DateNaissance = DateTime.Now,
                    DateInscription = DateTime.Now,
                    IsActivated = true,
                    Telephone = string.Empty,
                    Photo = "~/Content/Photos/user_avatar.png"
                };

                // on ajoute l'utilisateur dans la BDD
                db.Users.Add(user);
                db.SaveChanges();
            }

            var userASP = userManager.FindByName(user.Email);

            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email
                };

                userManager.Create(userASP, "123456");
            }

            userManager.AddToRole(userASP.Id, "Administrateur");
        }

        private void VerifierRole(string roleName, ApplicationDbContext userContext)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Vérifie si le rôle existe, sinon on le crée
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }
    }
}
