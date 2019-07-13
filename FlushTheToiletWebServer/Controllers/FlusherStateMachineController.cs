using FlushTheToiletWebServer.ApiModels;
using FlushTheToiletWebServer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FlushTheToiletWebServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FlusherStateMachineController : Controller
    {
        private readonly IToiletFlusherStateMachineModel mToiletFlusherStateMachineModel;
        public FlusherStateMachineController(IToiletFlusherStateMachineModel toiletFlusherStateMachineModel)
        {
            mToiletFlusherStateMachineModel = toiletFlusherStateMachineModel;
        }

        [HttpGet()]
        public JsonResult Status()
        {
            var status = mToiletFlusherStateMachineModel.GetStatus();
            var statusDTO = new FlusherStateMachienStatusDTO()
            {
                IsInAutomaticMode = status.IsInAutomaticMode,
                CurrentState = status.CurrentState,
                Distance = status.Distance.Meters
            };

            return Json(statusDTO);
        }

        [HttpPut()]
        public IActionResult Start([FromBody] string action)
        {
            IActionResult result = Ok();

            try
            {
                switch(action.ToLower())
                {
                    case "start":
                        mToiletFlusherStateMachineModel.StartToiletStateMachine();
                        break;
                    case "stop":
                        mToiletFlusherStateMachineModel.StopToiletStateMachine();
                        break;
                    default:
                        result = BadRequest($"state machine action {action} not known");
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
