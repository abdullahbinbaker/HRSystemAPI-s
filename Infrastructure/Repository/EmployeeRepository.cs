using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        public EmployeeRepository(DbContext context) : base(context)
        {
        }
         
        //public async Task<Employee> CheckLogin(string emailAddress, string password)
        //{
        //    Login loginMember = _context.Set<Login>().Where(item => item.EmailAddress.Equals(emailAddress) && item.Password.Equals(password)).FirstOrDefault();
        //    Employee employee = await _context.Set<Employee>().Where(item => item.Id == loginMember.EmployeeId).FirstOrDefaultAsync();
        //    return employee;
        //}

        public async Task<Employee> SearchForEmployeeByEmail(string emailAddress)
        {
           return await _context.Set<Employee>().Where(item => item.EmailAddress.Equals(emailAddress)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> RetrieveManagersEmployees(int managerId)
        {
            return await _context.Set<Employee>().Where(item => item.ManagerId == managerId).ToListAsync();
        }

        public async Task<Employee> Update(Employee employee)
        {
            Employee toUpdate = await _context.Set<Employee>().Where(item => item.Id == employee.Id).FirstOrDefaultAsync();
            if (toUpdate != null)
            {
                toUpdate.Id = employee.Id;
                toUpdate.Name = employee.Name;
                toUpdate.EmailAddress = employee.EmailAddress;
                toUpdate.MobileNumber = employee.MobileNumber;
                toUpdate.Gender = employee.Gender;
                toUpdate.Role = employee.Role;
                toUpdate.BirthDate = employee.BirthDate;
                toUpdate.ManagerId = employee.ManagerId;
                toUpdate.JWTToken = employee.JWTToken;
                toUpdate.RefreshToken = employee.RefreshToken;

                await _context.SaveChangesAsync();
                return toUpdate;
            }

            return null;
        }

    }
}
