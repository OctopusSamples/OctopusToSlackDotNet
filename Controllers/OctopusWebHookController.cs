using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OctopusToSlackDotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OctopusWebHookController : ControllerBase
    {
        private readonly ILogger<OctopusWebHookController> _logger;
        private readonly string _token;

        public OctopusWebHookController(ILogger<OctopusWebHookController> logger)
        {
            _logger = logger;
            _token = Environment.GetEnvironmentVariable("API_TOKEN");
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_token))
                {
                    throw new ApplicationException("API_TOKEN environment variable must be set.");
                }

                if (Request.Headers["API-TOKEN"] != _token)
                {
                    throw new ArgumentException("The token supplied in the API-TOKEN does not match the configured token.");
                }
                
                using (var reader = new StreamReader(Request.Body))
                {
                    var data = await reader.ReadToEndAsync();
                    
                    var octoMessage = JsonConvert.DeserializeObject<OctoMessage>(data);

                    var client = new SlackClient(Environment.GetEnvironmentVariable("SLACK_URI_APIKEY"));

                    var slackMessage = string.Format(
                        "{0} (by {1}) - <{2}|Go to Octopus>", 
                        octoMessage.Message, 
                        octoMessage.Username,
                        octoMessage.GetSpaceUrl());
                
                    var responseText = client.PostMessage(slackMessage);                
                    return Ok(responseText);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
