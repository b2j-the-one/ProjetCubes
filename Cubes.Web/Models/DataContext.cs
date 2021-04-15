using System.Data.Entity;

namespace Cubes.Web.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Ressource> Ressources { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Commentaire> Commentaires { get; set; }
    }
}