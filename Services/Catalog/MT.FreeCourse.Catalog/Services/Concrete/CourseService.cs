using AutoMapper;
using MongoDB.Driver;
using MT.FreeCourse.Catalog.Dtos;
using MT.FreeCourse.Catalog.Models;
using MT.FreeCourse.Catalog.Services.Interfaces;
using MT.FreeCourse.Catalog.Settings.Abstract;
using MT.FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Catalog.Services.Concrete
{
    public class CourseService :ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
       // private readonly IMongoCollection<Category> _categoryCollection;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CourseService( IMapper mapper, IDatabaseSettings databaseSettings, ICategoryService categoryService)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var databaseName=client.GetDatabase(databaseSettings.DatabaseName);
           _courseCollection = databaseName.GetCollection<Course>(databaseSettings.CourseCollectionName);
          //  _categoryCollection = databaseName.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();
   
            if(courses.Any())
            {
                foreach (var course in courses)
                {
                  var response= await _categoryService.GetByIdAsync(course.CategoryId);
                    course.Category = _mapper.Map<Category>(response.Data);
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>>GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(c => c.Id == id).FirstOrDefaultAsync();
            if(course==null)
            {
                return Response<CourseDto>.Fail("Course not Found", 404);
            }
            var response = await _categoryService.GetByIdAsync(course.CategoryId);
            course.Category = _mapper.Map<Category>(response.Data);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }


        public async Task<Response<List<CourseDto>>> GetAllByUserId(string userId)
        {
            var courses = await _courseCollection.Find(c => c.UserId == userId).ToListAsync();
            if (courses .Any())
            {
                foreach (var course in courses)
                {
                    var response = await _categoryService.GetByIdAsync(course.CategoryId);
                    course.Category = _mapper.Map<Category>(response.Data);
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updatedCourse = _mapper.Map<Course>(courseUpdateDto);
          var result=  await _courseCollection.FindOneAndReplaceAsync(x=>x.Id== courseUpdateDto.Id,updatedCourse);
            if (result == null)
            {
                return Response<NoContent>.Fail("Course Not Found", 404);
            }
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
           
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount >0 )
            {
                return Response<NoContent>.Success(204);
                
            }
            return Response<NoContent>.Fail("Course Not Found", 404);
        }
    }
}
