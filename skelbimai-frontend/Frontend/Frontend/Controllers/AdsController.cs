using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Frontend.Models;
using Frontend.API;
using Newtonsoft.Json;
using System.IO;

namespace Frontend.Controllers
{
    public class AdsController : Controller
    {
        //Stores users validation token's string
        public const string SessionTokenName = "_Token";
        //Stores user's username
        public const string SessionUsername = "_Username";
        //Stores user's role
        public const string SessionUserRole = "_UserRole";
        //Stores user's ID
        public const string SessionUserID = "_UserID";

        // Displays a page with information about specific ad
        public IActionResult AdDetail(int id)
        {
            if (id == 0)
                id = HttpContext.Session.GetInt32("SkelbimasID").GetValueOrDefault();
            else
                HttpContext.Session.SetInt32("SkelbimasID", id);

            if (HttpContext.Session.GetString(SessionTokenName) != null)
            {
                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);
                TempData["ID"] = HttpContext.Session.GetString(SessionUserID);
            }
            else
            {
                TempData["Role"] = "0";
                TempData["ID"] = "0";
            }

            try
            {
                var adInfo = JsonConvert.DeserializeObject<Skelbimas>(SkelbimaiAPI.GetAdInfo(
                        id
                    ));

                var comments = JsonConvert.DeserializeObject<List<Comment>>(SkelbimaiAPI.GetAdComments(
                        HttpContext.Session.GetString(SessionTokenName),
                        id
                    ));

                ViewBag.Ad = adInfo;

                var path = "wwwroot\\ads\\" + id;

                var files = Directory.GetFiles(path);
                string[] finalPictures = new string[files.Count()];

                for (int i = 0; i < files.Count(); i++)
                {
                    var finalPath = files[i].Substring(12, files[i].Length - 12);
                    finalPictures[i] = finalPath;
                }

                ViewBag.AdPictures = finalPictures;

                return View(comments);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                // SkelbimaiAPI.GetAd or SkelbimaiAPI.GetAdComments failed

                return RedirectToAction("Index", "Home");
            }
        }

        // Displays a page for adding new ad
        public IActionResult PostAd()
        {
            if (HttpContext.Session.GetString(SessionTokenName) != null)
            {
                // user is logged in
                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                try
                {
                    // when DB backend is on
                    var model = JsonConvert.DeserializeObject<List<Category>>(SkelbimaiAPI.GetCategories());

                    return View(model);
                }
                catch
                {
                    // when working without DB backend
                    return View();
                }
            }
            else
            {
                // user is not logged in

                return RedirectToAction("Login", "Users");
            }
        }

