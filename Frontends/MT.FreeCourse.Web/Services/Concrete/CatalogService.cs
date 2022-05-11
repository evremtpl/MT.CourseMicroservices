using MT.FreeCourse.Shared.Dtos;
using MT.FreeCourse.Web.Models.Catalogs;
using MT.FreeCourse.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Services.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput createInput)
        {
            var response = await _client.PostAsJsonAsync<CourseCreateInput>("course", createInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {

            var response = await _client.DeleteAsync($"course/{courseId}");

            return response.IsSuccessStatusCode;
        }

        public  async Task<List<CategoryViewModel>> GetAllCategoryAsync(int id)
        {
            var response = await _client.GetAsync("course");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseSuccess.Data;
        }
        //http:localhost:5000/services/catalogs/courses
        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response= await _client.GetAsync("category");
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess= await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            return responseSuccess.Data;

        }
        ///api/[controller]/GetAllByuserId/{userId}
        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"course/GetAllByuserId/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            return responseSuccess.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await _client.GetAsync($"course/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            return responseSuccess.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput updateInput)
        {
            var response = await _client.PutAsJsonAsync<CourseUpdateInput>("course", updateInput);

            return response.IsSuccessStatusCode;
        }
    }
}
