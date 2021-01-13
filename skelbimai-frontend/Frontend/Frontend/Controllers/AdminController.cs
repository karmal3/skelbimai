using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Frontend.API;
using Frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers
{
    public class AdminController : Controller
    {
        //Stores users validation token's string
        public const string SessionTokenName = "_Token";
        //Stores user's username
        public const string SessionUsername = "_Username";
        //Stores user's role
        public const string SessionUserRole = "_UserRole";

        // Displays a page with all users, user's and ad's count and also report system
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString(SessionUserRole) == "1")
                return RedirectToAction("Index", "Home");

            try
            {
                var users = JsonConvert.DeserializeObject<List<Users>>(SkelbimaiAPI.GetAllUsers(HttpContext.Session.GetString(SessionTokenName)));

                var usersCount = SkelbimaiAPI.GetUsersCount(HttpContext.Session.GetString(SessionTokenName));

                var adsCount = SkelbimaiAPI.GetAdsCount(HttpContext.Session.GetString(SessionTokenName));

                ViewBag.usersCount = usersCount;
                ViewBag.adsCount = adsCount;

                return View(users);
            }
            catch (Exception e)
            {
                //SkelbimaiAPI.GetAllUsers, SkelbimaiAPI.GetUsersCount or SkelbimaiAPI.GetAdsCount failed

                var msg = e.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // Displays a page with selected user's information for editting, blocking and deleting
        public IActionResult EditUserForm(int id)
        {
            if (HttpContext.Session.GetString(SessionUserRole) == "1")
                return RedirectToAction("Index", "Home");

            if (id == 0)
                id = HttpContext.Session.GetInt32("DASHBOARDusersID").GetValueOrDefault();
            else
                HttpContext.Session.SetInt32("DASHBOARDusersID", id);

            try
            {
                var user = JsonConvert.DeserializeObject<Users>(SkelbimaiAPI.GetUser(HttpContext.Session.GetString(SessionTokenName), id));

                var Countries = JsonConvert.DeserializeObject<List<Country>>(SkelbimaiAPI.GetCountries());

                ViewBag.Countries = Countries;

                return View(user);
            }
            catch (Exception e)
            {
                var msg = e.Message;

                return RedirectToAction("Dashboard");
            }
        }

        public IActionResult BlockUser(int id, string reason)
        {
            if (HttpContext.Session.GetString(SessionUserRole) == "1")
                return RedirectToAction("Index", "Home");

            try
            {
                Blocks block = new Blocks();
                block.Reason = reason;
                var blockedUser = JsonConvert.DeserializeObject<Blocks>(SkelbimaiAPI.BlockUser(
                           HttpContext.Session.GetString(SessionTokenName),
                           id,
                           block
                       ));

                TempData["Message"] = blockedUser.Message;

                return RedirectToAction("EditUserForm");
            }
            catch (Exception e)
            {
                var msg = e.Message;

                return RedirectToAction("EdituserForm");
            }
        }

        public IActionResult RemoveUser(int id)
        {
            if (HttpContext.Session.GetString(SessionUserRole) == "1")
                return RedirectToAction("Index", "Home");

            try
            {
                var removedUser = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.RemoveUser(
                           HttpContext.Session.GetString(SessionTokenName),
                           id
                       ));

                TempData["Message"] = removedUser.Message;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\users\\" + id);

                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                if (Directory.Exists(path))
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(path);
                }

                return RedirectToAction("EditUserForm");
            }
            catch (Exception e)
            {
                var msg = e.Message;

                return RedirectToAction("EdituserForm");
            }
        }

        public JsonResult Graph(Graph info)
        {
            var results = JsonConvert.DeserializeObject<Graph>(SkelbimaiAPI.Graph(HttpContext.Session.GetString(SessionTokenName), info));

            if (HttpContext.Session.GetString(SessionUserRole) != null)
            {
                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);
                TempData["Username"] = HttpContext.Session.GetString(SessionUsername);
            }
            else
            {
                TempData["Role"] = 0;
                TempData["Username"] = null;
            }

            return Json(results);
        }

        // Method for changing user's role
        public IActionResult Role(Users user)
        {
            if (HttpContext.Session.GetString(SessionUserRole) == "1")
                return RedirectToAction("Index", "Home");

            try
            {
                var results = JsonConvert.DeserializeObject<Users>(SkelbimaiAPI.Role(HttpContext.Session.GetString(SessionTokenName), user));

                TempData["Message"] = results.Message;

                return RedirectToAction("EditUserForm");
            }
            catch (Exception e)
            {
                var msg = e.Message;

                return RedirectToAction("EdituserForm");
            }
        }
    }
}