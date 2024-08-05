using Microsoft.EntityFrameworkCore;
using films_website.NewFolder1;

namespace films_website.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
        }

        public DbSet<Genre> Geners { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<films_website.NewFolder1.MovieFormViewModel>? MovieFormViewModel { get; set; }

    }

}
