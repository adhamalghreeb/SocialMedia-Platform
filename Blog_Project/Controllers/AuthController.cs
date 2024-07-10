using Blog_Project.CORE.@interface;
using Blog_Project.CORE.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public ITokenRepository TokenRepository { get; }

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            TokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create User Object
            var user = new IdentityUser
            {
                UserName = request.Email.Trim(),
                Email = request.Email.Trim(),
            };


            // add user
            var identity = await userManager.CreateAsync(user, request.Password);

            // add role
            if (identity.Succeeded)
            {
                identity = await userManager.AddToRoleAsync(user, "Reader");
                if (identity.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identity.Errors.Any())
                    {
                        foreach (var error in identity.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

            }
            else
            {
                if (identity.Errors.Any())
                {
                    foreach (var error in identity.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // check email
            var identity = await userManager.FindByEmailAsync(request.Email);

            if(identity is not null)
            {
                // check password
                var checkPassword = await userManager.CheckPasswordAsync(identity, request.Password);

                if (checkPassword)
                {

                    var roles = await userManager.GetRolesAsync(identity);

                    var jwtToken = TokenRepository.CreateJwtTok(identity, roles.ToList());

                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken

                    };

                    return Ok(response);
                }

            }
            ModelState.AddModelError("", "Email or Password Incorrect");
            return ValidationProblem(ModelState);
               
        }
    }
}