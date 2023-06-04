using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TerminalProjectApi.Database;
using TerminalProjectApi.Hubs;
using TerminalProjectApi.Interfaces;
using TerminalProjectApi.Models;

namespace TerminalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly ILogger<FlightsController> _logger;
        private readonly IControlTower _controltower;
        private readonly IHubContext<TerminalHub> _terminalHubContext;

        public FlightsController(DataDbContext dbContext, ILogger<FlightsController> logger, IControlTower controltower , IHubContext<TerminalHub> flightHubContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _controltower = controltower;
            _terminalHubContext = flightHubContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            try
            {

                //_logger.LogInformation("");
                return Ok("sss");
            }
            catch (Exception e)
            {
                //_logger.LogError(e,$"{e.Message},Some Exception");
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlightById(int id)
        {
            return await _dbContext.Flights.FirstOrDefaultAsync(f => f.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> CreateFlight([FromBody]FlightDto f)
        {
            //Adding to the db
            var flight = new Flight{FlightNumber = f.FlightNumber , IsArrival = f.IsArrival , AirLine = f.AirLine , MadeContact = f.MadeContactAt };
            var response = _dbContext.Flights.AddAsync(flight);
            _dbContext.SaveChanges();
            flight.Id = response.Result.Entity.Id;
            _terminalHubContext.Clients.All.SendAsync("FlightCreated", flight);
            await _controltower.MoveFlight(flight);
            return Ok(f);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _dbContext.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            _dbContext.Flights.Remove(flight);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] Flight flight)
        {
            var existingFlight = _dbContext.Flights.FirstOrDefault(f => f.Id == id);

            if (existingFlight == null)
            {
                return NotFound(); // Return 404 if the flight doesn't exist
            }

            // Update the properties of the existingFlight object with the new values
            existingFlight.FlightNumber = flight.FlightNumber;
            existingFlight.IsArrival = flight.IsArrival;
            existingFlight.AirLine = flight.AirLine;
            existingFlight.MadeContact = flight.MadeContact;
            existingFlight.CurrentLocation = flight.CurrentLocation;
            // ...

            _dbContext.SaveChanges(); // Save the changes to the database

            return Ok(existingFlight); // Return the updated flight object
        }
        [HttpPut("location/{id}")]
        public async Task<IActionResult> UpdateCurrentFlightLocation(int id, [FromBody] int currentLocation)
        {
            var existingFlight = _dbContext.Flights.FirstOrDefault(f => f.Id == id);

            if (existingFlight == null)
            {
                return NotFound(); // Return 404 if the flight doesn't exist
            }

            // Update the properties of the existingFlight object with the new values
            existingFlight.CurrentLocation = currentLocation;
            // ...

            _dbContext.SaveChanges(); // Save the changes to the database

            return Ok(existingFlight); // Return the updated flight object
        }
    }
}
