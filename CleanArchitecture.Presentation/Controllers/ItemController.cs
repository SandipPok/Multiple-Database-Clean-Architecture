using CleanArchitecture.Application.Interface;
using Domain_Layer.Modal;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // POST: api/item
        [HttpPost]
        public async Task<IActionResult> CreateItemAsync([FromBody] Item item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest("Item cannot be null.");
                }

                var result = await _itemRepository.CreateItemAsync(item);
                if (result > 0)
                {
                    return Ok(new { message = "Item created successfully.", ItemId = result });
                }
                return BadRequest("Failed to create item.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: api/item
        [HttpGet]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            try
            {
                var items = await _itemRepository.GetAllItemsAsync();
                if (items != null && items.Any())
                {
                    return Ok(items);
                }
                return NotFound("No items found.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: api/item/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id)
        {
            try
            {
                if (id == 0) return BadRequest("Id can't be null");
                var item = await _itemRepository.GetItemByIdAsync(id);
                if (item != null)
                {
                    return Ok(item);
                }
                return NotFound($"Item with ID {id} not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // PUT: api/item/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] Item item)
        {
            try
            {
                if (item == null || item.ItemId != id)
                {
                    return BadRequest("Invalid item data.");
                }

                var result = await _itemRepository.UpdateItemAsync(item);
                if (result > 0)
                {
                    return Ok(new { message = "Item updated successfully." });
                }
                return BadRequest("Failed to update item.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/item/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            try
            {
                if (id == 0) return BadRequest("Id can't be null");
                var result = await _itemRepository.DeleteItemAsync(id);
                if (result > 0)
                {
                    return Ok(new { message = "Item deleted successfully." });
                }
                return BadRequest("Failed to delete item.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
