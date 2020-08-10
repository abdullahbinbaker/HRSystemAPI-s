using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Api.Settings;
using Api.ViewsModels;
using Core.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UnitOfWork uow;
        private readonly IConfiguration _config;
        private readonly JWTSettings _jwtsettings;
        public LoginController(UnitOfWork unitOfWork, IConfiguration config ,IOptions<JWTSettings> jwtsettings)
        {
            uow = unitOfWork;
            _config = config;
            _jwtsettings = jwtsettings.Value;
        }


        ////Authentication Practise 
        //[HttpGet("ShowEmployeeDetails")]
        //public async Task<ActionResult<EmployeeDetailsViewModel>> ShowEmployeeDetails()
        //{
        //    try
        //    {
        //        int id = Convert.ToInt32(HttpContext.User.Identity.Name);
        //        var result = await uow.Employees.GetById(id);
        //        var result2 = await uow.Logins.GetById(id);
        //        if (result == null || result2 == null)
        //            return NotFound();

        //        EmployeeDetailsViewModel employeeInfo = new EmployeeDetailsViewModel
        //        {
        //            employee = result,
        //            password = result2.Password
        //        };
        //        return Ok(employeeInfo);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
        //    }
        //}

        //authentication with token jwt 


        //[HttpGet("ShowEmployeeDetailsWithJWT")]
        //public async Task<ActionResult<Employee>> ShowEmployeeDetailsWithJWT()
        //{
        //    try
        //    {
        //        int id = Convert.ToInt32(HttpContext.User.Identity.Name);
        //        Employee employee = await uow.Employees.GetById(id);
               
        //        if (employee == null)
        //            return NotFound();
               
        //        else 
        //        {
        //            RefreshToken refreshToken = GenerateRefreshToken(id);
        //            string jwtToken = GenerateAccessToken(id);

        //            await uow.RefreshTokens.Insert(refreshToken);
        //            uow.Save();

        //            employee.RefreshToken = refreshToken.Token;
        //            employee.JWTToken = jwtToken;
        //            await uow.Employees.Update(employee);
        //            uow.Save();
        //        }
                
        //        return Ok(employee);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
        //    }
        //}

        [AllowAnonymous]
        [HttpPost("RefreshingTheToken")]
        public async Task<ActionResult<Employee>> RefreshingTheToken([FromBody] RefreshRequest refreshRequest)
        {
            int id =Convert.ToInt32( GetEmployeeFromJWTTokenAsync(refreshRequest.JWTToken));
           
            Employee employee = await uow.Employees.GetById(id);

            if (employee != null && await ValidateRefreshTokenAsync(employee, refreshRequest.RefreshToken))
            {
                employee.JWTToken = GenerateAccessToken(employee.Id);
                await uow.Employees.Update(employee);
                uow.Save();

                return employee;
            }
            return null;
        }

        
        [AllowAnonymous]
        //this will return the employee 
        [HttpPost("LoginToSystem")]
        public async Task<ActionResult<Employee>> LoginToSystem([FromBody] LoginRequest loginRequest)
        {
            try
            {
               var employee = await uow.Logins.CheckLogin(loginRequest.EmailAddress, loginRequest.Password);
                if (employee == null)
                    return NotFound();

                else
                {
                   RefreshToken refreshToken = GenerateRefreshToken(employee.Id);
                    string jwtToken = GenerateAccessToken(employee.Id);

                    await uow.RefreshTokens.Insert(refreshToken);
                    uow.Save();

                    employee.RefreshToken = refreshToken.Token;
                    employee.JWTToken = jwtToken;
                    await uow.Employees.Update(employee);
                    uow.Save();
                }

                return Ok(employee);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
            
        }

        [HttpPost("UpdateLoginInfo")]
        public async Task<ActionResult<Login>> UpdateLoginInfo(int employeeId, [FromBody] Login login)
        {       
            try
            {
                if (employeeId != login.EmployeeId)
                    return BadRequest("Ids' isnt match");

                var logiToUpdate = await uow.Logins.GetById(employeeId);

                if (logiToUpdate == null)
                    return NotFound($"Login with id= {employeeId} not found");

                return Ok(await uow.Logins.Update(login));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
           
        }


        [HttpPost("CreateAccount")]
        public async Task<ActionResult<Employee>> CreateAccount(string password,[FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                    return BadRequest();

                //Employee employee1 = new Employee(employee.Name, employee.EmailAddress, employee.MobileNumber, employee.Gender, employee.Role, employee.BirthDate, employee.ManagerId, 30);
                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //mail.From = new MailAddress(_config.GetValue<string>("UserKeys:senderEmail"));
                //mail.To.Add(email);
                //mail.Subject = "Test Mail";
                //mail.Body = "welcome mr/mrs: " + employeeName + " your password is : " + password;
                //SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(_config.GetValue<string>("UserKeys:senderEmail"), _config.GetValue<string>("UserKeys:senderMailPassword"));
                //SmtpServer.EnableSsl = true;
                //SmtpServer.Send(mail);
                //Log("mail Sent succefully");
                var createdEmployee = await uow.Employees.Insert(employee);
                uow.Save();
                Login login = new Login{
                   EmployeeId= createdEmployee.Id,
                   EmailAddress= employee.EmailAddress,
                   Password= password
                };
                var createdLogin =await uow.Logins.Insert(login);
                uow.Save();

                return Ok(createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving the datat from database");
            }
        }

        //helper methods =================================

        private async Task<bool> ValidateRefreshTokenAsync(Employee employee, string token)
        {
            RefreshToken refreshToken = await uow.RefreshTokens.GetTheTokenByTokenProp(token);

            if (refreshToken != null && refreshToken.EmployeeId == employee.Id && refreshToken.ExpiryDate > DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }


        private string GetEmployeeFromJWTTokenAsync(string accessToken)
        {
            var tokenValidationParamters = new TokenValidationParameters
            {
                ValidateAudience = false, // You might need to validate this one depending on your case
                ValidateIssuer = false,
                ValidateActor = false,
                ValidateLifetime = false, // Do not validate lifetime here
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(_jwtsettings.SecretKey)
                    )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParamters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token!");
            }

            var userId = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new SecurityTokenException($"Missing claim: {ClaimTypes.Name}!");
            }

            return userId;
        }

        private string GenerateAccessToken(int employeeId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name,Convert.ToString(employeeId))
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(int userId)
        {
            RefreshToken refreshToken = new RefreshToken();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);
            refreshToken.EmployeeId = userId;
            return refreshToken;
        }


    }
}
