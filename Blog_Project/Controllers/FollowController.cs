using AutoMapper;
using Blog_Project.CORE;
using Blog_Project.CORE.Models.DTO;
using Blog_Project.Migrations;
using Blog_Project.CORE.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<IdentityUser> userManager;

        public FollowController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("followers")]
        [Authorize]
        public async Task<IActionResult> getFollowers()
        {
            var userId = userManager.GetUserId(User);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            
            var followersTask = await unitOfWork.follows.FindAll(f => f.FolloweeId ==  userId, new string[] { "Follower" });
            var followerDtos = followersTask.Select(f => mapper.Map<FollowerDTO>(f.Follower)).ToList();

            return Ok(followerDtos);

        }

        [HttpGet("followees")]
        [Authorize]
        public async Task<IActionResult> getFollowees()
        {
            var userId = userManager.GetUserId(User);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            
            var followeeTask = await unitOfWork.follows.FindAll(f => f.FollowerId == userId, new string[] { "Followee" });
            var followeeDtos = followeeTask.Select(f => mapper.Map<FollowerDTO>(f.Followee)).ToList();

            return Ok(followeeDtos);

        }

        [HttpPost("follow/{id}")]
        [Authorize]
        public async Task<IActionResult> followById(string id)
        {
            var userId = userManager.GetUserId(User);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var followeeUser = await userManager.FindByIdAsync(id);
            if (followeeUser == null)
            {
                return NotFound("User not found");
            }

            var existingFollow = await unitOfWork.follows.Find(f => f.FollowerId == userId && f.FolloweeId == id);
            if (existingFollow != null)
            {
                return Conflict("You are already following this user.");
            }

            var follow = new follow
            {
                FollowerId = userId,
                FolloweeId = id
            };

            await unitOfWork.follows.Add(follow);
            unitOfWork.Complete();

            return Ok("Successfully followed user.");
        }

    }
}
