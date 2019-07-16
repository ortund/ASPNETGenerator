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
            return View();
        }
        //public IActionResult Index()
        //{
        //    var files = new List<ResultPage>();

        //    var model = typeof(License);
        //    var defaultFiles = Generator.GetDefaultFiles(model);
        //    var createFiles = Generator.GetCreateFiles(model);
        //    var editFiles = Generator.GetEditFiles(model);

        //    foreach (var file in defaultFiles) files.Add(file);
        //    foreach (var file in createFiles) files.Add(file);
        //    foreach (var file in editFiles) files.Add(file);

        //    return View(files);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Default()
        {
            var model = typeof(License);
            var defaultFiles = Generator.GetDefaultFiles(model)
                .Select(x => new ResultPage
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();
            return View(defaultFiles);
        }

        public IActionResult Create()
        {
            var model = typeof(License);
            var createFiles = Generator.GetCreateFiles(model)
                .Select(x => new ResultPage
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();
            return View(createFiles);
        }

        public IActionResult Edit()
        {
            var model = typeof(License);
            var editFiles = Generator.GetEditFiles(model)
                .Select(x => new ResultPage
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();
            return View(editFiles);
        }

        public IActionResult Sorting()
        {
            return View();
        }

        public IActionResult Progress()
        {
            return View();
        }

        public IActionResult GetStarted()
        {
            return View();
        }
    }
}
