using films_website.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace films_website.NewFolder1
{
    public class MovieFormViewModel
    {
        
        public int Id { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }



        [Required, StringLength(2500)]
        public string StoryLine { get; set; }

        [Range(1, 10)]
        public double Rate { get; set; }

        [ValidateNever]
        [Display(Name = "Select poster...")]
        public byte[] Poster { get; set; }

        // Connect 2 tables (FK)

        [Display(Name = "Genre")]
        public byte GenreId { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

    }
}


