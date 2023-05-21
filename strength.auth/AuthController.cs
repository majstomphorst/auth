using System;
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
    public static async Task<IActionResult> Auth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequestMessage req, ILogger logger)
    {
        if (!Authorize(req, logger))
        {
            logger.LogWarning("HTTP basic authentication validation failed.");
            return new UnauthorizedResult();
        }

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
        return new OkObjectResult(new ResponseContent());
    }

    private static bool Authorize(HttpRequestMessage req, ILogger log)
    {
        // Get the environment's credentials
        string username = Environment.GetEnvironmentVariable("BASIC_AUTH_USERNAME", EnvironmentVariableTarget.Process);
        string password = Environment.GetEnvironmentVariable("BASIC_AUTH_PASSWORD", EnvironmentVariableTarget.Process);

        // Returns authorized if the username is empty or not exists.
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            log.LogInformation("HTTP basic authentication is not set.");
            return true;
        }

        // Check if the HTTP Authorization header exist
        if (req.Headers.Authorization == null)
        {
            log.LogWarning("Missing HTTP basic authentication header.");
            return false;
        }

        // Read the authorization header
        var auth = req.Headers.Authorization.ToString();

        // Ensure the type of the authorization header id `Basic`
        if (!auth.StartsWith("Basic "))
        {
            log.LogWarning("HTTP basic authentication header must start with 'Basic '.");
            return false;
        }

        // Get the the HTTP basinc authorization credentials
        var cred = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');

        // Evaluate the credentials and return the result
        return cred[0] == username && cred[1] == password;
    }
}