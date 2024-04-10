using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxuAzz.Context;
using OxuAzz.Dtos.CategoryDto;
using OxuAzz.Dtos.NewDto;
using OxuAzz.Models;

namespace OxuAzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CateogoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CateogoryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetListAllAsync()
        {
            try
            {
                var categoryList = await _context.Categories
                    .Where(x => !x.isDeleted)
                    .ToListAsync();

                if (categoryList == null || categoryList.Count == 0)
                {
                    return NotFound("No news found.");
                }

                return Ok(categoryList);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CategoryPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = _mapper.Map<Category>(dto);
                category.CreatedDate = DateTime.Now;
                category.isDeleted = false;

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Ok("Category created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating category: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCategory = await _context.Categories.FindAsync(id);
            if (updatedCategory == null)
            {
                return NotFound();
            }
         
            updatedCategory.UpdatedDate = DateTime.Now;

            updatedCategory.Name = dto.Name;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Category updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating category: {ex.Message}");
            }
        }

        //Databazadan bir basa data silmedim .IsDeleted den istifade etdim.
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var removedCategory = await _context.Categories.FindAsync(id);
            if (removedCategory == null)
            {
                return NotFound();
            }


            removedCategory.isDeleted = true;

            await _context.SaveChangesAsync();

            return Ok("Category deleted successfully.");
        }


    }
}
