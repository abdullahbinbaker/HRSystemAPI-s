using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class Employee : EntityBase
    {
        //public Employee(int id,string name, string emailAddress, string mobileNumber, string gender, string role, DateTime birthDate, int managerId, int vacationDaysAllowedYearly)
        //{
        //    Id = id;
        //    Name = name;
        //    EmailAddress = emailAddress;
        //    MobileNumber = mobileNumber;
        //    Gender = gender;
        //    Role = role;
        //    BirthDate = birthDate;
        //    ManagerId = managerId;
        //    VacationDaysAllowedYearly = vacationDaysAllowedYearly;
        //}

        //public Employee(string name, string emailAddress, string mobileNumber, string gender, string role, DateTime birthDate, int managerId, int vacationDaysAllowedYearly)
        //{
        //    Name = name;
        //    EmailAddress = emailAddress;
        //    MobileNumber = mobileNumber;
        //    Gender = gender;
        //    Role = role;
        //    BirthDate = birthDate;
        //    ManagerId = managerId;
        //    VacationDaysAllowedYearly = vacationDaysAllowedYearly;
        //}

        /// <summary>
        /// Employee Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Employee's Email Address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Employee Mobile Number
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Employee Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Employee Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Employee Birth Date 
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Manager Id Assigned Automatically to each Employee Based on the manager who create him 
        /// </summary>
        public int ManagerId { get; set; }

        /// <summary>
        /// days that the employee can take yearly as a vacation
        /// </summary>
        public int VacationDaysAllowedYearly { get; set; }

        public string JWTToken { get; set; }

        public string RefreshToken { get; set; }

    }
}
