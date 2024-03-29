﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.FreeCourse.Catalog.Dtos;
using MT.FreeCourse.Catalog.Services.Interfaces;
using MT.FreeCourse.Shared.ControllerBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomBaseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var response = await _courseService.GetAllAsync();
            return CreateActionResultInstance(response);
        }
        //course?id=4
        [HttpGet("{id}")] //Course/id
        public async Task<IActionResult> GetById(string id)
        {

            var response = await _courseService.GetByIdAsync(id);
            return CreateActionResultInstance<CourseDto>(response);
        }
       // [HttpGet("{id:length(24)}", Name = "GetAllByuserId")]
       [HttpGet]
        [Route("/api/[controller]/GetAllByuserId/{userId}")]
        public async Task<IActionResult> GetAllByuserId(string userId)
        {

            var response = await _courseService.GetAllByUserId(userId);
            return CreateActionResultInstance(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {

            var response = await _courseService.CreateAsync(courseCreateDto);
            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {

            var response = await _courseService.UpdateAsync(courseUpdateDto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            var response = await _courseService.DeleteAsync(id);
            return CreateActionResultInstance(response);
        }
    }
}
