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
    public class CategoryController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public CategoryController(SkelbimaiDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public object Get()
        {
            return _context.Category.Select(o => new { o.Id, o.Name, AdsCount = _context.Skelbimas.Count(z => o.Id == z.FkCategoryId) }).ToList();
        }
    }
}