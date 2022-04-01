
using IdentityModel.Client;
using MT.FreeCourse.Shared.Dtos;
using MT.FreeCourse.Web.Models;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Services.Interfaces
{
   public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInInput signInInput);

        Task<TokenResponse> GetAccessTokenByRefreshToken();

        Task RevokeRefreshToken();
    }
}
