// File: Controllers/SensorsController.cs

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using PresentationTier.DTOs.SensorDTOs;
using System.Linq;

namespace PresentationTier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly IRepository<Sensor> _sensorRepository;
        private readonly IRepository<SensorReading> _sensorReadingRepository;

        public SensorsController(
            IRepository<Sensor> sensorRepository,
            IRepository<SensorReading> sensorReadingRepository)
        {
            _sensorRepository = sensorRepository;
            _sensorReadingRepository = sensorReadingRepository;
        }

        /// <summary>
        /// Retrieves all sensors along with their readings.
        /// </summary>
        /// <returns>List of SensorDto objects with their SensorReadings.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorDto>>> GetSensors()
        {
            // Fetch all sensors
            var sensors = await _sensorRepository.GetAllAsync();

            // Fetch all sensor readings
            var sensorReadings = await _sensorReadingRepository.GetAllAsync();

            // Group readings by SensorId for efficient lookup
            var readingsGrouped = sensorReadings
                .GroupBy(r => r.SensorId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Map sensors to SensorDto, including their readings
            var sensorDtos = sensors.Select(sensor => new SensorDto
            {
                Id = sensor.Id,
                Name = sensor.Name,
                ExternalSensorId = sensor.ExternalSensorId,
                SensorType = sensor.SensorType,
                ConnectionDetails = sensor.ConnectionDetails,
                SensorReadings = readingsGrouped.ContainsKey(sensor.Id)
                    ? readingsGrouped[sensor.Id].Select(reading => new SensorReadingDto
                    {
                        Id = reading.Id,
                        Timestamp = reading.Timestamp,
                        Value = reading.Value
                    }).ToList()
                    : new List<SensorReadingDto>()
            }).ToList();

            return Ok(sensorDtos);
        }

        /// <summary>
        /// Retrieves a specific sensor by ID along with its readings.
        /// </summary>
        /// <param name="id">The ID of the sensor to retrieve.</param>
        /// <returns>A SensorDto object with its SensorReadings if found; otherwise, 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorDto>> GetSensor(int id)
        {
            // Fetch the sensor by ID
            var sensor = await _sensorRepository.GetByIdAsync(id);

            if (sensor == null)
            {
                return NotFound(new { Message = $"Sensor with ID {id} not found." });
            }

            // Fetch sensor readings for the specific sensor
            var sensorReadings = await _sensorReadingRepository.GetAllAsync();
            var readingsForSensor = sensorReadings
                .Where(r => r.SensorId == id)
                .Select(reading => new SensorReadingDto
                {
                    Id = reading.Id,
                    Timestamp = reading.Timestamp,
                    Value = reading.Value
                })
                .ToList();

            // Map sensor to SensorDto
            var sensorDto = new SensorDto
            {
                Id = sensor.Id,
                Name = sensor.Name,
                ExternalSensorId = sensor.ExternalSensorId,
                SensorType = sensor.SensorType,
                ConnectionDetails = sensor.ConnectionDetails,
                SensorReadings = readingsForSensor
            };

            return Ok(sensorDto);
        }
    }
}
