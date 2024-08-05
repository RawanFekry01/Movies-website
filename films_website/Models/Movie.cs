using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace films_website.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required , MaxLength(250)]
        public string Title { get; set; }
        
        public int Year { get; set; }

        [Required, MaxLength(2500)]
        public string StoryLine { get; set; }
        
        public double Rate { get; set; }

        [Required]
        public byte[] Poster  { get; set; }

         // Connect 2 tables (FK)
        public byte GenreId { get; set; }

        public Genre Genre { get; set; }
        
    }
}
