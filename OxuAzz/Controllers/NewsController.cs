using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxuAzz.Context;
using OxuAzz.Dtos;
using OxuAzz.Dtos.NewDto;
using OxuAzz.Models;

namespace OxuAzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NewsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewPostDto dto)
        {
            
            
            New news1 = _mapper.Map<New>(dto);
            news1.CreatedDate = DateTime.Now;



            await _context.News.AddAsync(news1);
            await _context.SaveChangesAsync();
            return StatusCode(200);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] NewUpdateDto news)
        {
            var updatedNews = await _context.News.FindAsync(id);
            if (updatedNews == null)
            {
                return NotFound();
            }
            updatedNews.Title = news.Title;
            updatedNews.Description = news.Description;
            updatedNews.ImgUrl = news.ImgUrl;
            updatedNews.UpdatedDate = DateTime.Now;
            updatedNews.CategoryId = news.CategoryId;
          await  _context.SaveChangesAsync();
            return StatusCode(200);
        }
    }
}
