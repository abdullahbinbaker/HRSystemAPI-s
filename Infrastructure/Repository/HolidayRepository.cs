using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class HolidayRepository : GenericRepository<Holiday>
    {
        public HolidayRepository(DbContext context) : base(context)
        {
        }

        public async Task<Holiday> UpdateHoliday(Holiday holiday)
        {
            Holiday toUpdate= _context.Set<Holiday>().Where(item => item.Id == holiday.Id).FirstOrDefault();
            if (toUpdate != null)
            {
                toUpdate.EmployeeId = holiday.EmployeeId;
                toUpdate.HolidayStartDate = holiday.HolidayStartDate;
                toUpdate.HolidayEndDate = holiday.HolidayEndDate;
                toUpdate.HolidayStatus = holiday.HolidayStatus;
                toUpdate.HolidayReason = holiday.HolidayReason;

                await _context.SaveChangesAsync();
                return toUpdate;
            }
            return null;
            
          }

        public async Task<IEnumerable<Holiday>> ListHolidaysByEmployeeId(int id)
        {
             //List<Holiday> results;
             var results =await  _context.Set<Holiday>().Where(item => item.EmployeeId == id).ToListAsync();
             return results;
        }

        public async Task<IEnumerable<Holiday>> ListHolidaysByEmployeeIdAndStatus(int id, string status)
        {
            // List<Holiday> results;
            return await _context.Set<Holiday>().Where(item => item.EmployeeId == id && item.HolidayStatus.Equals(status)).ToListAsync();    
        }

        public async Task<IEnumerable<Holiday>> ListHolidaysByEmployeeIdAndStartDate(int id, DateTime startDate)
        {
            //List<Holiday> results;
            return await _context.Set<Holiday>().Where(item => item.EmployeeId == id && item.HolidayStartDate == startDate).ToListAsync();
        }

        public async Task<IEnumerable<Holiday>> ListHolidaysByStatus(string status)
        {
            //List<Holiday> results;
            return await _context.Set<Holiday>().Where(item => item.HolidayStatus.Equals(status)).ToListAsync();
        }

    }
}
