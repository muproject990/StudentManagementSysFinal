using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Infrastructure.Persistence;
using StudentManagement.Infra.services;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public SyncController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // GET: api/Sync/Manual
        [HttpGet("Manual")]
        public async Task<IActionResult> TriggerManualSync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var syncService = scope.ServiceProvider.GetRequiredService<DatabaseSynchronizationService>();

                // Call the method that handles database synchronization.
                await syncService.RunSyncAsync();  // Ensure this method exists and triggers sync.
            }

            return Ok(new { Message = "Manual synchronization triggered. Check logs for details." });
        }

    }
}