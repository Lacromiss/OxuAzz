using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxuAzz.Context;
using OxuAzz.Dtos.CategoryDto;
using OxuAzz.Dtos.NewDto;
using OxuAzz.Models;
using OxuAzz.Validations.Categories;
using OxuAzz.Validations.News.News;

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
                var categoryList = await _context.Categories.Where(x => !x.isDeleted).ToListAsync();

                if (categoryList == null || categoryList.Count == 0)
                {
                    return NotFound("No news found");
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
            var validationResult = await new CategoryPostValidation().ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            var category = _mapper.Map<Category>(dto);
                category.CreatedDate = DateTime.Now;
                category.isDeleted = false;

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Ok("Category createdd successfully");
            
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CategoryPostDto dto)
        {

            var validationResult = await new CategoryPostValidation().ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            var updatedCategory = await _context.Categories.FirstOrDefaultAsync(x=>x.Id==id && x.isDeleted==false);
            if (updatedCategory == null)
            {
                return NotFound();
            }
         
            updatedCategory.UpdatedDate = DateTime.Now;

            updatedCategory.Name = dto.Name;

           
                await _context.SaveChangesAsync();
                return Ok("Category updated successfully");
            
           
        }

        //Databazadan bir basa data silmedim .IsDeleted den istifade etdim.
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var removedCategory = await _context.Categories.FirstOrDefaultAsync(x=>x.Id==id && x.isDeleted==false);
            if (removedCategory == null)
            {
                return NotFound();
            }


            removedCategory.isDeleted = true;

            await _context.SaveChangesAsync();

            return Ok("Category deleted successfully");
        }


    }
}
