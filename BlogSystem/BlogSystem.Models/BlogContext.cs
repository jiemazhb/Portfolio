using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BlogSystem.Models
{
    public class BlogContext:DbContext
    {
        public BlogContext():base("con")
        {
            Database.SetInitializer<BlogContext>(new CreateDatabaseIfNotExists<BlogContext>());
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleToCategory> ArticleToCategories { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Fans> Fans { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

    }
}