        // Method for posting a comment
        public IActionResult PostComment(Comment comment)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");
            try
            {
                comment.Username = HttpContext.Session.GetString(SessionUsername);

                var isCommentAdded = SkelbimaiAPI.CreateComment(
                        HttpContext.Session.GetString(SessionTokenName),
                        comment
                    );

                if (isCommentAdded)
                {
                    // comment was added

                    return RedirectToAction("AdDetail");
                }
                else
                {
                    // comment was't added
                    // TODO: ADD ERROR MESSAGE

                    return RedirectToAction("AdDetail");
                }
            }
            catch
            {
                // SkelbimaiAPI.CreateComment failed

                return RedirectToAction("AdDetail");
            }
        }

        // Method for deleting a comment
        public IActionResult DeleteComment(int id, int fkskelbimasid)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");
            try
            {
                var isCommentDeleted = SkelbimaiAPI.DeleteAdComment(
                        HttpContext.Session.GetString(SessionTokenName),
                        id
                    );

                if (isCommentDeleted)
                {
                    // comment was deleted

                    return RedirectToAction("AdDetail");
                }
                else
                {
                    // comment wasn't deleted

                    return RedirectToAction("AdDetail");
                }
            }
            catch
            {
                // SkelbimaiAPI.DeleteAdComment failed

                return RedirectToAction("AdDetail");
            }
        }

        // Method for updating a comment
        public IActionResult UpdateComment(Comment comment)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");

            try
            {
                var isCommentUpdatede = SkelbimaiAPI.UpdateAdComment(
                        HttpContext.Session.GetString(SessionTokenName),
                        comment
                    );

                if (isCommentUpdatede)
                {
                    // comment was updated

                    return RedirectToAction("AdDetail");
                }
                else
                {
                    // comment wasn't updated

                    return RedirectToAction("AdDetail");
                }
            }
            catch (Exception e)
            {
                // SkelbimaiAPI.UpdateAdComment failed

                var msg = e.Message;

                return RedirectToAction("AdDetail");
            }
        }

        // Method for upvoting a comment
        public JsonResult UpvoteComment(int id)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return Json(new { status = "Login" });

            try
            {
                var message = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpvoteComment(
                           HttpContext.Session.GetString(SessionTokenName),
                           id
                       ));

                if (message.Message == "Rating was upvoted.")
                    return Json(new { status = "invert" });
                else if (message.Message == "Rating was created and upvoted.")
                    return Json(new { status = "add" });
                else if (message.Message == "Rating was removed.")
                    return Json(new { status = "remove" });
                else
                    return Json(new { status = "Failed" });

            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }

        // Method for downvoting a comment
        public JsonResult DownvoteComment(int id)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return Json(new { status = "Login" });
            try
            {
                var message = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.DownvoteComment(
                           HttpContext.Session.GetString(SessionTokenName),
                           id
                       ));

                if (message.Message == "Rating was downvoted.")
                    return Json(new { status = "invert" });
                else if (message.Message == "Rating was created and downvoted.")
                    return Json(new { status = "add" });
                else if (message.Message == "Rating was removed.")
                    return Json(new { status = "remove" });
                else
                    return Json(new { status = "Failed" });

            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }

        // Displays a page for editing ad
        public IActionResult EditAdForm(Skelbimas skelbimas)
        {
            try
            {
                var categoriesInfo = JsonConvert.DeserializeObject<List<Category>>(SkelbimaiAPI.GetCategories());

                var adInfo = JsonConvert.DeserializeObject<Skelbimas>(SkelbimaiAPI.GetAd(
                        HttpContext.Session.GetString(SessionTokenName),
                        skelbimas.Id
                    ));

                ViewBag.Categories = categoriesInfo;

                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                return View(adInfo);
            }
            catch
            {
                // SkelbimaiAPI.GetAd returned null

                return RedirectToAction("MyAds", "Users");
            }
        }

        // Method for updating an ad
        public IActionResult UpdateAd(Skelbimas skelbimas, IFormFile indexPicture, List<IFormFile> images)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ads\\" + skelbimas.Id);

            foreach (var picture in images)
            {
                if (picture != null && picture.Length > 0)
                {
                    var fileTypeCount = 0;
                    if (picture.ContentType == "image/jpeg")
                        fileTypeCount = picture.ContentType.Substring(picture.ContentType.IndexOf("/"), picture.ContentType.Length - picture.ContentType.IndexOf("/")).Count() - 1;
                    else
                        fileTypeCount = picture.ContentType.Substring(picture.ContentType.IndexOf("/"), picture.ContentType.Length - picture.ContentType.IndexOf("/")).Count();
                    var fileType = picture.FileName.Substring(picture.FileName.Length - fileTypeCount, fileTypeCount);
                    var finalpath = Path.Combine(path, picture.FileName);
                    //string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\items", HttpContext.Session.GetString(SessionUsername) + fileType);
                    try
                    {
                        if (picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png")
                        {
                            if (System.IO.File.Exists(finalpath))
                                System.IO.File.Delete(finalpath);

                            FileStream haaa = new FileStream(finalpath, FileMode.Create);

                            picture.CopyTo(haaa);
                            haaa.Close();
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            try
            {
                if (indexPicture != null && indexPicture.Length > 0)
                {
                    if (indexPicture.ContentType == "image/jpg" || indexPicture.ContentType == "image/jpeg" || indexPicture.ContentType == "image/png")
                    {
                        var fileTypeCount = 0;
                        if (indexPicture.ContentType == "image/jpeg")
                            fileTypeCount = indexPicture.ContentType.Substring(indexPicture.ContentType.IndexOf("/"), indexPicture.ContentType.Length - indexPicture.ContentType.IndexOf("/")).Count() - 1;
                        else
                            fileTypeCount = indexPicture.ContentType.Substring(indexPicture.ContentType.IndexOf("/"), indexPicture.ContentType.Length - indexPicture.ContentType.IndexOf("/")).Count();
                        var fileType = indexPicture.FileName.Substring(indexPicture.FileName.Length - fileTypeCount, fileTypeCount);
                        var finalpath = Path.Combine(path + "\\index", "indexPicture" + fileType);

                        FileStream haaa = new FileStream(finalpath, FileMode.Create);

                        indexPicture.CopyTo(haaa);
                        haaa.Close();

                        skelbimas.Picture = "/ads/" + skelbimas.Id + "/index/indexPicture" + fileType;
                    }
                }

                var skelbimasUpdate = SkelbimaiAPI.UpdateAd(
                                HttpContext.Session.GetString(SessionTokenName),
                                skelbimas
                            );

                if (skelbimasUpdate)
                {
                    // ad was successfully updated
                    // TODO: ADD MESSAGE OF SUCCESS
                    // TODO: Return to addetails

                    return RedirectToAction("MyAds", "Users");
                }
                else
                {
                    // ad wasn't updated
                    // TODO: ADD MESSAGE OF FAILURE
                    // TODO: Return to addetails

                    return RedirectToAction("MyAds", "Users");
                }
            }
            catch
            {
                // SkelbimaiAPI.UpdateAd failed

                return RedirectToAction("MyAds", "Users");
            }
        }

        // Method for deleting an ad
        public IActionResult DeleteAd(Skelbimas skelbimas)
        {
            try
            {
                var isAdDeleted = SkelbimaiAPI.DeleteAd(HttpContext.Session.GetString(SessionTokenName), skelbimas);

                if (isAdDeleted)
                {
                    // ad was deleted
                    // TODO: ADD MESSAGE OF SUCCESS

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ads\\" + skelbimas.Id);

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

                    return RedirectToAction("MyAds", "Users");
                }
                else
                {
                    // ad wasn't deleted
                    // TODO: ADD MESSAGE OF FAILURE

                    return RedirectToAction("MyAds", "Users");
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
                // isAdDeleted failed

                return RedirectToAction("MyAds", "Users");
            }
        }

        // Method for creating new ad
        public IActionResult CreateAd(Skelbimas skelbimas, List<IFormFile> images, IFormFile indexPicture)
        {
            var newAdID = SkelbimaiAPI.CreateAd(HttpContext.Session.GetString(SessionTokenName), skelbimas);
            //image/png
            //image/jpeg
            //image/jpg
            //image/gif
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ads\\" + newAdID);

            if (!System.IO.Directory.Exists(path + "\\index"))
                Directory.CreateDirectory(path + "\\index");

            if (indexPicture != null && indexPicture.Length > 0)
            {
                if (indexPicture.ContentType == "image/jpg" || indexPicture.ContentType == "image/jpeg" || indexPicture.ContentType == "image/png")
                {
                    var fileTypeCount = 0;
                    if (indexPicture.ContentType == "image/jpeg")
                        fileTypeCount = indexPicture.ContentType.Substring(indexPicture.ContentType.IndexOf("/"), indexPicture.ContentType.Length - indexPicture.ContentType.IndexOf("/")).Count() - 1;
                    else
                        fileTypeCount = indexPicture.ContentType.Substring(indexPicture.ContentType.IndexOf("/"), indexPicture.ContentType.Length - indexPicture.ContentType.IndexOf("/")).Count();
                    var fileType = indexPicture.FileName.Substring(indexPicture.FileName.Length - fileTypeCount, fileTypeCount);
                    var finalpath = Path.Combine(path + "\\index", "indexPicture" + fileType);

                    FileStream haaa = new FileStream(finalpath, FileMode.Create);

                    indexPicture.CopyTo(haaa);
                    haaa.Close();

                    skelbimas.Picture = "/ads/" + newAdID + "/index/indexPicture" + fileType;
                    skelbimas.Id = newAdID;

                    var skelbimasUpdate = SkelbimaiAPI.UpdateAd(
                            HttpContext.Session.GetString(SessionTokenName),
                            skelbimas
                        );
                }
                
            }

            foreach (var picture in images)
            {
                if (picture != null && picture.Length > 0)
                {
                    var fileTypeCount = 0;
                    if (picture.ContentType == "image/jpeg")
                        fileTypeCount = picture.ContentType.Substring(picture.ContentType.IndexOf("/"), picture.ContentType.Length - picture.ContentType.IndexOf("/")).Count() - 1;
                    else
                        fileTypeCount = picture.ContentType.Substring(picture.ContentType.IndexOf("/"), picture.ContentType.Length - picture.ContentType.IndexOf("/")).Count();
                    var fileType = picture.FileName.Substring(picture.FileName.Length - fileTypeCount, fileTypeCount);
                    var finalpath = Path.Combine(path, picture.FileName);
                    //string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\items", HttpContext.Session.GetString(SessionUsername) + fileType);
                    try
                    {
                        if (picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png")
                        {
                            string picturePathPNG = Path.Combine(path, ".png");
                            string picturePathJPEG = Path.Combine(path, ".jpeg");
                            string picturePathJPG = Path.Combine(path, ".jpg");

                            if (System.IO.File.Exists(picturePathPNG))
                                System.IO.File.Delete(picturePathPNG);
                            else if (System.IO.File.Exists(picturePathJPEG))
                                System.IO.File.Delete(picturePathJPEG);
                            else if (System.IO.File.Exists(picturePathJPG))
                                System.IO.File.Delete(picturePathJPG);

                            FileStream haaa = new FileStream(finalpath, FileMode.Create);

                            picture.CopyTo(haaa);
                            haaa.Close();
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // TODO: CHANGE REDIRECTION TO EITHER MYADS OR CHANGE HOME FOLDER NAME!
            return RedirectToAction("Index", "Home");
        }

        public JsonResult AdViewCounter(int id)
        {
            try
            {
                var isViewCounterIncreased = SkelbimaiAPI.AdsViewCounter(id);

                if (isViewCounterIncreased)
                    return Json(new { message = "Added" });
                else
                    return Json(new { message = "Failed" });

            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }

        public JsonResult RateAd(Skelbimasrating newRating)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return Json(new { status = "Login" });

            try
            {
                var adRating = JsonConvert.DeserializeObject<Skelbimasrating>(SkelbimaiAPI.RateAd(HttpContext.Session.GetString(SessionTokenName), newRating));

                return Json(new { status = adRating.Message });
            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }
    }
}