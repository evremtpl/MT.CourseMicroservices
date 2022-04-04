using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.Web.Models;
using MT.FreeCourse.Web.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Controllers
{
    public class AuthController : Controller
    {
        private IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn( SignInInput signInInput)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var response = await _identityService.SignIn(signInInput);
            if (!response.IsSuccessFul)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError("", x);

                });

                return View();
            }


            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
