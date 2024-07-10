using AutoMapper;
using Blog_Project.CORE;
using Blog_Project.CORE.Models.DTO;
using Blog_Project.CORE.Models.Domain;
using Blog_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork UnitOfWork;

        private readonly UserManager<IdentityUser> UserManager;

        public CommentController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this.mapper = mapper;
            this.UnitOfWork = unitOfWork;
            UserManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllComments(Guid id)
        {
            
            var blogPost = await UnitOfWork.BlogPosts.GetById(id, new string[] { "Categories", "Comments", "Comments.User" });
            if (blogPost is null)
            {
                return NotFound();
            }
            
            var commentsDTO = mapper.Map<IEnumerable<CommentDTO>>(blogPost.Comments);

            return Ok(commentsDTO);

        }

        [HttpPost]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest createCommentRequest)
        {
            // check if there is a blog with that id then convert dto into comment then save it
            var userId = UserManager.GetUserId(User);
            var user = await UserManager.FindByIdAsync(userId);

            // check BlogPost
            var blogPost = await UnitOfWork.BlogPosts.GetById(createCommentRequest.BlogPostId, new string[] { "Categories" , "Comments" });
            if (blogPost == null)
            {
                return NotFound("Blog post not found.");
            }

            if (user == null)
            {
                return Unauthorized();
            }
            
            var comment = new Comment
            {
                Content = createCommentRequest.Content,
                DateCreated = DateTime.UtcNow,
                BlogPostId = createCommentRequest.BlogPostId,
                BlogPost = blogPost,
                UserId = user.Id
            };

            await UnitOfWork.comments.Add(comment);
            UnitOfWork.Complete();
            return Ok(comment);

        }

        [HttpPut]
        public async Task<IActionResult> EditComment([FromBody] EditCommentRequest editCommentRequest)
        {
            // get the comment id and user currently login
            var userId = UserManager.GetUserId(User);
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized();
            }

            
            // find comment
            var comment  = await UnitOfWork.comments.GetById(editCommentRequest.Id, new string[] { "User" });
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            // make sure the userid = currently userid
            if (comment.UserId != userId)
            {
                return Unauthorized("You are not authorized to edit this comment.");
            }

            comment.Content = editCommentRequest.Content;
            comment.DateCreated = DateTime.UtcNow;

            // make the changes
            var response = await UnitOfWork.comments.Update(comment);

            // save
            UnitOfWork.Complete();
            return Ok(response);
        }
    }
}
