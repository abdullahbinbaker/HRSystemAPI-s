using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.ViewsModels;
using Microsoft.Extensions.Configuration;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly UnitOfWork uow;
        private readonly IConfiguration _config;
        public HolidayController(UnitOfWork unitOfWork, IConfiguration config)
        {
            uow = unitOfWork;
            _config = config;
        }

        [HttpPost("UpdateHoliday")]
        public async Task<ActionResult<HolidayViewModel>> UpdateHoliday(int holidayId,[FromBody] Holiday holiday)
        {
            try
            {
                if (holidayId != holiday.Id)
                    return BadRequest("Holiday Id's mismatch");

                var holidayToUpdate = await uow.Holidays.GetById(holidayId);

                if (holidayToUpdate == null)
                    return NotFound($"holiday with id= {holidayId} not found");

                return Ok(await uow.Holidays.UpdateHoliday(holiday));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
            
        }


        [HttpGet("ListHolidaysByStatus")]
        public async Task<ActionResult> ListHolidaysByStatus(string status)
        { 
            try
            {
                return Ok( await uow.Holidays.ListHolidaysByStatus(status));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
        }


        [HttpGet("ListEmployeeHolidaysByStatus")]
        public async Task<ActionResult> ListEmployeeHolidaysByStatus(int userId, string status)
        {
            try
            {
                return Ok(await uow.Holidays.ListHolidaysByEmployeeIdAndStatus(userId, status));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
        }


        [HttpGet("ListEmployeeHolidays")]
        public async Task<ActionResult<Holiday>> ListEmployeeHolidays(int userId)
        {
            try
            {
                return Ok(await uow.Holidays.ListHolidaysByEmployeeId(userId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }

        }


     
        [HttpPost("CreateHoliday")]
        public async Task<ActionResult<Holiday>> CreateHoliday([FromBody]Holiday holiday)
        {
            try {

                if (holiday == null)
                    return BadRequest();

                var createdHoliday=await uow.Holidays.Insert(holiday);
                uow.Save();

                return CreatedAtAction(nameof(GetHolidayById), new { id = createdHoliday.Id }, createdHoliday);
                }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
        }

        [HttpGet("GetHolidayById")]
        public async Task<ActionResult<Holiday>> GetHolidayById(int id)
        {
            try 
            {
                var holiday = await uow.Holidays.GetById(id);

                if (holiday == null)
                    return NotFound();

                return holiday;
            }catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retreiving the datat from database");
            }
        }

    }
}
