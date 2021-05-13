using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorsOpen.Data;
using DoorsOpen.Models;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoorsOpen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly SiteDbContext _context;
        private readonly IConfiguration _config;

        public ValuesController(SiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public List<DoorsOpen.Models.BuildingViewModel> Get()
        {
            return _context.Buildings.Select(b => new BuildingViewModel(b, _config.GetValue<string>("AzureImagePrefix"))).ToList();
        }
    }
}
