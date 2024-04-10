using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxuAzz.Context;
using OxuAzz.Models;

namespace OxuAzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] New news)
        {


            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
            return StatusCode(200);
        }
    }
}
