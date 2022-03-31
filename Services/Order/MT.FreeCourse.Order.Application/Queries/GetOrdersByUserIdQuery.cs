using MediatR;
using MT.FreeCourse.Order.Application.Dtos;
using MT.FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.FreeCourse.Order.Application.Queries
{
   public class GetOrdersByUserIdQuery :IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
