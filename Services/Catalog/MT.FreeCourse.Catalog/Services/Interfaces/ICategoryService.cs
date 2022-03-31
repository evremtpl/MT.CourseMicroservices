using MT.FreeCourse.Catalog.Dtos;
using MT.FreeCourse.Catalog.Models;
using MT.FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Catalog.Services.Interfaces
{
    public interface ICategoryService

    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);

        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
