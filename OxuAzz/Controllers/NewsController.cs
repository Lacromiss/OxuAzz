using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxuAzz.Context;
using OxuAzz.Dtos;
using OxuAzz.Dtos.NewDto;
using OxuAzz.Models;
using OxuAzz.Validations.News.News;
using System.ComponentModel.DataAnnotations;

namespace OxuAzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NewsController(AppDbContext context, IMapper mapper )
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
                

                var newsList = await _context.News.Where(x => x.Title.Trim().ToLower().Contains(keyword.Trim().ToLower()) && !x.isDeleted).ToListAsync();


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

        [HttpGet("filterNewsByCategory")]
        public async Task<IActionResult> FilterNewsByCategory([FromQuery] int categoryId)
        {
            if (categoryId <= 0 && categoryId==null)
            {
                return BadRequest("Invalid category Id");
            }

            var categoryExists = await _context.Categories.AnyAsync(x => x.Id == categoryId && x.isDeleted==false);
            if (!categoryExists)
            {
                return NotFound("Category not found or deleted");
            }

            var newsList = await _context.News.Where(x => x.isDeleted == false && x.CategoryId == categoryId).ToListAsync();

            if (newsList==null && newsList.Count==0)
            {

                return BadRequest("News not found or deleted");

            }
            return Ok(newsList);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetNewsWithPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            if (pageNumber <= 0 || pageSize <= 0  || pageSize>20)
            {
                return BadRequest("Invalid page number or page size");
            }

            var totalItems = await _context.News.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageNumber > totalPages)
            {
                return NotFound("Page not found");
            }

            var newsList = await _context.News
                .Where(x => x.isDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginationData = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                PageNumber = pageNumber,
                News = newsList
            };

            return Ok(paginationData);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] NewPostDto dto)
        {

            var validationResult = await new NewDtoValidation().ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.isDeleted == false);
            if (!categoryExists)
            {
                return BadRequest("The specified category ID does not exist or is deleted.");
            }

            var news1 = _mapper.Map<New>(dto);
                news1.CreatedDate = DateTime.Now;
                news1.isDeleted = false;

                await _context.News.AddAsync(news1);
                await _context.SaveChangesAsync();

                return Ok("News created successfully");
            
         


        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateAsync(int id, [FromBody] NewPostDto news)
        {

            var validationResult = await new NewDtoValidation().ValidateAsync(news);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updatedNews = await _context.News.FirstOrDefaultAsync(x=>x.isDeleted==false && x.Id==id);
            if (updatedNews == null)
            {
                return NotFound();
            }
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == news.CategoryId && c.isDeleted == false);
            if (!categoryExists)
            {
                return BadRequest("The specified category ID does not exist or is deleted.");
            }


            updatedNews.Title = news.Title;
            updatedNews.Description = news.Description;
            updatedNews.ImgUrl = news.ImgUrl;
            updatedNews.UpdatedDate = DateTime.Now;
            updatedNews.CategoryId = news.CategoryId;



                await _context.SaveChangesAsync();
                return Ok("News updated successfully");

        }
        [HttpDelete("{id}")]
        //Databazadan bir basa data silmek doqru olmadiqi ucun IsDeleted den istifade etdim.

        public async Task<IActionResult> Delete(int id)
        {
            var removedNew = await _context.News.FirstOrDefaultAsync(x=>x.isDeleted==false && x.Id==id);
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
