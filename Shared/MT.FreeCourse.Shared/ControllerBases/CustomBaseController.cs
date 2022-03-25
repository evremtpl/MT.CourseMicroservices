using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.Shared.Dtos;

namespace MT.FreeCourse.Shared.ControllerBases
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode=response.StatusCode
            };
        }
    }
}
