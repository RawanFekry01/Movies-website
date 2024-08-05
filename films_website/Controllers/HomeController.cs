using films_website.Models;
using films_website.NewFolder1;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace films_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
       
      
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IActionResult> Index()
        {

            var movies = await _context.Movies.OrderByDescending(m => m.Year).Take(3).ToListAsync();
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(movies));
            return View(movies);
        }

        [HttpPost]
        public IActionResult Read_Movies([DataSourceRequest] DataSourceRequest request)
        {
            if (request == null)
            {
                return NotFound();
            }
            // throw new Exception();
            //var movies = _context.Movies;
            var movies = _context.Movies.Select(c => new MovieFormViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Rate = c.Rate

            });
            //System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(movies));
            return Json(movies.OrderByDescending(m => m.Rate).ToList().ToDataSourceResult(request));
        }
        public IActionResult Update_Movies([DataSourceRequest] DataSourceRequest request)
        {
            if (request == null)
            {
                return NotFound();
            }
            // throw new Exception();
            //var movies = _context.Movies;
            var movies = _context.Movies.Select(c => new MovieFormViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Rate = c.Rate

            });
            //System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(movies));
            return Json(movies.ToList().ToDataSourceResult(request));
        }

        public JsonResult Get_Movies()
        {
            var movies = _context.Movies.Select(c => new MovieFormViewModel
            {
                Id = c.Id,
                Title = c.Title,
             

            });

            return Json(movies);
        }


        public IActionResult CombBox_GetMovies()
        {
            List<SelectListItem> hobbies = GetData();

            return Json(hobbies);
        }


        public JsonResult GetProducts(string text)
        {
            var movies = _context.Movies.Select(c => new MovieFormViewModel
            {
                Id = c.Id,
                Title = c.Title,


            });
            if (!string.IsNullOrEmpty(text))
            {
                movies = movies.Where(p => p.Title.Contains(text));
            }

            return Json(movies.ToList());
        }


        private static List<SelectListItem> GetData()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Football"},
                new SelectListItem { Value = "2", Text = "Golf" },
                new SelectListItem { Value = "3", Text = "Baseball" },
                new SelectListItem { Value = "4", Text = "Table Tennis" },
                new SelectListItem { Value = "5", Text = "Volleyball" },
                new SelectListItem { Value = "6", Text = "Basketball" },
                new SelectListItem { Value = "7", Text = "Boxing" },
                new SelectListItem { Value = "8", Text = "Badminton" },
                new SelectListItem { Value = "9", Text = "Cycling" },
                new SelectListItem { Value = "10", Text = "Gymnastics" },
                new SelectListItem { Value =  "11", Text = "Swimming" },
                new SelectListItem { Value =  "12", Text = "Wrestling" },
                new SelectListItem { Value =  "13", Text = "Snooker" },
                new SelectListItem { Value =  "14", Text = "Skiing" },
                new SelectListItem { Value =  "15", Text = "Handball" }
            };
        }
        public async Task<IActionResult> _Recent()
        {

            var movies = await _context.Movies.OrderByDescending(m => m.Year).Take(3).ToListAsync();
            return PartialView(movies.ToList());
        }
     
        public async Task<IActionResult> _Rate()
        {
            //var Rates = (from p in _context.Movies
            //             orderby p.Rate descending
            //             select p).Take(3).ToListAsync();

            var Rates = _context.Movies.AsQueryable();
            Rates = Rates.OrderByDescending(m => m.Rate);
            Rates = Rates.Take(3);
            return PartialView(Rates);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
