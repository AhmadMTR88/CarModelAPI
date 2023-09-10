using CarModelAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarModelAPI.Controllers
{
    [ApiController]
    [Route("api/models")]
    public class ModelsController : ControllerBase
    {
        private readonly CarMakeService _carMakeService;
        private readonly ILogger<ModelsController> _logger;

        public ModelsController(CarMakeService carMakeService, ILogger<ModelsController> logger)
        {
            _logger = logger;
            _carMakeService = carMakeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetModelsByMakeAndYear([FromQuery] string make, [FromQuery] int modelyear)
        {
            var makeId = _carMakeService.GetCarMakeIdByName(make);
            if (makeId == 0)
            {
                return NotFound("Car make not found.");
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = $"https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{modelyear}?format=json";

                    // Send a GET request to the API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        return Content(await response.Content.ReadAsStringAsync(), "application/json");
                    }
                    else
                    {
                        return NotFound($"Failed to call the API. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    return NotFound($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
