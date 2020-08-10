using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class LoginRepository : GenericRepository<Login>
    {
        public LoginRepository(DbContext context) : base(context)
        {
        }

        public async Task<string> RetrievePasswordByEmail(string email)
        {
           Login login=await _context.Set<Login>().Where(item => item.EmailAddress.Equals(email)).FirstOrDefaultAsync();
           return login.Password.ToString();
           ////return password;
        }
        public async Task<Employee> CheckLogin(string emailAddress, string password)
        {
            Login loginMember = _context.Set<Login>().Where(item => item.EmailAddress.Equals(emailAddress) && item.Password.Equals(password)).FirstOrDefault();
            Employee employee = await _context.Set<Employee>().Where(item => item.Id == loginMember.EmployeeId).FirstOrDefaultAsync();
            return employee;
        }

        public async Task<Login> Update(Login login)
        {
            Login toUpdate = await _context.Set<Login>().Where(item => item.EmployeeId == login.EmployeeId).FirstOrDefaultAsync();
            if(toUpdate != null)
            {
                toUpdate.EmployeeId = login.EmployeeId;
                toUpdate.EmailAddress = login.EmailAddress;
                toUpdate.Password = login.Password;

                await _context.SaveChangesAsync();
                return toUpdate;
            }

            return null;
        }
    }
}
