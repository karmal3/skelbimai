using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkelbimaiAPI.Entities;

namespace SkelbimaiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public CountryController(SkelbimaiDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public object Get()
        {
            return _context.Country.Select(o => new { o.Id, o.Name }).ToList();
        }
    }
}