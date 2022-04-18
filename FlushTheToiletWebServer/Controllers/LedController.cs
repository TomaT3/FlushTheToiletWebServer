using System;
using FlushTheToiletWebServer.CF01;
using Microsoft.AspNetCore.Mvc;

namespace FlushTheToiletWebServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LedController : Controller
    {
        private readonly ILedControl mLedControl;

        public LedController(ILedControl ledControl)
        {
            mLedControl = ledControl;
        }

        [HttpPut("on")]
        public IActionResult On([FromBody] string action)
        {
            IActionResult result = Ok();
            try
            {
                switch (action.ToLower())
                {
                    case "red":
                        mLedControl.RedLedOn();
                        break;

                    case "yellow":
                        mLedControl.YellowLedOn();
                        break;

                    case "green":
                        mLedControl.GreenLedOn();
                        break;

                    case "blue":
                        mLedControl.BlueLedOn();
                        break;

                    case "all":
                        mLedControl.TurnAllLedsOn();
                        break;

                    default:
                        result = BadRequest($"led action {action} not known");
                        break;
                }
            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
            }

            return result;
        }

        [HttpPut("off")]
        public IActionResult Off([FromBody] string action)
        {
            IActionResult result = Ok();
            try
            {
                switch (action.ToLower())
                {
                    case "red":
                        mLedControl.RedLedOff();
                        break;

                    case "yellow":
                        mLedControl.YellowLedOff();
                        break;

                    case "green":
                        mLedControl.GreenLedOff();
                        break;

                    case "blue":
                        mLedControl.BlueLedOff();
                        break;

                    case "all":
                        mLedControl.TurnAllLedsOff();
                        break;

                    default:
                        result = BadRequest($"led action {action} not known");
                        break;
                }
            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
            }

            return result;
        }
    }
}