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

namespace Cubes.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Messages
        public async Task<ActionResult> Index()
        {
            var messages = await db.Messages.Include(m => m.Replies)
                                            .OrderByDescending(m => m.DateEnvoie).ToListAsync();
            return View(messages);
        }

        // Poster un commentaire
        public async Task<ActionResult> PosterMessage(string MessageText)
        {
            // On récupère la séssion de l'utilisateur connecté
            int idUser = Convert.ToInt32(Session["IdUser"]);
            if (idUser == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            Message m = new Message();
            m.Text = MessageText;
            m.IdUser = idUser;
            m.DateEnvoie = DateTime.Now;

            db.Messages.Add(m);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Poster une réponse 
        public async Task<ActionResult> PosterReponse(ReplyView view)
        {
            // On récupère la séssion de l'utilisateur connecté
            int idUser = Convert.ToInt32(Session["IdUser"]);
            if(idUser == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            Reply r = new Reply();
            r.Text = view.Reply;
            r.IdMessage = view.IdMessage;
            r.IdUser = idUser;
            r.DateReponse = DateTime.Now;

            db.Replies.Add(r);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //// GET: Messages/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Message message = await db.Messages.FindAsync(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(message);
        //}

        //// GET: Messages/Create
        //public ActionResult Create()
        //{
        //    ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Email");
        //    return View();
        //}

        //// POST: Messages/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "IdMessage,Text,DateEnvoie,IdUser")] Message message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Messages.Add(message);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Email", message.IdUser);
        //    return View(message);
        //}

        //// GET: Messages/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Message message = await db.Messages.FindAsync(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Email", message.IdUser);
        //    return View(message);
        //}

        //// POST: Messages/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "IdMessage,Text,DateEnvoie,IdUser")] Message message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(message).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Email", message.IdUser);
        //    return View(message);
        //}

        //// GET: Messages/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Message message = await db.Messages.FindAsync(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(message);
        //}

        //// POST: Messages/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Message message = await db.Messages.FindAsync(id);
        //    db.Messages.Remove(message);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
