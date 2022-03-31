
namespace MT.FreeCourse.Order.Application.Dtos
{
    public class OrderItemDto
    {
        public string CourseId { get;  set; }
        public string CourseName { get;  set; }
        public string PictureUrl { get;  set; }
        public decimal Price { get;  set; }
    }
}
