using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Cubes.Web.Models;
using System.Linq;
using Cubes.Web.Helpers;
using System;

namespace Cubes.Web.Controllers
{
    public class RessourcesController : Controller
    {
        private DataContext db = new DataContext();

        // Poster un commentaire
        public async Task<ActionResult> PostComment(CommentView view)
        {
            // On récupère la séssion de l'utilisateur connecté
            int idUser = Convert.ToInt32(Session["IdUser"]);
            if (idUser == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            Commentaire c = new Commentaire();
            c.Text = view.CommentText;
            c.IdUser = idUser;
            c.IdRessource = view.IdRessource;
            c.DateCommentaire = DateTime.Now;

            db.Commentaires.Add(c);
            await db.SaveChangesAsync();

            return RedirectToAction($"{nameof(Publications)}");
        }

        // Liste des publications public
        public async Task<ActionResult> Publications(string search)
        {
            var ressources = db.Ressources.Where(r => r.IsPublish == true)
                                          .Where(r => r.IsValid == true)
                                          .Where(r => r.IsPrivate == true)
                                          .Include(r => r.Categorie)
                                          .Include(m => m.Commentaires);



            //var ressources = from r in db.Ressources
            //                 select r;

            if (!String.IsNullOrEmpty(search))
            {
                ressources = ressources.Where(r => r.Categorie.Libelle.Contains(search));
            }

            return View(await ressources.OrderByDescending(r => r.DatePublication).ToListAsync());
        }

        // Liste des publications public
        //public async Task<ActionResult> Publications()
        //{
        //    var ressources = db.Ressources.Where(r => r.IsPublish == true)
        //                                  .Where(r => r.IsValid == true)
        //                                  .Where(r => r.IsPrivate == true)
        //                                  .Include(r => r.Categorie);
        //    return View(await ressources.OrderByDescending(r => r.DatePublication).ToListAsync());
        //}

        // Liste des publications public
        //[Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        //public async Task<ActionResult> MyPublish()
        //{

        //    var ressources = await db.Ressources.Where(r => r.IsPublish == true)
        //                                  .Where(r => r.IsValid == true)
        //                                  .Where(r => r.IsPrivate == true)
        //                                  .Where(r => r.User == )
        //                                  .Include(r => r.Categorie)
        //                                  .OrderBy(r => r.DatePublication).ToListAsync();
        //    return View(ressources);
        //}

        // Publier la ressource
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> Publish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ressource ressource = await db.Ressources.FindAsync(id);

            if(ressource != null)
            {
                if(ressource.IsPublish == false)
                {
                    ressource.IsPublish = true;
                    ressource.DatePublication = DateTime.Now;
                    ressource.IsPrivate = true;

                    db.Entry(ressource).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // Valider une ressource pour publication
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> Validate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ressource ressource = await db.Ressources.FindAsync(id);

            if (ressource != null)
            {
                if (ressource.IsPublish == true && ressource.IsPrivate == true)
                {
                    ressource.IsValid = true;

                    db.Entry(ressource).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // Suspendre ressource
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> Suspendre(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ressource ressource = await db.Ressources.FindAsync(id);

            if (ressource != null)
            {
                if (ressource.IsPublish == true && ressource.IsPrivate == true)
                {
                    ressource.IsValid = false;

                    db.Entry(ressource).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // Changer photo utilisateur
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> ChangeFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ressource ressource = await db.Ressources.FindAsync(id);
            if (ressource == null)
            {
                return HttpNotFound();
            }

            var view = new RessourceFileView
            {
                IdRessource = ressource.IdRessource,
                IdCategorie = ressource.IdCategorie,
                Nom = ressource.Nom,
                IsPrivate = ressource.IsPrivate,
                Titre = ressource.Titre,
                Description = ressource.Description,
                Fichier = ressource.Fichier,
            };

            ViewBag.IdCategorie = new SelectList(db.Categories, "IdCategorie", "Libelle", ressource.IdCategorie);
            return View(view);
        }


        // Changer photo utilisateur
        [HttpPost]
        public async Task<ActionResult> ChangeFile(RessourceFileView view)
        {
            var file = string.Empty;
            var folder = "~/Content/Files";

            // Charger la photo
            if (view.NewFile != null)
            {
                file = FilesHelper.UploadFile(view.NewFile, folder);
                file = string.Format("{0}/{1}", folder, file);
            }
            else
            {
                file = "~/Content/Files/noimage.png";
            }

            var ressource = new Ressource
            {

                IdRessource = view.IdRessource,
                IdCategorie = view.IdCategorie,
                Nom = view.Nom,
                IsPrivate = view.IsPrivate,
                Titre = view.Titre,
                Description = view.Description,
                Fichier = view.Fichier,
            };

            ressource.Fichier = file;
            db.Entry(ressource).State = EntityState.Modified;
            await db.SaveChangesAsync();

            ViewBag.IdCategorie = new SelectList(db.Categories, "IdCategorie", "Libelle", ressource.IdCategorie);
            return RedirectToAction($"{nameof(Details)}/{ressource.IdRessource}");
        }

        // Liste des ressources
        public async Task<ActionResult> Index()
        {
            var ressources = db.Ressources.Include(r => r.Categorie);
            return View(await ressources.OrderBy(r => r.Nom).ToListAsync());
        }

        // GET: Ressources/Details/5
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ressource ressource = await db.Ressources.FindAsync(id);
            if (ressource == null)
            {
                return HttpNotFound();
            }

            var ressourceView = ToView(ressource);

            ViewBag.IdCategorie = new SelectList(db.Categories, "IdCategorie", "Libelle", ressource.IdCategorie);
            return View(ressourceView);
        }

        // Créer ressource / GET
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public ActionResult Create()
        {
            ViewBag.IdCategorie = new SelectList(db.Categories.OrderBy(c => c.Libelle), "IdCategorie", "Libelle");
            return View();
        }

        // Créer ressource / POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RessourceView ressourceView)
        {
            if (ModelState.IsValid)
            {
                var file = string.Empty;
                var folder = "~/Content/Files";

                // Charger le fichier
                if (ressourceView.FichierFile != null && ressourceView.FichierFile.ContentLength < 104857600)
                {
                    file = FilesHelper.UploadFile(ressourceView.FichierFile, folder);
                    file = string.Format("{0}/{1}", folder, file);
                }
                else
                {
                    file = "~/Content/Files/noimage.png";
                }

                // On trouve l'utilisateur connecté
                var emailUser = this.User.Identity.Name;
                var users = db.Users.ToList();

                var utilisateur = from user in users
                                  where user.Email == emailUser
                                  select new { user.NomComplet };

                foreach (var user in utilisateur)
                {
                    var userFind = user.NomComplet;

                    var ressource = new Ressource
                    {
                        IdRessource = ressourceView.IdRessource,
                        IdCategorie = ressourceView.IdCategorie,
                        Nom = ressourceView.Nom,
                        Fichier = ressourceView.Fichier,
                        Titre = ressourceView.Titre,
                        IsPrivate = true,
                        IsFavorite = false,
                        IsPublish = false,
                        IsValid = false,
                        DateCreation = DateTime.Now,
                        Description = ressourceView.Description,
                        User = userFind
                    };

                    ressource.Fichier = file;

                    db.Ressources.Add(ressource);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            ViewBag.IdCategorie = new SelectList(db.Categories.OrderBy(c => c.Libelle), "IdCategorie", "Libelle", ressourceView.IdCategorie);
            return View(ressourceView);
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ressource ressource = await db.Ressources.FindAsync(id);
            if (ressource == null)
            {
                return HttpNotFound();
            }

            var ressourceView = ToView(ressource);

            ViewBag.IdCategorie = new SelectList(db.Categories.OrderBy(c => c.Libelle), "IdCategorie", "Libelle", ressource.IdCategorie);
            return View(ressourceView);
        }

        // Elements affichés pour la modification
        private RessourceView ToView(Ressource ressource)
        {
            return new RessourceView
            {
                IdRessource = ressource.IdRessource,
                IdCategorie = ressource.IdCategorie,
                Nom = ressource.Nom,
                IsPrivate = ressource.IsPrivate,
                Titre = ressource.Titre,
                Description = ressource.Description,
                Fichier = ressource.Fichier,
                User = ressource.User
            };
        }

        // POST: Ressources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RessourceView ressourceView)
        {
            if (ModelState.IsValid)
            {
                var file = string.Empty;
                var folder = "~/Content/Files";

                // Charger le fichier
                if (ressourceView.FichierFile != null && ressourceView.FichierFile.ContentLength < 104857600)
                {
                    file = FilesHelper.UploadFile(ressourceView.FichierFile, folder);
                    file = string.Format("{0}/{1}", folder, file);
                }
                else
                {
                    file = "~/Content/Files/noimage.png";
                }

                var ressource = ToRessource1(ressourceView);
                ressource.Fichier = file;

                db.Entry(ressource).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.IdCategorie = new SelectList(db.Categories.OrderBy(c => c.Libelle), "IdCategorie", "Libelle", ressourceView.IdCategorie);
            return View(ressourceView);
        }

        private Ressource ToRessource1(RessourceView ressourceView)
        {
            return new Ressource
            {
                IdRessource = ressourceView.IdRessource,
                IdCategorie = ressourceView.IdCategorie,
                Nom = ressourceView.Nom,
                Fichier = ressourceView.Fichier,
                Titre = ressourceView.Titre,
                IsPrivate = ressourceView.IsPrivate,
                Description = ressourceView.Description,
                User = ressourceView.User
            };
        }

        // Supprimer la ressource
        [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
        public async Task<ActionResult> Delete(int? id)
        {
            var ressource = await db.Ressources.FindAsync(id);

            db.Ressources.Remove(ressource);
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
