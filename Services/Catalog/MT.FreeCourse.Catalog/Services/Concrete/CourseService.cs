using AutoMapper;
using MongoDB.Driver;
using MT.FreeCourse.Catalog.Models;
using MT.FreeCourse.Catalog.Settings.Abstract;


namespace MT.FreeCourse.Catalog.Services.Concrete
{
    internal class CourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMapper _mapper;

        public CourseService( IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var databaseName=client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = databaseName.GetCollection<Course>(databaseSettings.CourseCollectionName);

            _mapper = mapper;
        }
    }
}
