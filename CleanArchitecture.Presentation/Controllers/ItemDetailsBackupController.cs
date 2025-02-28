using CleanArchitecture.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDetailsBackupController : ControllerBase
    {
        private readonly IItemDetailsBackupRepository _itemDetailsBackupRepository;

        public ItemDetailsBackupController(IItemDetailsBackupRepository itemDetailsBackupRepository)
        {
            _itemDetailsBackupRepository = itemDetailsBackupRepository;
        }

        // POST: api/item/details/backup/{detailId}
        [HttpDelete("details/backup/{detailId}")]
        public async Task<IActionResult> BackupItemDetailAsync(int detailId)
        {
            try
            {
                var result = await _itemDetailsBackupRepository.BackupItemDetailAsync(detailId);
                if (result > 0)
                {
                    return Ok(new { message = "Item detail backed up successfully." });
                }
                return BadRequest("Failed to back up item detail.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/item/details/backup
        [HttpGet("details/backup")]
        public async Task<IActionResult> GetAllBackedUpDetailsAsync()
        {
            try
            {
                var backedUpDetails = await _itemDetailsBackupRepository.GetAllBackedUpDetailsAsync();
                if (backedUpDetails != null && backedUpDetails.Any())
                {
                    return Ok(backedUpDetails);
                }
                return NotFound("No backed up item details found.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
