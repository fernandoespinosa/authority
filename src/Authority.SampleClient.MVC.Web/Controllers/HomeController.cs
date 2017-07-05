using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Authority.SampleClient.MVC.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AuthorizedResource()
        {
            return Ok(new
            {
                Message = "This is your authorized resource"
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
