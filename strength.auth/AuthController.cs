using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace strength.auth;

public class AuthController
{
    [FunctionName("auth")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequestMessage req, ILogger logger)
    {
        logger.Log(LogLevel.Information, "point hit");
        var result = "no body";
        if (req.Content != null)
        {
            var body = await req.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(body))
            {
                logger.Log(LogLevel.Information, body);
                result = body;
            }
        }
        return new OkObjectResult(result);
    }
}