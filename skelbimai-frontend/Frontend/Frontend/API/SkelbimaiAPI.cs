using Frontend.Models;
using Newtonsoft.Json;
using Polly;
using Polly.Bulkhead;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Frontend.API
{
    static public class SkelbimaiAPI
    {
        private static readonly HttpClient client;
        private static readonly AsyncRetryPolicy retry;
        private static readonly AsyncBulkheadPolicy bulkhead;

        static SkelbimaiAPI()
        {
            retry = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });

            bulkhead = Policy
                .BulkheadAsync(20, int.MaxValue);

            client = new HttpClient
            {
                Timeout = new TimeSpan(0, 2, 0)
            };
        }

        public static async Task<string> Login(Users dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            string url = @"http://localhost:56257/api/user/authenticate";
            var response = await Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async ()
                => await client.PostAsync(url, content));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var str = await response.Content.ReadAsStringAsync();
                return str;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var str = await response.Content.ReadAsStringAsync();
                return str;
            }
                
                
            return null;
        }

        public static async Task<string> Register(Users dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            string url = @"http://localhost:56257/api/user/register";
            var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async ()
                => await client.PostAsync(url, content));
            return await response.Result.Content.ReadAsStringAsync();
        }

        public static async Task<bool> ResetPassword(Users dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            string url = @"http://localhost:56257/api/user/passwordreset";
            var response = await Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async ()
                => await client.PostAsync(url, content));
            return response.StatusCode == HttpStatusCode.OK;
        }

        public static string GetUser(string token)
        {
            string url = @"http://localhost:56257/api/user";

            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    string str = response.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                    
                return null;
            }
        }

        public static int CreateAd(string token, Skelbimas dto)
        {
            string url = @"http://localhost:56257/api/skelbimas/create";

            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    var str = JsonConvert.DeserializeObject<Skelbimas>(response.Result.Content.ReadAsStringAsync().Result);
                    return str.Id;
                }
                return -1;
            }
        }

        public static string GetSkelbimai(Filter dto)
        {
            string url = @"http://localhost:56257/api/skelbimas";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetCategories()
        {
            string url = @"http://localhost:56257/api/category";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetCountries()
        {
            string url = @"http://localhost:56257/api/country";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetUsersAds(string token)
        {
            string url = @"http://localhost:56257/api/skelbimas/usersads";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    string str = response.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }

                return null;
            }
        }

        public static bool DeleteAd(string token, Skelbimas dto)
        {
            string url = @"http://localhost:56257/api/skelbimas/delete";
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string GetAd(string token, int id)
        {
            string url = @"http://localhost:56257/api/skelbimas/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    string str = response.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                return null;
            }
        }

        public static string GetAdInfo(int id)
        {
            string url = @"http://localhost:56257/api/skelbimas/info/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    string str = response.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                return null;
            }
        }

        public static bool UpdateAd(string token, Skelbimas dto)
        {
            string url = @"http://localhost:56257/api/skelbimas/edit";
            using (var msg = new HttpRequestMessage(HttpMethod.Put, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string UpdateUserInfo(string token, Users dto)
        {
            string url = @"http://localhost:56257/api/user/update";
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static bool CreateComment(string token, Comment dto)
        {
            string url = @"http://localhost:56257/api/comments/add";

            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string GetAdComments(string token, int id)
        {
            string url = @"http://localhost:56257/api/comments/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    string str = response.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                return null;
            }
        }

        public static bool DeleteAdComment(string token, int id)
        {
            string url = @"http://localhost:56257/api/comments/delete/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static bool CreateForumCategory(string token, ForumCategory dto)
        {
            string url = @"http://localhost:56257/api/forum/create";

            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string GetForumCategories()
        {
            string url = @"http://localhost:56257/api/forum";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static bool CreateTopic(string token, Topic dto)
        {
            string url = @"http://localhost:56257/api/topic/create";

            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string DeleteTopic(string token, int id)
        {
            string url = @"http://localhost:56257/api/topic/delete/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string UpdateTopic(string token, Topic dto)
        {
            string url = @"http://localhost:56257/api/topic/edit";
            using (var msg = new HttpRequestMessage(HttpMethod.Put, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetTopics(int id)
        {
            string url = @"http://localhost:56257/api/topic/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetTopic(int id)
        {
            string url = @"http://localhost:56257/api/topic/get/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetUserTopics(string token)
        {
            string url = @"http://localhost:56257/api/topic";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string UpvoteComment(string token, int id)
        {
            string url = @"http://localhost:56257/api/comments/up/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string DownvoteComment(string token, int id)
        {
            string url = @"http://localhost:56257/api/comments/down/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static bool UploadPicture(string token, Users dto)
        {
            string url = @"http://localhost:56257/api/user/test";
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static bool CreateTopicComment(string token, ForumComments dto)
        {
            string url = @"http://localhost:56257/api/topiccomments/add";

            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string GetTopicComments(string token, int id)
        {
            string url = @"http://localhost:56257/api/topiccomments/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    string str = response.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                return null;
            }
        }

        public static bool DeleteTopicComment(string token, int id)
        {
            string url = @"http://localhost:56257/api/topiccomments/delete/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static bool TopicViewCounter(int id)
        {
            string url = @"http://localhost:56257/api/topic/view/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static bool CategoryViewCounter(int id)
        {
            string url = @"http://localhost:56257/api/forum/view/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        // specific forum category's topics number
        public static int GetTotalTopicsNumber(int id)
        {
            string url = @"http://localhost:56257/api/topic/total/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    int str = int.Parse(response.Result.Content.ReadAsStringAsync().Result);
                    return str;
                }
                return -1;
            }
        }

        // all topics
        public static int GetTotalTopics()
        {
            string url = @"http://localhost:56257/api/forum/total";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    int str = int.Parse(response.Result.Content.ReadAsStringAsync().Result);
                    return str;
                }
                return -1;
            }
        }

        public static string UpvoteForumComment(string token, int id)
        {
            string url = @"http://localhost:56257/api/topiccomments/up/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string DownvoteForumComment(string token, int id)
        {
            string url = @"http://localhost:56257/api/topiccomments/down/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static bool UpdateAdComment(string token, Comment dto)
        {
            string url = @"http://localhost:56257/api/comments/edit";
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string GetAllUsers(string token)
        {
            string url = @"http://localhost:56257/api/user/getusers";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetUser(string token, int id)
        {
            string url = @"http://localhost:56257/api/user/getuser/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static int GetUsersCount(string token)
        {
            string url = @"http://localhost:56257/api/user/userscount";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return int.Parse(response.Result.Content.ReadAsStringAsync().Result);
                return -1;
            }
        }

        public static int GetAdsCount(string token)
        {
            string url = @"http://localhost:56257/api/skelbimas/adcount";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return int.Parse(response.Result.Content.ReadAsStringAsync().Result);
                return -1;
            }
        }

        public static string BlockUser(string token, int id, Blocks block)
        {
            string url = @"http://localhost:56257/api/user/block/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(block), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string RemoveUser(string token, int id)
        {
            string url = @"http://localhost:56257/api/user/delete/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetUserInbox(string token)
        {
            string url = @"http://localhost:56257/api/message/inbox";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetUserSent(string token)
        {
            string url = @"http://localhost:56257/api/message/sent";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string SendMessage(string token, Messages message)
        {
            string url = @"http://localhost:56257/api/message/send";
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string DeleteInboxMessage(string token, int id)
        {
            string url = @"http://localhost:56257/api/message/inboxdelete/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string DeleteSentMessage(string token, int id)
        {
            string url = @"http://localhost:56257/api/message/sentdelete/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static int GetInboxCount(string token)
        {
            string url = @"http://localhost:56257/api/message/inboxcount";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return int.Parse(response.Result.Content.ReadAsStringAsync().Result);
                return -1;
            }
        }

        public static int GetSentCount(string token)
        {
            string url = @"http://localhost:56257/api/message/sentcount";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return int.Parse(response.Result.Content.ReadAsStringAsync().Result);
                return -1;
            }
        }

        public static string GetUserAdsAdmin(string token, int id)
        {
            string url = @"http://localhost:56257/api/skelbimas/usersadsadmin/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string GetUserTopicsAdmin(string token, int id)
        {
            string url = @"http://localhost:56257/api/topic/usertopicsadmin/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static bool AdsViewCounter(int id)
        {
            string url = @"http://localhost:56257/api/skelbimas/view/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg)).Result;
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        public static string RateAd(string token, Skelbimasrating dto)
        {
            string url = @"http://localhost:56257/api/skelbimas/rate";
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string DeleteForumCategory(string token, int id)
        {
            string url = @"http://localhost:56257/api/forum/delete/" + id;
            using (var msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string UpdateForumCategory(string token, ForumCategory dto)
        {
            string url = @"http://localhost:56257/api/forum/edit";
            using (var msg = new HttpRequestMessage(HttpMethod.Put, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string Graph(string token, Graph dto)
        {
            string url = @"http://localhost:56257/api/skelbimas/graph";
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string Role(string token, Users dto)
        {
            string url = @"http://localhost:56257/api/user/role";
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }

        public static string UpdateTopicComment(string token, ForumComments dto)
        {
            string url = @"http://localhost:56257/api/topiccomments/edit";
            using (var msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                msg.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = Policy.WrapAsync(bulkhead, retry).ExecuteAsync(async () => await client.SendAsync(msg));
                if (response.Result.StatusCode == HttpStatusCode.OK)
                    return response.Result.Content.ReadAsStringAsync().Result;
                else if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                    return response.Result.Content.ReadAsStringAsync().Result;
                return null;
            }
        }
    }
}
