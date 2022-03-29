using MT.FreeCourse.Basket.Dtos;
using MT.FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Basket.Services.Interfaces
{
   public interface IBasketService
    {
        Task <Response<BasketDto>> GetBasket (string userId);
        Task<Response<bool>> CreateOrUpdate(BasketDto basketDto);
       public Task<Response<bool>> Delete(string userId);
    }
}
