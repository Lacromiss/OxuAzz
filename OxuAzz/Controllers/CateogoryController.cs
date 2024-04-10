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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryPostDto dto)
        {


            Category category = _mapper.Map<Category>(dto);
            category.CreatedDate = DateTime.Now;



            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return StatusCode(200);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDto(int id,[FromBody] CategoryUpdateDto dto)
        {


            var updatedCategory = await _context.Categories.FindAsync(id);
            if (updatedCategory == null)
            {
                return NotFound();
            }

            updatedCategory.UpdatedDate = DateTime.Now;
            updatedCategory.Name = dto.Name;
           
            await _context.SaveChangesAsync();
            return StatusCode(200);
        }

       

    }
}
