using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.Shared.Services.Interfaces;
using MT.FreeCourse.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        

        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CourseController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId));
        }
    }
}
