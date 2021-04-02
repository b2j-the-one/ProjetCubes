using Cubes.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cubes.Web.Helpers
{
    public class FilesHelper : IDisposable
    {
        private static DataContext db = new DataContext();
        private static ApplicationDbContext userContext = new ApplicationDbContext();

        public static ApplicationUser GetUserASP(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            return userASP;
        }

        // Télécharger fichier
        public static string UploadFile(HttpPostedFileBase file, string folder)
        {
            string path = string.Empty;
            string pic = string.Empty;

            if (file != null)
            {
                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                file.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }

            return pic;
        }

        // Télécharger fichier
        //public static string UploadFie(HttpPostedFileBase file)
        //{
        //    string path = HttpContext.Current.Server.MapPath("~/Content/Files");
        //    string fileName = Path.GetFileName(file.FileName);

        //    string fullPath = Path.Combine(path, fileName);
        //    file.SaveAs(fullPath);

        //    return pic;
        //}

        // Créer utilisateur ASP
        public static void CreateASPUser(UserView userView)
        {
            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Créer le rôle de l'utilisateur
            string roleName = "Citoyen";

            // Vérifie si le rôle existe, sinon on le crée
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }

            // Créer l'utilisateur ASP NET
            var userASP = new ApplicationUser
            {
                UserName = userView.Email,
                Email = userView.Email
            };

            userManager.Create(userASP, userASP.UserName);

            // Ajoute le rôle de l'utilisateur
            userASP = userManager.FindByName(userView.Email);
            userManager.AddToRole(userASP.Id, "Citoyen");
        }

        // Modifier son compte
        public static void ChangerEmail(string emailActuel, UserChange user)
        {
            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(emailActuel);

            if (userASP == null)
            {
                return;
            }

            userManager.Delete(userASP);

            userASP = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email
            };

            userManager.Create(userASP, user.CurrentPassword);
            userManager.AddToRole(userASP.Id, "Citoyen");
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}