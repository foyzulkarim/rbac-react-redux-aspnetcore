using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly PostService _postService;

        public CommentsController(PostService postService)
        {
            _postService = postService;
        }

        // GET: api/Comments
        [HttpGet]
        public ActionResult<List<Comment>> Get(string postId)
        {
            return _postService.Get(postId).Comments;
        }
        
        // POST: api/Comments
        [HttpPost]
        public ActionResult<Comment> Post(string postId, [FromBody] Comment comment)
        {
            return _postService.CreateComment(postId, comment);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete()]
        public Post Delete(string bookId, string commentId)
        {
            return _postService.DeleteComment(bookId, commentId);
        }
    }
}
