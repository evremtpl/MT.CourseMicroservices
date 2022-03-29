﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.PhotoStock.Dtos;
using MT.FreeCourse.Shared.ControllerBases;
using MT.FreeCourse.Shared.Dtos;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MT.FreeCourse.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : CustomBaseController
    {

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                var returnPath = "photos/" + photo.FileName;


                PhotoDto photoDto = new() { Url = returnPath }; //C# 9
                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is empty", 400));
        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found",404));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}