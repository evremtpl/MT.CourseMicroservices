using MT.FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MT.FreeCourse.Discount.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<Response<List<Model.Discount>>> GetAll();

        Task<Response<Model.Discount>> GetById(int id);

        Task<Response<NoContent>> Create(Model.Discount discount);

        Task<Response<NoContent>> Update(Model.Discount discount);

        Task<Response<NoContent>> Delete(int id);

        Task<Response<Model.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}
