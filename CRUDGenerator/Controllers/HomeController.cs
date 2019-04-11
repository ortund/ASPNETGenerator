using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDGenerator.Models;

namespace CRUDGenerator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var files = new List<ResultPage>();

            var model = typeof(License);
            var defaultFiles = Generator.GetDefaultFiles(model);
            var createFiles = Generator.GetCreateFiles(model);
            //var editFiles = Generator.GetEditFiles();

            foreach (var file in defaultFiles) files.Add(file);
            foreach (var file in createFiles) files.Add(file);
            //foreach (var file in editFiles) files.Add(file);

            return View(files);
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
