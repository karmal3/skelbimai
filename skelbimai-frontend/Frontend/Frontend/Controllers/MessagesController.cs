using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frontend.API;
using Frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers
{
    public class MessagesController : Controller
    {
        //Stores users validation token's string
        public const string SessionTokenName = "_Token";
        //Stores user's username
        public const string SessionUsername = "_Username";
        //Stores user's role
        public const string SessionUserRole = "_UserRole";

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionTokenName) != null)
            {
                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                //return View();
            }
            else
            {
                TempData["Role"] = null;

                return RedirectToAction("Index", "Home");
            }

            try
            {
                
                var inbox = JsonConvert.DeserializeObject<List<Messages>>(SkelbimaiAPI.GetUserInbox(HttpContext.Session.GetString(SessionTokenName)));
                var sent = JsonConvert.DeserializeObject<List<Messages>>(SkelbimaiAPI.GetUserSent(HttpContext.Session.GetString(SessionTokenName)));

                var inboxCount = SkelbimaiAPI.GetInboxCount(HttpContext.Session.GetString(SessionTokenName));
                var sentCount = SkelbimaiAPI.GetSentCount(HttpContext.Session.GetString(SessionTokenName));

                ViewBag.inbox = inbox;
                ViewBag.sent = sent;

                ViewBag.inboxCount = inboxCount;
                ViewBag.sentCount = sentCount;

                return View();
            }
            catch
            {
                // SkelbimaiAPI.GetUserInbox or SkelbimaiAPI.GetUserSent returned null

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult SendMessage(Messages msg)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Messages>(SkelbimaiAPI.SendMessage(HttpContext.Session.GetString(SessionTokenName), msg));

                TempData["Message"] = result.Message;

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var err = e.Message;

                TempData["Message"] = err;

                return RedirectToAction("Index");
            }
            
        }

        public IActionResult DeleteInboxMessage(int id)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Messages>(SkelbimaiAPI.DeleteInboxMessage(HttpContext.Session.GetString(SessionTokenName), id));

                TempData["Message"] = result.Message;

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var err = e.Message;

                TempData["Message"] = err;

                return RedirectToAction("Index");
            }

        }

        public IActionResult DeleteSentMessage(int id)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Messages>(SkelbimaiAPI.DeleteSentMessage(HttpContext.Session.GetString(SessionTokenName), id));

                TempData["Message"] = result.Message;

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var err = e.Message;

                TempData["Message"] = err;

                return RedirectToAction("Index");
            }

        }
    }
}