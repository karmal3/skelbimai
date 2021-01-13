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
    public class UsersController : Controller
    {
        //Stores user's validation token's string
        public const string SessionTokenName = "_Token";
        //Stores user's username
        public const string SessionUsername = "_Username";
        //Stores user's role
        public const string SessionUserRole = "_UserRole";
        //Stores user's ID
        public const string SessionUserID = "_UserID";

        // Displays register screen
        public IActionResult Register()
        {
            try
            {
                var model = JsonConvert.DeserializeObject<List<Country>>(SkelbimaiAPI.GetCountries());

                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // Method for login user out of the website
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

        // Displays login screen
        public IActionResult Login()
        {
            return View();
        }

        // Displays user's ads
        public IActionResult MyAds(int id)
        {
            if (HttpContext.Session.GetString(SessionTokenName) != null)
            {
                // user is logged in

                try
                {
                    if (HttpContext.Session.GetString(SessionUserRole) == "2" && id != 0)
                    {
                        var userAds = JsonConvert.DeserializeObject<List<Skelbimas>>(SkelbimaiAPI.GetUserAdsAdmin(HttpContext.Session.GetString(SessionTokenName), id));

                        TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                        return View(userAds);
                    }
                    else
                    {
                        var userAds = JsonConvert.DeserializeObject<List<Skelbimas>>(SkelbimaiAPI.GetUsersAds(HttpContext.Session.GetString(SessionTokenName)));

                        TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                        return View(userAds);
                    }
                }
                catch
                {
                    // SkelbimaiAPI.GetUsersAds returned null

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // user is not logged in

                return RedirectToAction("Login");
            }
        }

        // Displays a page with user's existing topics
        public IActionResult MyTopics(int id)
        {
            if (HttpContext.Session.GetString(SessionTokenName) != null)
            {
                // user is logged in

                try
                {
                    if (HttpContext.Session.GetString(SessionUserRole) == "2" && id != 0)
                    {
                        var userTopics = JsonConvert.DeserializeObject<List<Topic>>(SkelbimaiAPI.GetUserTopicsAdmin(HttpContext.Session.GetString(SessionTokenName), id));

                        TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                        return View(userTopics);
                    }
                    else
                    {
                        var userTopics = JsonConvert.DeserializeObject<List<Topic>>(SkelbimaiAPI.GetUserTopics(HttpContext.Session.GetString(SessionTokenName)));

                        TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                        return View(userTopics);
                    }
                }
                catch
                {
                    // SkelbimaiAPI.GetUsersAds returned null

                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                // user is not logged in

                return RedirectToAction("Login");
            }
        }

        // Method for updating user's profile picture
        public IActionResult Picture(IFormFile picture, Users useris)
        {
            //image/png
            //image/jpeg
            //image/jpg
            //image/gif

            if (useris.Id != 0)
            {
                // Admin is changing user's profile picture

                if (picture != null && picture.Length > 0)
                {
                    Users user = new Users();

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\users\\" + useris.Id);

                    if (!System.IO.Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\users\\" + useris.Id, picture.FileName);
                    try
                    {
                        if (picture.ContentType == "image/gif" || picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png")
                        {
                            System.IO.DirectoryInfo di = new DirectoryInfo(path);

                            if (Directory.Exists(path))
                            {
                                foreach (FileInfo file in di.GetFiles())
                                {
                                    file.Delete();
                                }
                            }

                            FileStream haaa = new FileStream(fullPath, FileMode.Create);

                            picture.CopyTo(haaa);
                            haaa.Close();
                            user.ProfilePicture = "/users/" + useris.Id + "/" + picture.FileName;
                            user.Id = useris.Id;
                        }
                        else
                            return RedirectToAction("EditUserForm", "Admin");

                        var test = SkelbimaiAPI.UploadPicture(HttpContext.Session.GetString(SessionTokenName), user);
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;

                        return RedirectToAction("EditUserForm", "Admin");
                    }
                }
                return RedirectToAction("EditUserForm", "Admin");
            }
            else
            {
                // User is changing profile picture

                if (picture != null && picture.Length > 0)
                {
                    Users user = new Users();

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\users\\" + HttpContext.Session.GetString(SessionUserID));

                    if (!System.IO.Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\users\\" + HttpContext.Session.GetString(SessionUserID), picture.FileName);
                    try
                    {
                        if (picture.ContentType == "image/gif" || picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png")
                        {
                            System.IO.DirectoryInfo di = new DirectoryInfo(path);

                            if (Directory.Exists(path))
                            {
                                foreach (FileInfo file in di.GetFiles())
                                {
                                    file.Delete();
                                }
                            }

                            FileStream haaa = new FileStream(fullPath, FileMode.Create);

                            picture.CopyTo(haaa);
                            haaa.Close();
                            user.ProfilePicture = "/users/" + HttpContext.Session.GetString(SessionUserID) + "/" + picture.FileName;
                        }
                        else
                            return RedirectToAction("Profile");

                        var test = SkelbimaiAPI.UploadPicture(HttpContext.Session.GetString(SessionTokenName), user);
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;

                        return RedirectToAction("Profile");
                    }
                }
                return RedirectToAction("Profile");
            }
        }

        // Displays user's profile information
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString(SessionTokenName) != null)
            {
                // user is logged in

                try
                {
                    var Countries = JsonConvert.DeserializeObject<List<Country>>(SkelbimaiAPI.GetCountries());

                    var userInfo = JsonConvert.DeserializeObject<Users>(SkelbimaiAPI.GetUser(HttpContext.Session.GetString(SessionTokenName)));

                    // for different layers depending on user's role
                    TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                    ViewBag.Countries = Countries;

                    return View(userInfo);
                }
                catch (Exception e)
                {
                    // method SkelbimaiAPI.GetUser returned null

                    var msg = e.Message;

                    return View();
                    //return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // user is not logged in

                return RedirectToAction("Login");
            }
        }

        // Method for registrating new users
        public IActionResult RegisterUser(Users user, string Country)
        {
            user.FkCountryId = int.Parse(Country);
            user.Role = 1;  // normal user's rights
            var info = SkelbimaiAPI.Register(user);
            var register = info.Result.Contains("message");

            if (!register)
            {
                return RedirectToAction("Login", false);
            }
            else
            {
                TempData["Message"] = new string(info.Result).Substring(info.Result.IndexOf(":") + 2, info.Result.Length - info.Result.IndexOf(":") - 4);
                return RedirectToAction("Register", false);
            }
        }

        // Method for login in the users
        public IActionResult LoginUser(Users user)
        {
            try
            {
                var userInfo = JsonConvert.DeserializeObject<Users>(SkelbimaiAPI.Login(user).Result);

                if (userInfo.Token != null)
                {
                    // User logged in successfully

                    HttpContext.Session.SetString(SessionTokenName, userInfo.Token);
                    HttpContext.Session.SetString(SessionUsername, userInfo.Username);
                    HttpContext.Session.SetString(SessionUserRole, userInfo.Role.ToString());
                    HttpContext.Session.SetString(SessionUserID, userInfo.Id.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // User didn't logged in

                    TempData["Message"] = userInfo.Message;
                    return View("Login");
                }
            }
            catch
            {
                // userInfo is null

                return View("Login");
            }

        }

        // Method for reseting user's password
        public IActionResult ResetPassword(Users user)
        {
            var info = SkelbimaiAPI.ResetPassword(user);

            return View("Login");
        }

        // Method for updating user's optional information
        public IActionResult UpdateUser(Users user)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateUserInfo(
                            HttpContext.Session.GetString(SessionTokenName),
                            user));

                TempData["Message"] = result.Message;

                if (user.Id == 0)
                    return RedirectToAction("Profile");
                else
                    return RedirectToAction("EditUserForm", "Admin");
            }
            catch (Exception e)
            {
                // SkelbimaiAPI.UpdaeUserInfo returned null

                TempData["Message"] = e.Message;

                if (user.Id == 0)
                    return RedirectToAction("Profile");
                else
                    return RedirectToAction("EditUserForm", "Admin");
            }
        }

        // Method for updating user's password
        public IActionResult UpdatePassword(Users user, string newpassword)
        {
            try
            {
                var userInfo = JsonConvert.DeserializeObject<Users>(SkelbimaiAPI.Login(user).Result);

                // checks if user's current password is correct
                if (userInfo.Token == null)
                {
                    // user's current password is incorrect

                    TempData["Message"] = "Old password is incorrect!";

                    return RedirectToAction("Profile");
                }

                user.Password = newpassword;

                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateUserInfo(
                            HttpContext.Session.GetString(SessionTokenName),
                            user));

                TempData["Message"] = result.Message;

                return RedirectToAction("Profile");
            }
            catch (Exception e)
            {
                // SkelbimaiAPI.Login or SkelbimaiAPI.UpdaeUserInfo returned null

                TempData["Message"] = e.Message;

                return RedirectToAction("Profile");
            }
        }

        // Method for updating user's email address
        public IActionResult UpdateEmail(Users user)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateUserInfo(
                            HttpContext.Session.GetString(SessionTokenName),
                            user));

                TempData["Message"] = result.Message;

                return RedirectToAction("Profile");
            }
            catch (Exception e)
            {
                // SkelbimaiAPI.UpdaeUserInfo returned null

                TempData["Message"] = e.Message;

                return RedirectToAction("Profile");
            }
        }

        // Method for updating user's username
        public IActionResult UpdateUsername(Users user)
        {
            if (user.Id == 0)
            {
                // user is updating his username

                try
                {
                    var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateUserInfo(
                            HttpContext.Session.GetString(SessionTokenName),
                            user));

                    TempData["Message"] = result.Message;

                    return RedirectToAction("Profile");
                }
                catch (Exception e)
                {
                    // SkelbimaiAPI.UpdaeUserInfo returned null

                    var msg = e.Message;

                    TempData["Message"] = msg;

                    return RedirectToAction("Profile");
                }
            }
            else
            {
                // admin is updating user's username

                try
                {
                    var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateUserInfo(
                            HttpContext.Session.GetString(SessionTokenName),
                            user));

                    TempData["Message"] = result.Message;

                    return RedirectToAction("EditUserForm", "Admin");
                }
                catch (Exception e)
                {
                    // SkelbimaiAPI.UpdaeUserInfo returned null

                    TempData["Message"] = e.Message;

                    return RedirectToAction("EditUserForm", "Admin");
                }
            }
        }
    }
}