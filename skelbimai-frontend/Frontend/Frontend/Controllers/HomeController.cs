using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Newtonsoft.Json;
using Frontend.API;
using Microsoft.AspNetCore.Http;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        //Stores users validation token's string
        public const string SessionTokenName = "_Token";
        //Stores user's username
        public const string SessionUsername = "_Username";
        //Stores user's role
        public const string SessionUserRole = "_UserRole";

        public JsonResult Index2(Filter filter)
        {
            var model = JsonConvert.DeserializeObject<List<Skelbimas>>(SkelbimaiAPI.GetSkelbimai(filter));

            if (HttpContext.Session.GetString(SessionUserRole) != null)
            {
                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);
                TempData["Username"] = HttpContext.Session.GetString(SessionUsername);
            }
            else
            {
                TempData["Role"] = null;
                TempData["Username"] = null;
            }

            return Json(model);
        }

        // Home page of website
        public IActionResult Index(Filter filter)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<List<Skelbimas>>(SkelbimaiAPI.GetSkelbimai(filter));

                var categories = JsonConvert.DeserializeObject<List<Category>>(SkelbimaiAPI.GetCategories());

                ViewBag.categories = categories;

                if (HttpContext.Session.GetString(SessionUserRole) != null)
                {
                    TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);
                    TempData["Username"] = HttpContext.Session.GetString(SessionUsername);
                }
                else
                {
                    TempData["Role"] = null;
                    TempData["Username"] = null;
                }

                return View(model);
            }
            catch (Exception e)
            {
                var msg = e.Message;

                return View();
            }
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
