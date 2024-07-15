using DomainService.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [ApiController]
    [Route("event-management-api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly JobOperations jobOperations;

        public JobController(JobOperations jobOperations)
        {
            this.jobOperations = jobOperations;
        }

        [HttpPost("notify-upcoming-events")]
        public async Task<IActionResult> SendReminders()
        {
            await jobOperations.ExecuteAsync();
            return Ok("Email başarıyla gönderildi.");
        }
    }
}
