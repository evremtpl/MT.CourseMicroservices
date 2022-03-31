

using MT.FreeCourse.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.FreeCourse.Order.Domain.OrderAggregate
{

    //EF Core features
    //--Owned Types
    //--Shadow Property
    //--Backing Field
    public class Order :Entity, IAggregateRoot
    {
        public DateTime CreateDate { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }

        private readonly List<OrderItem> _orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {

        }
        public Order(Address address, string buyerId)
        {
            _orderItems = new List<OrderItem>();
            CreateDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
        }
        public void AddOrderItem(string courseId, string courseName,decimal price,string pictureUrl)
        {
            var existCourse = _orderItems.Any(x => x.CourseId == courseId);
            if(!existCourse)
            {
                var newOrderItem = new OrderItem(courseId, courseName, pictureUrl, price);
                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
    }
}
