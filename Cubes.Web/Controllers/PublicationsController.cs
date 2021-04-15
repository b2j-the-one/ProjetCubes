using Cubes.Web.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cubes.Web.Controllers
{
    [Authorize]
    public class PublicationsController : Controller
    {
        private DataContext db = new DataContext();

        // Liste des publications
        public async Task<ActionResult> Index()
        {
            var ressources = db.Ressources.Where(r => r.IsPublish == true)
                                          .Where(r => r.IsValid == true)
                                          .Include(r => r.Categorie);
            return View(await ressources.OrderBy(r => r.DatePublication).ToListAsync());
        }
    }
}