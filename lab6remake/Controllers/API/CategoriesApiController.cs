using Microsoft.AspNetCore.Mvc;
using lab6remake.Models;
using lab6remake.Repositories.Interfaces;

namespace lab6remake.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesApiController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/CategoriesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _repository.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/CategoriesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục" });
            }

            return Ok(category);
        }

        // POST: api/CategoriesApi
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await _repository.CreateAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT: api/CategoriesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCategory = await _repository.UpdateAsync(id, category);

            if (updatedCategory == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục" });
            }

            return NoContent();
        }

        // DELETE: api/CategoriesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy danh mục" });
            }

            return NoContent();
        }
    }
}