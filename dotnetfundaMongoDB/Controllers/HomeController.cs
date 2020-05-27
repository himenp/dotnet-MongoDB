using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Collections.Generic;
using dotnetfundaMongoDB.Models;
using MongoDB.Bson;

namespace dotnetfundaMongoDB.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var blogContext = MongoDBContext.Instance;
            var recentPosts = await blogContext.Posts.Find(x => true)
                .SortByDescending(x => x.CreatedAtUtc)
                .Limit(10)
                .ToListAsync();
            if (TempData["Message"] !=null)
            { 
                ViewBag.Message = TempData["Message"];
            }
            return View(recentPosts);
        }
        [HttpGet]
        public ActionResult NewPost()
        {
            ViewBag.PageTitle = "Post | New Post";
            return View("Post", new PostModel());
        }
        [HttpPost]
        public async Task<ActionResult> NewPost(PostModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PageTitle = "Post | New Post";
                return View(model);
            }

            var blogContext = MongoDBContext.Instance;
            var post = new Post
            {
                Author = "Himen Patel",//You can change this to User.Identity.Name after authentication
                Title = model.Title,
                Content = model.Content,
                Tags = model.Tags.Split(' ', ',', ';'),
                CreatedAtUtc = DateTime.UtcNow
            };
            await blogContext.Posts.InsertOneAsync(post);
            if (!String.IsNullOrEmpty(post.Id))
            {
                TempData["Message"] = "Post Successfully Created.";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var blogContext = MongoDBContext.Instance;

            var post = await blogContext.Posts.Find(x => x.Id == id).SingleOrDefaultAsync();

            if (post == null)
            {
                return RedirectToAction("Index");
            }
            var model = new PostModel
            {
                Title = post.Title,
                Content = post.Content,
                Tags = Utilities.ConvertStringArrayToString(post.Tags)
            };
            ViewBag.PageTitle = "Post | Edit Post";
            return View("Post", model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(PostModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PageTitle = "Post | Edit Post";
                return RedirectToAction("Post", new { id = id });
            }
            var blogContext = MongoDBContext.Instance;

            var update = Builders<Post>.Update
                .Set("Author", "Himen Patel")
                .Set("Content", model.Content)
                .Set("Title", model.Title)
                .Set("Tags", model.Tags.Split(' ', ',', ';'))
                .CurrentDate("CreatedAtUtc");
            var _result = await blogContext.Posts.UpdateOneAsync(x => x.Id == id, update);
            if (_result.ModifiedCount > 0)
            {
                TempData["Message"] = "Post Successfully Updated.";
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var blogContext = MongoDBContext.Instance;

            var _filter = Builders<Post>.Filter.Eq("_id", ObjectId.Parse(id)); 
            var _result = await blogContext.Posts.DeleteOneAsync(_filter);
            if (_result.DeletedCount > 0)
            {
                TempData["Message"] = "Post Successfully Deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}