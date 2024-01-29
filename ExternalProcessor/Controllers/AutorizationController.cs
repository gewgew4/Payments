using ExternalProcessor.Helper;
using ExternalProcessor.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExternalProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutorizationController(ILogger<AutorizationController> logger) : ControllerBase
    {
        private readonly ILogger<AutorizationController> _logger = logger;

        [HttpPost(Name = "Autorize")]
        public AutorizationResponse Get(AutorizationRequest request)
        {
            _logger.LogInformation($"{request.Id} has requested approval");

            return new AutorizationResponse
            {
                Id = request.Id,
                IsAutorized = ApplicationHelper.IsInteger(request.Total)
            };
        }
    }
}
