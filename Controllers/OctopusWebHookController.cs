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

        public OctopusWebHookController(ILogger<OctopusWebHookController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post()
        {
            try
            {
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
                    // Do something
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
