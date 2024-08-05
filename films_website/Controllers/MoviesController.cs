using films_website.Models;
using films_website.NewFolder1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace films_website.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;


        // initalize context in constructor to use it inside the controller 
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // indec action select all movies from database and sent them to the view 
        public async Task<IActionResult> Index()
        {
            // access database tables which have refence 
            // Movies => is a list contain all movies on database
            var Movies = await _context.Movies.ToListAsync();
            // send movies again to front
            return View(Movies);
        }



        public async Task<IActionResult> Create()
        {
            // value population
            var veiwModel = new MovieFormViewModel
            {
                Genres = await _context.Geners.OrderBy(M => M.Name).ToListAsync()

            };
            // send view mode that create movie form eill deal with
            return View(veiwModel);
        }


        [HttpPost] //action type 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            model.Genres = await _context.Geners.OrderBy(m => m.Name).ToListAsync();
            ModelState.Remove("Genres");
            
            foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
            {

            }

            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            // make sure there is attached file(poster)

            // Request all files attached with the form
            var files = Request.Form.Files;

            if (!files.Any())
            {

                ModelState.AddModelError("Poster", "Please select movie poster!");
                return PartialView("_Form", model);
            }
            var poster = files.FirstOrDefault();
            var _allowedExtenstions = new List<string> { ".jpg", ".png" };

            if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {

                ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                return PartialView("_Form", model);
            }

            //var _maxAllowedPosterSize = 1048576;
            if (poster.Length > 1048576)
            {

                ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                return PartialView("_Form", model);
            }

            // save data
            using var dataStream = new MemoryStream();
            await poster.CopyToAsync(dataStream);



            // map values from view model to the model database
            // database type => movie but type here is MovieFormViewModel
            var movies = new Movie
            {
                Title = model.Title,
                GenreId = model.GenreId,
                Year = model.Year,
                Rate = model.Rate,
                StoryLine = model.StoryLine,
                Poster = dataStream.ToArray()
            };
            _context.Movies.Add(movies);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();


            // I need to show all data while editng
            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                Rate = movie.Rate,
                Year = movie.Year,
                StoryLine = movie.StoryLine,
                Poster = movie.Poster,

                Genres = await _context.Geners.OrderBy(m => m.Name).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
          
            model.Genres = await _context.Geners.OrderBy(m => m.Name).ToListAsync();
            ModelState.Remove("Genres");

            foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
            {

            }
            if (!ModelState.IsValid)
            {
              
                return PartialView("~/Views/Shared/EditorTemplates/MovieFormViewModel.cshtml", model);
            }

            var movie = await _context.Movies.FindAsync(model.Id);

            if (movie == null)
                return NotFound();
            var files = Request.Form.Files;

            if (files.Any())
            {
                var poster = files.FirstOrDefault();

                using var dataStream = new MemoryStream();

                await poster.CopyToAsync(dataStream);

                model.Poster = dataStream.ToArray();

                var _allowedExtenstions = new List<string> { ".jpg", ".png" };
                if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    model.Genres = await _context.Geners.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                    return PartialView("~/Views/Shared/EditorTemplates/MovieFormViewModel.cshtml", model);
                }

                if (poster.Length > 1048576)
                {
                    model.Genres = await _context.Geners.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                    return PartialView("~/Views/Shared/EditorTemplates/MovieFormViewModel.cshtml", model);
                }

                movie.Poster = model.Poster;
            }



            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.StoryLine = model.StoryLine;

            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return Ok();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }
    }
}

