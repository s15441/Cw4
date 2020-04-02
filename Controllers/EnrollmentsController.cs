using Cw3.Models;
using Cw4.DTOs.Requests;
using Cw4.DTOs.Responses;
using Cw4.Models;
using Cw4.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace Cw4.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;

        public EnrollmentsController()
        {
            _service = new SqlServerStudentDbService();
        }
        [HttpPost]
        public IActionResult AddStudent(EnrollStudentRequest request)
        {
            if (_service.EnrollStudent(request) == null) return BadRequest();
            else return Ok();

        }

        [HttpPost("{promotions}")]
        public IActionResult PromoteStudent(PromoteRequest request)
        {
            Enrollment result = _service.PromoteStudents(request);
            if(result  == null) return NotFound();
            else return Created("", result);
        }
    }
 
}