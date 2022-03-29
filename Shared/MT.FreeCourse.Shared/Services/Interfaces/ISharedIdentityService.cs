using System;
using System.Collections.Generic;
using System.Text;

namespace MT.FreeCourse.Shared.Services.Interfaces
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }
}
