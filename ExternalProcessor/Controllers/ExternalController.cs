using ExternalProcessor.Helper;
using ExternalProcessor.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExternalProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalController(ILogger<ExternalController> logger) : ControllerBase
    {
        private readonly ILogger<ExternalController> _logger = logger;

        [HttpPost]
        public ExternalResponse Post(ExternalRequest request)
        {
            _logger.LogInformation($"{request.Id} has requested approval");
            var x = new ExternalResponse
            {
                Id = request.Id,
                IsAutorized = ApplicationHelper.IsInteger(request.Total)
            };

            return x;
        }
    }
}
