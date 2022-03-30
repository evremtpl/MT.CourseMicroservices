

using MT.FreeCourse.Order.Domain.Core;

namespace MT.FreeCourse.Order.Domain.OrderAggregate
{
    public class OrderItem :Entity
    {
        public string CourseId{ get; private set; }
        public string CourseName{ get; private set; }
        public string PictureUrl { get; private set; }
        public decimal Price { get; private set; }

        public OrderItem(string courseId, string courseName, string pictureUrl, decimal price)
        {
            CourseId = courseId;
            CourseName = courseName;
            PictureUrl = pictureUrl;
            Price = price;
        }


        public void UpdateOrderItem(string courseId,string courseName,string pictureUrl, decimal price)
        {
            CourseId = courseId;
            CourseName = courseName;
            PictureUrl = pictureUrl;
            Price = price;


        }
    }
}
