

using MT.FreeCourse.Web.Models;
using MT.FreeCourse.Web.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Services.Concrete
{
   
    public class UserService : IUserService
    {
        private readonly HttpClient _userHttpClient;

        public UserService(HttpClient userHttpClient)
        {
            _userHttpClient = userHttpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            return await _userHttpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");
        }
    }
}
