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
    public class ForumController : Controller
    {
        //Stores users validation token's string
        public const string SessionTokenName = "_Token";
        //Stores user's username
        public const string SessionUsername = "_Username";
        //Stores user's role
        public const string SessionUserRole = "_UserRole";
        //Stores user's ID
        public const string SessionUserID = "_UserID";

        // Forum's home page
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                TempData["Role"] = 0;
            else
                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

            try
            {
                var forumCategories = JsonConvert.DeserializeObject<List<ForumCategory>>(SkelbimaiAPI.GetForumCategories());

                ViewBag.Total = SkelbimaiAPI.GetTotalTopics();

                return View(forumCategories);
            }
            catch
            {
                // SkelbimaiAPI.GetForumCategories failed

                return View();
            }
        }

        // Method for creating new forum category
        public IActionResult CreateForumCategory(ForumCategory forumCategory)
        {
            if (HttpContext.Session.GetString(SessionUserRole) != "2")
            {
                // unauthorized user

                return RedirectToAction("Index");
            }

            try
            {
                var isForumCategoryCreated = SkelbimaiAPI.CreateForumCategory(
                        HttpContext.Session.GetString(SessionTokenName),
                        forumCategory
                    );

                if (isForumCategoryCreated)
                {
                    // forum category created

                    return RedirectToAction("Index");
                }
                else
                {
                    // forum category wasn't created

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                // SkelbimaiAPI.CreateForumCategory failed

                return RedirectToAction("Index");
            }
        }

        // Method for deleting an existing forum category
        public IActionResult DeleteForumCategory(int id)
        {
            if (HttpContext.Session.GetString(SessionUserRole) != "2")
                RedirectToAction("Index", "Home");

            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.DeleteForumCategory(HttpContext.Session.GetString(SessionTokenName), id));

                TempData["Message"] = result.Message;

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                //SkelbimaiAPI.DeleteForumCategory

                var msg = e.Message;

                TempData["Message"] = msg;

                return RedirectToAction("Index");
            }
        }

        // Method for updating forum category
        public IActionResult UpdateForumCategory(ForumCategory category)
        {
            if (HttpContext.Session.GetString(SessionUserRole) != "2")
                RedirectToAction("Index", "Home");

            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateForumCategory(HttpContext.Session.GetString(SessionTokenName), category));

                TempData["Message"] = result.Message;

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                // SkelbimaiAPI.UpdateForumCategory

                var msg = e.Message;

                return RedirectToAction("Index");
            }
        }

        // Displays a page of forum category's discussions
        public IActionResult Discussion(int id)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                TempData["Role"] = 0;
            else
                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

            if (id == 0)
                id = HttpContext.Session.GetInt32("ForumCategoryID").GetValueOrDefault();
            else
                HttpContext.Session.SetInt32("ForumCategoryID", id);

            try
            {
                var topics = JsonConvert.DeserializeObject<List<Topic>>(SkelbimaiAPI.GetTopics(id));

                ViewBag.Total = SkelbimaiAPI.GetTotalTopicsNumber(id);

                return View(topics);
            }
            catch
            {
                // SkelbimaiAPI.GetTopics failed

                return View();
            }
        }

        // Method for creating new topic
        public IActionResult CreateTopic(Topic topic)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
            {
                // non logged in user

                return RedirectToAction("Login", "Users");
            }

            try
            {
                topic.FkForumcategoryId = HttpContext.Session.GetInt32("ForumCategoryID").GetValueOrDefault();

                var isTopicCreated = SkelbimaiAPI.CreateTopic(
                        HttpContext.Session.GetString(SessionTokenName),
                        topic
                    );

                if (isTopicCreated)
                {
                    // topic created

                    return RedirectToAction("Discussion");
                }
                else
                {
                    // topic wasn't created

                    return RedirectToAction("Discussion");
                }
            }
            catch
            {
                // SkelbimaiAPI.CreateTopic failed

                return RedirectToAction("Discussion");
            }
        }

        // Method for deleting an existing topic
        public IActionResult DeleteTopic(int id)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");
            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.DeleteTopic(HttpContext.Session.GetString(SessionTokenName), id));

                TempData["Message"] = result.Message;

                return RedirectToAction("MyTopics", "Users");
            }
            catch (Exception e)
            {
                // isAdDeleted failed

                var msg = e.Message;

                TempData["Message"] = msg;

                return RedirectToAction("MyTopics", "Users");
            }
        }

        // Method for admins deleting an existing topic 
        public IActionResult DeleteTopicAdmin(int id)
        {
            if (HttpContext.Session.GetString(SessionUserRole) != "2")
                RedirectToAction("Index", "Home");

            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.DeleteTopic(HttpContext.Session.GetString(SessionTokenName), id));

                TempData["Message"] = result.Message;

                return RedirectToAction("Discussion");
            }
            catch (Exception e)
            {
                // isAdDeleted failed

                var msg = e.Message;

                TempData["Message"] = msg;

                return RedirectToAction("Discussion");
            }
        }

        // Method for updating a topic
        public IActionResult UpdateTopic(Topic topic)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");
            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateTopic(HttpContext.Session.GetString(SessionTokenName), topic));

                TempData["Message"] = result.Message;

                return RedirectToAction("MyTopics", "Users");
            }
            catch (Exception e)
            {
                // SkelbimaiAPI.UpdateAd failed

                var msg = e.Message;

                return RedirectToAction("MyTopics", "Users");
            }
        }

        // Method for admins updating a topic
        public IActionResult UpdateTopicAdmin(Topic topic)
        {
            if (HttpContext.Session.GetString(SessionUserRole) != "2")
                RedirectToAction("Index", "Home");

            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateTopic(HttpContext.Session.GetString(SessionTokenName), topic));

                TempData["Message"] = result.Message;

                return RedirectToAction("Discussion");
            }
            catch (Exception e)
            {
                // SkelbimaiAPI.UpdateAd failed

                var msg = e.Message;

                TempData["Message"] = msg;

                return RedirectToAction("Discussion");
            }
        }

        // Displays a page for editing a topic
        public IActionResult EditTopicForm(Topic topic)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");
            try
            {
                var topicInfo = JsonConvert.DeserializeObject<Topic>(SkelbimaiAPI.GetTopic(
                        topic.Id
                    ));

                TempData["Role"] = HttpContext.Session.GetString(SessionUserRole);

                return View(topicInfo);
            }
            catch
            {
                // SkelbimaiAPI.GetTopic failed

                return RedirectToAction("MyTopics", "Users");
            }
        }

        // Method for increasing forum's category view counter
        public JsonResult CategoryViewCounter(int id)
        {
            try
            {
                var isViewCounterIncreased = SkelbimaiAPI.CategoryViewCounter(id);

                return Json(new { message = "Added" });
            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }

        // Method for increasing topic's view counter
        public JsonResult ViewCounter(int id)
        {
            try
            {
                var isViewCounterIncreased = SkelbimaiAPI.TopicViewCounter(id);

                return Json(new { message = "Added" });
            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }

        // Displays a page with specific topic's information
        public IActionResult TopicInfo(int id)
        {
            if (id == 0)
                id = HttpContext.Session.GetInt32("TopicID").GetValueOrDefault();
            else
                HttpContext.Session.SetInt32("TopicID", id);

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
                var topicInfo = JsonConvert.DeserializeObject<Topic>(SkelbimaiAPI.GetTopic(id));

                var comments = JsonConvert.DeserializeObject<List<ForumComments>>(SkelbimaiAPI.GetTopicComments(
                        HttpContext.Session.GetString(SessionTokenName),
                        id
                    ));

                ViewBag.Topic = topicInfo;

                return View(comments);
            }
            catch
            {
                // SkelbimaiAPI.GetTopic or SkelbimaiAPI.GetTopicComments failed

                return View();
            }
        }

        // Method for posting a comment
        public IActionResult PostComment(ForumComments comment)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");
            try
            {
                var isCommentAdded = SkelbimaiAPI.CreateTopicComment(
                        HttpContext.Session.GetString(SessionTokenName),
                        comment
                    );

                if (isCommentAdded)
                {
                    // comment was added

                    return RedirectToAction("TopicInfo");
                }
                else
                {
                    // comment was't added
                    // TODO: ADD ERROR MESSAGE

                    return RedirectToAction("TopicInfo");
                }
            }
            catch
            {
                // SkelbimaiAPI.CreateTopicComment failed

                return RedirectToAction("TopicInfo");
            }
        }

        // Method for deleting a comment
        public IActionResult DeleteComment(int id, int fkskelbimasid)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");
            try
            {
                var isCommentDeleted = SkelbimaiAPI.DeleteTopicComment(
                        HttpContext.Session.GetString(SessionTokenName),
                        id
                    );

                if (isCommentDeleted)
                {
                    // comment was deleted

                    return RedirectToAction("TopicInfo");
                }
                else
                {
                    // comment wasn't deleted

                    return RedirectToAction("TopicInfo");
                }
            }
            catch
            {
                // SkelbimaiAPI.DeleteAdComment failed

                return RedirectToAction("TopicInfo");
            }
        }

        // Method for updating a comment
        public IActionResult UpdateComment(ForumComments comment)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return RedirectToAction("Login", "Users");

            try
            {
                var result = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpdateTopicComment(
                            HttpContext.Session.GetString(SessionTokenName),
                            comment));

                TempData["Message"] = result.Message;

                return RedirectToAction("TopicInfo");
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message;

                return RedirectToAction("TopicInfo");
            }
        }

        // Method for upvoting a comment
        public JsonResult UpvoteComment(int id)
        {
            if (HttpContext.Session.GetString(SessionTokenName) == null)
                return Json(new { status = "Login" });

            try
            {
                var message = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.UpvoteForumComment(
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
                var message = JsonConvert.DeserializeObject<ErrorMessage>(SkelbimaiAPI.DownvoteForumComment(
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
    }
}