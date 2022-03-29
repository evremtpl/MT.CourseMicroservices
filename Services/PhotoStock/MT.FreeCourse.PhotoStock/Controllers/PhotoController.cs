using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.PhotoStock.Dtos;
using MT.FreeCourse.Shared.ControllerBases;
using MT.FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MT.FreeCourse.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : CustomBaseController
    {

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo,CancellationToken cancellationToken)
        {
            if(photo!=null && photo.Length>0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                    await photo.CopyToAsync(stream, cancellationToken);
                
                var returnPath="photos/"+ photo.FileName;


                PhotoDto photoDto = new() { Url = returnPath }; //C# 9
                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto,200));
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is empty", 400));
        }
    }
}
