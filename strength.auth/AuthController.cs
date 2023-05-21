using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace strength.auth;

public class AuthController
{
    [FunctionName("auth")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequestMessage req)
    {
        var result = "no body";
        if (req.Content != null)
        {
            var body = await req.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(body))
            {
                result = body;
            }
        }
        return new OkObjectResult(result);
    }
}