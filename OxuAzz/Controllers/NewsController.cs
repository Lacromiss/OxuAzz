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
        [HttpGet]
        public async Task<IActionResult> GetListAllAsync()
        {
            try
            {
                var newsList = await _context.News.Where(x => !x.isDeleted).ToListAsync();

                if (newsList == null || newsList.Count == 0)
                {
                    return NotFound("No news found");
                }

                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("searchByTitle")]
        public async Task<IActionResult> SearchNews([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return BadRequest("Keyword cannot be empty");
                }

                var newsList = await _context.News.Where(x => x.Title.Contains(keyword) && !x.isDeleted).ToListAsync();


                if (newsList == null || newsList.Count == 0)
                {
                    return NotFound("No news found");
                }

                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while searching news: {ex.Message}");
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterNewsByCategory([FromQuery] int categoryId)
        {
            if (categoryId <= 0)
            {
                return BadRequest("Invalid category Id");
            }

            var categoryExists = await _context.Categories.AnyAsync(x => x.Id == categoryId && !x.isDeleted);
            if (!categoryExists)
            {
                return NotFound("Category not found or deleted");
            }

            var newsList = await _context.News.Where(x => x.isDeleted == false && x.CategoryId == categoryId).ToListAsync();

            return Ok(newsList);
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] NewPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var news1 = _mapper.Map<New>(dto);
                news1.CreatedDate = DateTime.Now;
                news1.isDeleted = false;

                await _context.News.AddAsync(news1);
                await _context.SaveChangesAsync();

                return Ok("News created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating news: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] NewUpdateDto news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedNews = await _context.News.FindAsync(id);
            if (updatedNews == null)
            {
                return NotFound();
            }
            if (news.CategoryId == 0)
            {
                ModelState.AddModelError(nameof(news.CategoryId), "CategoryId field is required");
                return BadRequest(ModelState);
            }

            updatedNews.Title = news.Title;
            updatedNews.Description = news.Description;
            updatedNews.ImgUrl = news.ImgUrl;
            updatedNews.UpdatedDate = DateTime.Now;
            updatedNews.CategoryId = news.CategoryId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("News updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating news: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        //Databazadan bir basa data silmek doqru olmadiqi ucun IsDeleted den istifade etdim.

        public async Task<IActionResult> Delete(int id)
        {
            var removedNew = await _context.News.FindAsync(id);
            if (removedNew == null)
            {
                return NotFound();
            }

            removedNew.isDeleted = true;
            await _context.SaveChangesAsync();

            return Ok("News deleted successfully.");
        }


    }
}
