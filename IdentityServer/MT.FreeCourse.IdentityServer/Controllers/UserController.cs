using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.IdentityServer.Dtos;
using MT.FreeCourse.IdentityServer.Models;
using MT.FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace MT.FreeCourse.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task <IActionResult> SignUp (SignUpDto signUpDto )
        {
            var user = new ApplicationUser
            {
                UserName=signUpDto.Username,
                Email=signUpDto.Email,
                City=signUpDto.City

            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if(!result.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x=>x.Description).ToList(),400));
            }
            return NoContent();
        }
    }
}
