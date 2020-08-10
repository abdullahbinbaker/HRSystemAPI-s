using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Core.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Api.ViewsModels;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly UnitOfWork uow;
        private readonly IConfiguration _config;
        public HomeController(UnitOfWork unitOfWork, IConfiguration config)
        {
            uow = unitOfWork;
            _config = config;
        }

    }
}
