using AnalyticsNET.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AnalyticsNET.API.Controllers
{
    [AllowAnonymous]
    [Route("/")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(ILogger<AnalyticsController> logger)
        {
            this._logger = logger;
        }

        [HttpPost]
        public ActionResult Post([FromForm] DeviceAnalyticRequest request)
        {
            try
            {
                ModelState.ValidateOrThrow();
                _logger.LogInformation("received analytic request");
                //Get Required Info
                string ipAddress = Request.GetClientIpAddress();
                string appSecret = string.Format("{0}", Request.Headers["appSecret"])?.ToString().Trim();
                if (string.IsNullOrWhiteSpace(appSecret)) return new BadRequestObjectResult("App Secret Key was not Provided");
                if (appSecret.Length < 10) return new BadRequestObjectResult("App Secret was not in the correct format or Length");

                //** process
                if (request.TraitKey.Equals("cryptData", StringComparison.OrdinalIgnoreCase))
                {
                    string decryptedContent = request.TraitValue.StuDecrypt(2);
                    _logger.LogInformation(decryptedContent);
                }
                else
                {
                    _logger.LogInformation(request.TraitValue);
                }
                //response
                string sessionGen = string.Format("ANONYMOUS-{0}-{1}", $"{request.AppName}{ipAddress}".ToMD5String(), DateTime.UtcNow.ToString("yyyyMMdd"));
                //Handle Request to Subscribers
                return Content($"10000|{sessionGen}|OK");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
