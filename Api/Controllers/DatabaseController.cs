using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : Controller
    {
        private readonly ILogger<DatabaseController> logger;
        private readonly VehiclesContext vehiclesContext;

        public DatabaseController(ILogger<DatabaseController> logger, VehiclesContext vehiclesContext)
        {
            this.logger = logger;
            this.vehiclesContext = vehiclesContext;
        }


        [HttpGet("Reset")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> ResetAsync()
        {

            // Deleting the database
            await this.vehiclesContext.Database.EnsureDeletedAsync();

            // Creating it again
            await this.vehiclesContext.Database.EnsureCreatedAsync();


            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = Guid.NewGuid(), Make = "Make On Server", Year = "2022", VehicleType = VehicleType.VA },
                new Vehicle { Id = Guid.NewGuid(), Make = "Make On Server", Year = "2022", VehicleType = VehicleType.VA },
                new Vehicle { Id = Guid.NewGuid(), Make = "Make On Server", Year = "2022", VehicleType = VehicleType.VA },
            };


            await this.vehiclesContext.Vehicle.AddRangeAsync(vehicles);

            await this.vehiclesContext.SaveChangesAsync();

            // get all vehicles without cache activated
            var insertedVehicles = await this.vehiclesContext.Vehicle.AsNoTracking().ToListAsync();

            return insertedVehicles;

            //var jObject = JObject.FromObject(new { Id = Guid.NewGuid(), Type = "Test" });
            //return Content(jObject.ToString(), "application/json");
        }


        [HttpGet("Vehicles")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            var allVehicles = await this.vehiclesContext.Vehicle.ToListAsync();

            return allVehicles;
        }
    }
}
