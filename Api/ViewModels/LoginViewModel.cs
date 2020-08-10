using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewsModels
{
    public class LoginViewModel
    {
        public Employee employee { get; set; }
        public List<Holiday> MyHolidaysList { get; set; }
    }
}
