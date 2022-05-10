using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using MT.FreeCourse.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser =  _userManager.FindByEmailAsync(context.UserName).Result;//username değil email gelecek

            if(existUser==null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Email or Password are not correct" });
                context.Result.CustomResponse = errors;
                return;
            }
            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);
            if(!passwordCheck)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Email or Password are not correct" });
                context.Result.CustomResponse = errors;
                return;
            }
            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
