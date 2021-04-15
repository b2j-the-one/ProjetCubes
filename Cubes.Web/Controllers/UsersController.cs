using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Cubes.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Cubes.Web.Helpers;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Cubes.Web.Controllers
{
    public class UsersController : Controller
    {
        private DataContext db = new DataContext();

        // Définir comme Super Administrateur
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> OnOffSuperAdmin(int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user != null)
            {
                var userContext = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var userASP = await userManager.FindByEmailAsync(user.Email);

                if (userASP != null)
                {
                    if (userManager.IsInRole(userASP.Id, "SuperAdmin"))
                    {
                        userManager.RemoveFromRole(userASP.Id, "SuperAdmin");
                        //userManager.AddToRole(userASP.Id, "Citoyen");
                    }
                    else
                    {
                        userManager.AddToRole(userASP.Id, "SuperAdmin");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        // Définir comme Administrateur
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> OnOffAdmin(int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user != null)
            {
                var userContext = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var userASP = await userManager.FindByEmailAsync(user.Email);

                if (userASP != null)
                {
                    if (userManager.IsInRole(userASP.Id, "Admin"))
                    {
                        userManager.RemoveFromRole(userASP.Id, "Admin");
                        //userManager.AddToRole(userASP.Id, "Citoyen");
                    }
                    else
                    {
                        userManager.AddToRole(userASP.Id, "Admin");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        // Définir comme Modérateur
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> OnOffModerator(int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user != null)
            {
                var userContext = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var userASP = await userManager.FindByEmailAsync(user.Email);

                if (userASP != null)
                {
                    if (userManager.IsInRole(userASP.Id, "Moderateur"))
                    {
                        userManager.RemoveFromRole(userASP.Id, "Moderateur");
                        //userManager.AddToRole(userASP.Id, "Citoyen");
                    }
                    else
                    {
                        userManager.AddToRole(userASP.Id, "Moderateur");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        // Activer/Désactiver un compte
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ActivateUserAccount(int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user != null)
            {
                if (user.IsActivated == true)
                {
                    user.IsActivated = false;
                    user.DateBannissement = DateTime.Now;

                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                else
                {
                    user.IsActivated = true;
                    user.DateFinBannissement = DateTime.Now;

                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // Changer photo utilisateur
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> ChangePhoto()
        {
            // On trouve l'utilisateur connecté
            var user = await db.Users.Where(u => u.Email == this.User.Identity.Name).FirstOrDefaultAsync();

            var view = new UserProfilView
            {
                DateNaissance = user.DateNaissance,
                Prenom = user.Prenom,
                Nom = user.Nom,
                Photo = user.Photo,
                IdUser = user.IdUser,
                Email = user.Email,
                Telephone = user.Telephone
            };

            return View(view);
        }


        // Changer photo utilisateur
        [HttpPost]
        public async Task<ActionResult> ChangePhoto(UserProfilView view)
        {
            var pic = string.Empty;
            var folder = "~/Content/Files";

            // Charger la photo
            if (view.NewPhoto != null)
            {
                pic = FilesHelper.UploadFile(view.NewPhoto, folder);
                pic = string.Format("{0}/{1}", folder, pic);
            }
            else
            {
                pic = "~/Content/Files/user_avatar.png";
            }

            var user = new User
            {
                DateNaissance = view.DateNaissance,
                Prenom = view.Prenom,
                Nom = view.Nom,
                Photo = view.Photo,
                IdUser = view.IdUser,
                Email = view.Email,
                Telephone = view.Telephone,
                IsActivated = true
            };

            user.Photo = pic;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction($"{nameof(MyProfile)}/{user.IdUser}");
        }

        // Profil utilisateur
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> MyProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            string sql = "UPDATE Users SET Email = " + user.Email + ", Nom = " + user.Nom + ", Prenom = " + user.Prenom +
                                                      ", Telephone = " + user.Telephone + " WHERE Users.IdUser = " + user.IdUser;
            try
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            //// On trouve l'utilisateur connecté
            //var user = await db.Users.Where(u => u.Email == this.User.Identity.Name).FirstOrDefaultAsync();

            //var view = new UserProfilView
            //{
            //    DateNaissance = user.DateNaissance,
            //    Prenom = user.Prenom,
            //    Nom = user.Nom,
            //    Photo = user.Photo,
            //    IdUser = user.IdUser,
            //    Email = user.Email,
            //    Telephone = user.Telephone
            //};

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MyProfile(UserProfilView view)
        {
            if (ModelState.IsValid)
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                //SqlConnection connection = new SqlConnection(connectionString);
                ////var sql = @"UPDATE Users SET Email = " + view.Email + ", Nom = " + view.Nom + ", Prenom = " + view.Prenom +
                ////", Telephone = " + view.Telephone + " WHERE Users.IdUser = " + view.IdUser;

                //try
                //{
                //    connection.Open();
                //    SqlCommand cmd = new SqlCommand("UPDATE Users SET Email = " + view.Email + ", Nom = " + view.Nom + ", Prenom = " + view.Prenom +
                //                                    ", Telephone = " + view.Telephone + " WHERE Users.IdUser = " + view.IdUser);
                //    await cmd.ExecuteNonQueryAsync();
                //}
                //catch (Exception ex)
                //{
                //    ex.ToString();
                //}

                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                string sql = "UPDATE Users SET Email = " + view.Email + ", Nom = " + view.Nom + ", Prenom = " + view.Prenom +
                                                          ", Telephone = " + view.Telephone + " WHERE Users.IdUser = " + view.IdUser;
                try
                {
                    connection.Open();
                    var command = new SqlCommand(sql, connection);
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                //var user = new User
                //{
                //    IdUser = view.IdUser,
                //    Email = view.Email,
                //    Nom = view.Nom,
                //    Prenom = view.Prenom,
                //    DateNaissance = view.DateNaissance,
                //    IsActivated = true,
                //    Photo = view.Photo,
                //    Telephone = view.Telephone
                //};

                ////user.Photo = pic;
                //db.Entry(user).State = EntityState.Modified;
                //await db.SaveChangesAsync();
            }

            return View(view);
        }

        // Liste des utilisateurs
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult> Index()
        {
            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var users = await db.Users.OrderBy(u => u.Prenom).ThenBy(u => u.Nom).ToListAsync();
            var usersView = new List<UserIndexView>();

            foreach (var user in users)
            {
                // on trouve l'utilisateur ASP via son email
                var userASP = userManager.FindByEmail(user.Email);

                usersView.Add(new UserIndexView
                {
                    Photo = user.Photo,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    IsAdmin = userASP != null && userManager.IsInRole(userASP.Id, "Admin"),
                    IsSuperAdmin = userASP != null && userManager.IsInRole(userASP.Id, "SuperAdmin"),
                    IsModerator = userASP != null && userManager.IsInRole(userASP.Id, "Moderateur"),
                    Email = user.Email,
                    DateNaissance = user.DateNaissance,
                    IsActivated = user.IsActivated,
                    IdUser = user.IdUser
                });
            }

            return View(usersView);
        }

        // Infos de l'utilisateur
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // créer user
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        // créer user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView userView)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Files";

                // Charger la photo
                if (userView.PhotoFile != null)
                {
                    pic = FilesHelper.UploadFile(userView.PhotoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                else
                {
                    pic = "~/Content/Files/user_avatar.png";
                }

                var user = ToUser(userView);
                user.Photo = pic;
                db.Users.Add(user);

                try
                {
                    await db.SaveChangesAsync();
                    this.CreateASPUser(userView);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("Index_Email"))
                    {
                        ViewBag.Error = "Cet E-mail est déja utilisé par un autre utilisatur";
                    }
                    else
                    {
                        ViewBag.Error = ex.Message;
                    }

                    return View(userView);
                }

                return RedirectToAction("Index");
            }

            return View(userView);
        }

        private User ToUser(UserView userView)
        {
            return new User
            {
                IdUser = userView.IdUser,
                Email = userView.Email,
                Nom = userView.Nom,
                Prenom = userView.Prenom,
                DateNaissance = userView.DateNaissance,
                IsActivated = true,
                Photo = userView.Photo,
                DateInscription = DateTime.Now,
                Telephone = userView.Telephone
            };
        }

        private void CreateASPUser(UserView userView)
        {
            // Gerer Utilisateur
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

            // Créer l'utilisateur ASP 
            var userASP = new ApplicationUser
            {
                UserName = userView.Email,
                Email = userView.Email
            };

            // création de l'utilisateur ASP avec comme password par défaut son E-mail
            userManager.Create(userASP, userASP.UserName);

            // Ajoute le rôle de l'utilisateur
            userASP = userManager.FindByName(userView.Email);
            userManager.AddToRole(userASP.Id, "Citoyen");
        }

        // editer user
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = ToView(user);

            return View(userView);
        }

        // Elements affichés pour la modification
        private UserView ToView(User user)
        {
            return new UserView
            {
                IdUser = user.IdUser,
                Email = user.Email,
                Nom = user.Nom,
                Prenom = user.Prenom,
                DateNaissance = user.DateNaissance,
                Photo = user.Photo,
                Telephone = user.Telephone
            };
        }

        // editer user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserView userView)
        {
            if (ModelState.IsValid)
            {
                var pic = userView.Photo;
                var folder = "~/Content/Files";

                if (userView.PhotoFile != null)
                {
                    pic = FilesHelper.UploadFile(userView.PhotoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                else
                {
                    pic = "~/Content/Files/user_avatar.png";
                }

                var user = ToUser1(userView);
                user.Photo = pic;

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(userView);
        }

        private User ToUser1(UserView userView)
        {
            return new User
            {
                IdUser = userView.IdUser,
                Email = userView.Email,
                Nom = userView.Nom,
                Prenom = userView.Prenom,
                DateNaissance = userView.DateNaissance,
                IsActivated = userView.IsActivated,
                Photo = userView.Photo,
                Telephone = userView.Telephone
            };
        }

        // supprimer user
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await db.Users.FindAsync(id);

            db.Users.Remove(user);
            await db.SaveChangesAsync(); 

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
