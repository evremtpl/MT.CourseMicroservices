
using MT.FreeCourse.Web.Models;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Services.Interfaces
{
   public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
