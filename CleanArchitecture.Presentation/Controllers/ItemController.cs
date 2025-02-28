using CleanArchitecture.Application.Interface;
using CleanArchitecture.Application.Mapper;
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
        [HttpPost("insert")]
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

        // GET: api/get-all
        [HttpGet("get-all")]
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

        // GET: api/getById/{id}
        [HttpGet("getById/{id}")]
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
        [HttpPut("update/{id}")]
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
        [HttpDelete("delete/{id}")]
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

        // POST api/items/create
        [HttpPost("insert/parent/child")]
        public async Task<IActionResult> CreateItemWithDetails([FromBody] ItemWithDetailsDto itemWithDetails)
        {
            if (itemWithDetails == null || itemWithDetails.Item == null || itemWithDetails.ItemDetails == null || !itemWithDetails.ItemDetails.Any())
            {
                return BadRequest("Invalid data provided.");
            }

            try
            {
                // Calling the service method to handle the insert logic
                await _itemRepository.CreateItemWithDetails(itemWithDetails.Item, itemWithDetails.ItemDetails);

                // Return a success response
                return Ok("Item and details successfully created.");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and return a 500 Internal Server Error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
