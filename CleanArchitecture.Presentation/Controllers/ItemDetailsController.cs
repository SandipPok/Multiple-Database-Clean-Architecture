using CleanArchitecture.Application.Interface;
using CleanArchitecture.Application.Mapper;
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

        // GET: api/item/details
        [HttpGet("getAll")]
        public async Task<IActionResult> GetDetailsByItemIdAsync()
        {
            try
            {
                var details = await _itemDetailsRepository.GetAllDetailsAsync();
                if (details != null && details.Any())
                {
                    return Ok(details);
                }
                return NotFound($"No details found.");
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
