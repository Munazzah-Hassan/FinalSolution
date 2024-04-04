using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
namespace FinalSolution
{
    public class CustomSignInManager : SignInManager<IdentityUser>
    {
        public CustomSignInManager(UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<IdentityUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<IdentityUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<IdentityUser> userConfirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, userConfirmation)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByNameAsync(userName);

                // Get the user's current claims
                var existingClaims = await UserManager.GetClaimsAsync(user);

                // Remove any existing LastLoggedIn claim (if any)
                var lastLoggedInClaim = existingClaims.FirstOrDefault(c => c.Type == "LastLoggedIn");
                if (lastLoggedInClaim != null)
                {
                    await UserManager.RemoveClaimAsync(user, lastLoggedInClaim);
                }

                // Add the updated LastLoggedIn claim
                var newLastLoggedInClaim = new Claim("LastLoggedIn", DateTime.Now.ToString());
                await UserManager.AddClaimAsync(user, newLastLoggedInClaim);
            }

            return result;
        }

    }

}