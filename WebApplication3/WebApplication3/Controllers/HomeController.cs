using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
 
        private readonly ILogger<HomeController> _logger;

      
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
         {
            //kết nối DB
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            //Lưu trữ Document/Data
            {
                documentstore.Initialize();

                using (var session = documentstore.OpenSession())
                {
                   
                    //fetch all the record
                        var allcompanies = session.Query<Company>().ToArray();
                }
            }
            // lấy dữ liệu trong records
            
            return View();
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
