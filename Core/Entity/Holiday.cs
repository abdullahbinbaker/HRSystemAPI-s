using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class Holiday : EntityBase
    {
        //public Holiday(int employeeId, DateTime holidayStartDate, DateTime holidayEndDate, string holidayStatus, string holidayReason)
        //{
        //    EmployeeId = employeeId;
        //    HolidayStartDate = holidayStartDate;
        //    HolidayEndDate = holidayEndDate;
        //    HolidayStatus = holidayStatus;
        //    HolidayReason = holidayReason;

        //}

        /// <summary>
        /// The Employee Id
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// the date that Employee's vacation will start
        /// </summary>
        [Required]
        [Column(TypeName = "date")]
        public DateTime HolidayStartDate { get; set; }

        /// <summary>
        /// Vacation's End Date
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime HolidayEndDate { get; set; }

        /// <summary>
        /// approaved-rejected-Underprocessing
        /// </summary>
        public string HolidayStatus { get; set; }

        /// <summary>
        /// The reasn for requesting this Vacation
        /// </summary>
        public string HolidayReason { get; set; }
        
    }
}
