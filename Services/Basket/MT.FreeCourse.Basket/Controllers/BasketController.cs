using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.Basket.Dtos;
using MT.FreeCourse.Basket.Services.Interfaces;
using MT.FreeCourse.Shared.ControllerBases;
using MT.FreeCourse.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var response = await _basketService.GetBasket(_sharedIdentityService.GetUserId);
            return CreateActionResultInstance(response);
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(BasketDto basketDto)
        {
            var response= await _basketService.CreateOrUpdate(basketDto);

            return CreateActionResultInstance(response);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            var response = await _basketService.Delete(_sharedIdentityService.GetUserId);

            return CreateActionResultInstance(response);
        }
    }
}
