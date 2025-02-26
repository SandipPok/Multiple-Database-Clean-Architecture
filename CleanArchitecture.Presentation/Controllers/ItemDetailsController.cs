using CleanArchitecture.Application.Interface;
using Domain_Layer.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDetailsController : ControllerBase
    {
        private readonly IItemDetailsRepository _itemDetailsRepository;

        public ItemDetailsController(IItemDetailsRepository itemDetailsRepository)
        {
            _itemDetailsRepository = itemDetailsRepository;
        }

        // POST: api/item/details
        [HttpPost("details")]
        public async Task<IActionResult> CreateItemDetailAsync([FromBody] ItemDetail itemDetail)
        {
            try
            {
                if (itemDetail == null)
                {
                    return BadRequest("Item detail cannot be null.");
                }

                var result = await _itemDetailsRepository.CreateItemDetailAsync(itemDetail);
                if (result > 0)
                {
                    return Ok(new { message = "Item detail created successfully." });
                }
                return BadRequest("Failed to create item detail.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/item/details/{itemId}
        [HttpGet("details/{itemId}")]
        public async Task<IActionResult> GetDetailsByItemIdAsync(int itemId)
        {
            try
            {
                var details = await _itemDetailsRepository.GetDetailsByItemIdAsync(itemId);
                if (details != null && details.Any())
                {
                    return Ok(details);
                }
                return NotFound($"No details found for Item ID {itemId}.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
