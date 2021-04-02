using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cubes.Web.Models;
using Cubes.Web.Helpers;

namespace Cubes.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderateur, Citoyen")]
    public class RessourcesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Ressources
        public async Task<ActionResult> Index()
        {
            var ressources = db.Ressources.Include(r => r.Categorie);
            return View(await ressources.OrderBy(r => r.Nom).ToListAsync());
        }

        // GET: Ressources/Details/5
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
            return View(ressource);
        }

        // GET: Ressources/Create
        public ActionResult Create()
        {
            ViewBag.IdCategorie = new SelectList(db.Categories.OrderBy(c => c.Libelle), "IdCategorie", "Libelle");
            return View();
        }

        // Créer ressources
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

                var ressource = ToRessource(ressourceView);
                ressource.Fichier = file;

                db.Ressources.Add(ressource);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.IdCategorie = new SelectList(db.Categories, "IdCategorie", "Libelle", ressourceView.IdCategorie);
            return View(ressourceView);
        }

        private Ressource ToRessource(RessourceView ressourceView)
        {
            return new Ressource
            {
                Id = ressourceView.Id,
                IdCategorie = ressourceView.IdCategorie,
                Nom = ressourceView.Nom,
                Fichier = ressourceView.Fichier,
                Titre = ressourceView.Titre,
                IsPrivate = true,
                IsFavorite = false,
                IsPublish = false,
                IsValid = false,
                DateCreation = DateTime.Now,
                //DatePublication = null,
                Description = ressourceView.Description,
            };
        }

        // GET: Ressources/Edit/5
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
            ViewBag.IdCategorie = new SelectList(db.Categories, "IdCategorie", "Libelle", ressource.IdCategorie);
            return View(ressource);
        }

        // POST: Ressources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nom,IdCategorie,Titre,IsPrivate,IsValid,IsPublish,IsFavorite,Fichier,Description")] Ressource ressource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ressource).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategorie = new SelectList(db.Categories, "IdCategorie", "Libelle", ressource.IdCategorie);
            return View(ressource);
        }

        // GET: Ressources/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(ressource);
        }

        // POST: Ressources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ressource ressource = await db.Ressources.FindAsync(id);
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
