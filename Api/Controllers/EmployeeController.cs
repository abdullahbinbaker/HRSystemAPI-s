using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Core.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Api.ViewsModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Schema;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UnitOfWork uow;
        private readonly IConfiguration _config;
        public EmployeeController(UnitOfWork unitOfWork, IConfiguration config)
        {
            uow = unitOfWork;
            _config = config;
        }


        [HttpGet("ShowEmployeeDetails")]
        public async Task<ActionResult<EmployeeDetailsViewModel>> ShowEmployeeDetails(int id)
        {
            try
            {
                var result = await uow.Employees.GetById(id);
                var result2 =await uow.Logins.GetById(id);
                if(result ==null || result2==null)
                    return NotFound();

                EmployeeDetailsViewModel employeeInfo = new EmployeeDetailsViewModel
                {
                    employee = result,
                    password = result2.Password
                };
                return Ok(employeeInfo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
        }

     
        [HttpGet("ShowManagerDetails")]
        public async Task<ActionResult<Employee>> ShowManagerDetails(int managerId)
        {
           try
            {
                var result = await uow.Employees.RetrieveManagersEmployees(managerId);

                if(result==null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
        }


        [HttpGet("RetrieveManagersEmployees")]
        public async Task<ActionResult> RetrieveManagersEmployees(int managerId)
        {
           try
            {
                var result = await uow.Employees.RetrieveManagersEmployees(managerId);
                if(result==null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
        }




    }
}
