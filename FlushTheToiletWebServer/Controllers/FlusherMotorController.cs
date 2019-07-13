using System;
using FlushTheToiletWebServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlushTheToiletWebServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FlusherMotorController : Controller
    {
        private readonly IFlusherMotorModel mFlusherMotorModel;
        public FlusherMotorController(IFlusherMotorModel flusherMotorModel)
        {
            mFlusherMotorModel = flusherMotorModel;
        }

        /// <summary>
        /// Move motor
        /// </summary>
        /// <param name="action">Possible values: forward, backward, stop</param>
        /// <returns>result</returns>
        [HttpPut()]
        public IActionResult Put([FromBody] string action)
        {
            IActionResult result = Ok();
            try
            {
                switch (action.ToLower())
                {
                    case "forward":
                        mFlusherMotorModel.MotorForward();
                        break;

                    case "backward":
                        mFlusherMotorModel.MotorBackward();
                        break;

                    case "flush":
                        mFlusherMotorModel.Flush();
                        break;

                    case "stop":
                        mFlusherMotorModel.MotorStop();
                        break;

                    default:
                        result = BadRequest("motor direction not known");
                        break;
                }
            }
            catch(Exception ex)
            {
                result = BadRequest(ex);
            }

            return result;
        }
    }
}
