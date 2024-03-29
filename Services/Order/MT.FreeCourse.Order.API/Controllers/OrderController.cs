﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.Order.Application.Commands;
using MT.FreeCourse.Order.Application.Queries;
using MT.FreeCourse.Shared.ControllerBases;
using MT.FreeCourse.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomBaseController
    {

        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
           
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdentityService.GetUserId });

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand createOrderCommand )
        {
            var response = await _mediator.Send(createOrderCommand);

            return CreateActionResultInstance(response);
        }
    }
}
