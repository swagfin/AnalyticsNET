using AnalyticsNET.API.Entity;
using AnalyticsNET.API.Entity.Requests;
using AnalyticsNET.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AnalyticsNET.API.Controllers
{
    [AllowAnonymous]
    [Route("/")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IAnalyticUserApplicationPersistanceService _analyticUserApplicationPersistanceService;
        private readonly int _defaultCallBackInt = 60000;

        public AnalyticsController(ILogger<AnalyticsController> logger, IAnalyticUserApplicationPersistanceService analyticUserApplicationPersistanceService)
        {
            this._logger = logger;
            this._analyticUserApplicationPersistanceService = analyticUserApplicationPersistanceService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Redirect("/index.html");
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromForm] DeviceAnalyticRequest request)
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
                //Check the App Secret
                AnalyticUserApplication app = await this._analyticUserApplicationPersistanceService.GetByAppSecretAsync(appSecret);
                if (app == null) return new UnauthorizedObjectResult("The provided app secret is invalid or does not seem to belong to a registered application");

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
                string sessionGen = $"ANONYMOUS-{request.AppName}{ipAddress.ToMD5String()}-{DateTime.UtcNow:yyyyMMdd}";
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
