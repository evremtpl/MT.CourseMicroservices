using Microsoft.AspNetCore.Http;
using MT.FreeCourse.Shared.Services.Interfaces;
using System;
using System.Linq;

namespace MT.FreeCourse.Shared.Services.Concrete
{
    public class SharedIdentityService : ISharedIdentityService
    {

        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
