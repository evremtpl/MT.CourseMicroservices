using MediatR;
using MT.FreeCourse.Order.Application.Commands;
using MT.FreeCourse.Order.Application.Dtos;
using MT.FreeCourse.Order.Domain.OrderAggregate;
using MT.FreeCourse.Order.Infrastructure;
using MT.FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MT.FreeCourse.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);
            Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(newAddress, request.BuyerId);

            request.OrderItems.ForEach(x =>
            {
                newOrder.AddOrderItem(x.CourseId,x.CourseName,x.Price,x.PictureUrl);

            });
           await _context.Orders.AddAsync(newOrder);

            var result = await _context.SaveChangesAsync();
           return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 204);
        }
    }
}
