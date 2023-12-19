using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiniValidation;
using NetDevPack.Identity.Jwt;
using NetDevPack.Identity.Model;

namespace ControleDeHorasExtras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<IResult> Registro(
                [FromServices] SignInManager<IdentityUser> signInManager,
                [FromServices] UserManager<IdentityUser> userManager,
                IOptions<AppJwtSettings> appJwtSettings,
                [FromBody] RegisterUser registerUser)
        {
            if (registerUser == null)
                return Results.BadRequest("Usuário não informado.");

            if (!MiniValidator.TryValidate(registerUser, out var errors))
                return Results.ValidationProblem(errors);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            var jwt = new JwtBuilder()
                .WithUserManager(userManager)
                .WithJwtSettings(appJwtSettings.Value)
                .WithEmail(user.Email)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .BuildUserResponse();

            return Results.Ok(jwt);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IResult> Login(
           [FromServices] SignInManager<IdentityUser> signInManager,
           [FromServices] UserManager<IdentityUser> userManager,
            IOptions<AppJwtSettings> appJwtSettings,
           [FromBody] LoginUser loginUser)
        {
            if (loginUser == null)
                return Results.BadRequest("Usuário não informado.");

            if (!MiniValidator.TryValidate(loginUser, out var errors))
                return Results.ValidationProblem(errors);

            var result = await signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, true, true);

            if (result.IsLockedOut)
                return Results.BadRequest("Usuário bloqueado.");

            if (!result.Succeeded)
                return Results.BadRequest("Usuário ou senha inválidos.");

            var jwt = new JwtBuilder()
                        .WithUserManager(userManager)
                        .WithJwtSettings(appJwtSettings.Value)
                        .WithEmail(loginUser.Email)
                        .WithJwtClaims()
                        .WithUserClaims()
                        .WithUserRoles()
                        .BuildUserResponse();

            return Results.Ok(new { Token = jwt.AccessToken });
        }

        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            return Ok(new { Message = "Logout successful. Please remove the stored JWT." });
        }
    }
}