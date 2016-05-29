using SocialNetwork.Models;
using SocialNetwork.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace SocialNetweork.WepServeses.Controllers
{
    [Authorize]
    public class CommentsController : BaseApiController
    {

        //Post api/posts/{postId}/comments
        [HttpPost]
        [Route("api/posts/{postId}/comments")]
        public IHttpActionResult AddCommentToPost(
            int postId,
            AddCommentBindingModel model)
        {
            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            if (model == null)
            {
                return this.BadRequest("Empty model is not allowed");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.User.Identity.GetUserId();

            var comment = new Comment()
            {
                Content = model.Content,
                PostedOn = DateTime.Now,
                AuthorId = userId
            };

            post.Comments.Add(comment);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }

    }
