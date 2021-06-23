using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FODLSystem.Models;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using FODLSystem.Models.View_Model;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FODLSystem.Controllers
{
    [Authorize]
    [BreadCrumb(Title = "Home", UseDefaultRouteUrl = true, Order = 0, IgnoreAjaxRequests = true)]
    public class HomeController : Controller
    {

        //private readonly ILogger<HomeController> _logger;
        private readonly FODLSystemContext _context;

        public HomeController(FODLSystemContext context
            //, ILogger<HomeController> logger
            )
        {
            //_logger = logger;
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
            string companyAccess = User.Identity.GetCompanyAccess();
            int[] compId = companyAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();


           
            return View();
        }



    }

}
