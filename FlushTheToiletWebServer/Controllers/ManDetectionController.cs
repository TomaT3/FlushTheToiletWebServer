using FlushTheToiletWebServer.ApiModels;
using FlushTheToiletWebServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlushTheToiletWebServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ManDetectionController : Controller
    {
        private readonly IManDetectionModel mManDetectionModel;
        public ManDetectionController(IManDetectionModel manDetectionModel)
        {
            mManDetectionModel = manDetectionModel;
        }
        // GET: api/ManDetection
        [HttpGet()]
        public JsonResult Distance()
        {
            var distance = new DistanceDTO()
            {
                Distance = mManDetectionModel.GetDistance()
            };

            return Json(distance);
        }
    }
}
