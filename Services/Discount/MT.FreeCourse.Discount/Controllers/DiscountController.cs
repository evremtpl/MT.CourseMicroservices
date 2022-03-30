using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.Discount.Services.Interfaces;
using MT.FreeCourse.Shared.ControllerBases;
using MT.FreeCourse.Shared.Services.Interfaces;
using System.Threading.Tasks;

namespace MT.FreeCourse.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var response = await _discountService.GetAll();
            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _discountService.GetById(id);

            return CreateActionResultInstance(response);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            var response = await _discountService.GetByCodeAndUserId(code, userId);

            return CreateActionResultInstance(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create( Model.Discount discount)
        {
            var response = await _discountService.Create(discount);

            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Model.Discount discount)
        {
            var response = await _discountService.Update(discount);

            return CreateActionResultInstance(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _discountService.Delete(id);

            return CreateActionResultInstance(response);
        }
    }
}
