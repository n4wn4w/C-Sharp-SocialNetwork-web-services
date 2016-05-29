using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SocialNetwork.Services.Models;
using SocialNetwork.Models;
using Microsoft.AspNet.Identity;

namespace SocialNetweork.WepServeses.Controllers
{
    public class PostsController : BaseApiController
    {
        // GET api/posts
        [HttpGet]
        public IHttpActionResult GetPosts()
        {
            

            var data = this.Data.Posts.OrderBy(p => p.PostedOn).Select(PostViewModel.Create);

            return this.Ok(data);
        }

        // POST api/post{id}
        [Authorize]
        [HttpPost]
        public IHttpActionResult AddPost([FromBody]SocialNetwork.Services.Models.AddPostBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model can't be null");
            }
            //Autorize
            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            
            var wallOwner = this.Data.Users.FirstOrDefault(u => u.UserName == model.WallOwnerUsername);


            if (wallOwner == null)
            {
                return this.BadRequest(string.Format("There is no {0} user", wallOwner));
            }

            if (model == null)
            {
                return this.BadRequest("Model cannot be null (no data in request)");
            }


            var post = new Post
            {
                AuthorId = loggedUserId,
                PostedOn = DateTime.Now,
                Content = model.Content,
                WallOwner = wallOwner,
            };

            this.Data.Posts.Add(post);
            this.Data.SaveChanges();

            var data = this.Data.Posts
                .Where(p => p.Id == post.Id)
                .Select(PostViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }

        [HttpPut]
        public IHttpActionResult EditPost(int id, [FromBody]EditPostBindingModel model)
        {
            var post = this.Data.Posts.Find(id);
            if (post == null)
            {
                return this.NotFound();
            }

            var loggetUserId = this.User.Identity.GetUserId();

            if (loggetUserId != post.AuthorId)
            {
                return this.Unauthorized();
            }

            if (model == null)
            {
                return this.BadRequest("Model is empty");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            post.Content = model.Content;

            this.Data.SaveChanges();

            var data = this.Data.Posts
                .Where(p => p.Id == post.Id)
                .Select(PostViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }

        //DELETE api/posts/{id}
        [Authorize(Roles = "Admin")]
        [Route("api/posts/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var post = this.Data.Posts.Find(id);
            if (post == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId != post.AuthorId &&
                loggedUserId != post.WallOwnerId &&
                !this.User.IsInRole("Admin"))
            {
                return this.Unauthorized();
            }

            this.Data.Posts.Remove(post);
            this.Data.SaveChanges();

            return this.Ok();
        }

        

    }
}