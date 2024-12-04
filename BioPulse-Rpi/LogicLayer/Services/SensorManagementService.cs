using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using LogicLayer.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class SensorManagementService
    {
        private readonly TemperatureSensorRepo _temperatureSensorRepo;
        private readonly EcSensorRepo _ecSensorRepo;
        private readonly PhSensorRepo _phSensorRepo;
        private readonly LightSensorRepo _lightSensorRepo;
        // I2C Manager for handling hardware-level interactions
        private readonly I2CManager _i2cManager;

        public SensorManagementService(
            TemperatureSensorRepo temperatureSensorRepo,
            EcSensorRepo ecSensorRepo,
            PhSensorRepo phSensorRepo,
            LightSensorRepo lightSensorRepo,
            I2CManager i2cManager)
        {
            _temperatureSensorRepo = temperatureSensorRepo;
            _ecSensorRepo = ecSensorRepo;
            _phSensorRepo = phSensorRepo;
            _lightSensorRepo = lightSensorRepo;
            _i2cManager = i2cManager;
        }

        public async Task<double> ReadTemperatureSensorAsync(int sensorId)
        {
            // Fetch the sensor from the repository
            var sensor = await _temperatureSensorRepo.GetByIdAsync(sensorId);
            if (sensor == null || !sensor.IsEnabled)
                throw new InvalidOperationException("Sensor not found or disabled.");

            // Read the sensor's value via I2C
            var reading = _i2cManager.ReadByte(1, sensor.Address, 0x00); // Adjust register as per sensor specification
            if (reading.HasValue)
            {
                // Update the sensor's reading in the database
                sensor.LastReading = reading.Value;
                sensor.LastReadingTime = DateTime.Now;
                await _temperatureSensorRepo.UpdateAsync(sensor);
                return sensor.LastReading;
            }

            throw new Exception("Failed to read temperature sensor data.");
        }



        /// <summary>
        /// Controls actuators based on the sensor data and thresholds defined in the plant profile.
        /// </summary>
        /// <param name="profileId">The ID of the plant profile containing thresholds.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ControlActuatorsAsync(int profileId)
        {
            // Fetch plant profile thresholds (Replace this method with actual implementation)
            var profile = GetPlantProfile(profileId);

            // Retrieve all enabled temperature sensors
            var sensors = await _temperatureSensorRepo.GetAllAsync();

            foreach (var sensor in sensors.Where(s => s.IsEnabled))
            {
                // Compare sensor readings with profile thresholds and activate actuators
                if (sensor.LastReading > profile.MaxTemperatureThreshold)
                {
                    // Turn on cooling actuator (e.g., fan)
                    _i2cManager.WriteByte(1, sensor.Address, 0x01, 0x00); // Register and value depend on actuator
                }
                else if (sensor.LastReading < profile.MinTemperatureThreshold)
                {
                    // Turn on heating actuator
                    _i2cManager.WriteByte(1, sensor.Address, 0x01, 0x01); // Register and value depend on actuator
                }
            }
        }

        /// <summary>
        /// A placeholder method for fetching plant profile thresholds.
        /// Replace this with actual implementation to retrieve thresholds from the database.
        /// </summary>
        /// <param name="profileId">The ID of the plant profile.</param>
        /// <returns>A dynamic object containing threshold values.</returns>
        private dynamic GetPlantProfile(int profileId)
        {
            // Example of static thresholds
            return new
            {
                MaxTemperatureThreshold = 30.0,
                MinTemperatureThreshold = 20.0
            };
        }
        // CRUD Operations for Temperature Sensor
        public async Task<TemperatureSensor> AddTemperatureSensorAsync(TemperatureSensor sensor)
        {
            await _temperatureSensorRepo.AddAsync(sensor);
            return sensor;
        }

        public async Task<TemperatureSensor> GetTemperatureSensorAsync(int id)
        {
            return await _temperatureSensorRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TemperatureSensor>> GetAllTemperatureSensorsAsync()
        {
            return await _temperatureSensorRepo.GetAllAsync();
        }

        public async Task UpdateTemperatureSensorAsync(TemperatureSensor sensor)
        {
            await _temperatureSensorRepo.UpdateAsync(sensor);
        }

        public async Task DeleteTemperatureSensorAsync(int id)
        {
            await _temperatureSensorRepo.DeleteAsync(id);
        }

        // Enable/Disable Temperature Sensor
        public async Task EnableTemperatureSensorAsync(int id)
        {
            var sensor = await _temperatureSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = true;
                await _temperatureSensorRepo.UpdateAsync(sensor);
            }
        }

        public async Task DisableTemperatureSensorAsync(int id)
        {
            var sensor = await _temperatureSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = false;
                await _temperatureSensorRepo.UpdateAsync(sensor);
            }
        }

        // CRUD Operations for Ec Sensor
        public async Task<EcSensor> AddEcSensorAsync(EcSensor sensor)
        {
            await _ecSensorRepo.AddAsync(sensor);
            return sensor;
        }

        public async Task<EcSensor> GetEcSensorAsync(int id)
        {
            return await _ecSensorRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EcSensor>> GetAllEcSensorsAsync()
        {
            return await _ecSensorRepo.GetAllAsync();
        }

        public async Task UpdateEcSensorAsync(EcSensor sensor)
        {
            await _ecSensorRepo.UpdateAsync(sensor);
        }

        public async Task DeleteEcSensorAsync(int id)
        {
            await _ecSensorRepo.DeleteAsync(id);
        }

        // Enable/Disable Ec Sensor
        public async Task EnableEcSensorAsync(int id)
        {
            var sensor = await _ecSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = true;
                await _ecSensorRepo.UpdateAsync(sensor);
            }
        }

        public async Task DisableEcSensorAsync(int id)
        {
            var sensor = await _ecSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = false;
                await _ecSensorRepo.UpdateAsync(sensor);
            }
        }

        // CRUD Operations for Ph Sensor
        public async Task<PhSensor> AddPhSensorAsync(PhSensor sensor)
        {
            await _phSensorRepo.AddAsync(sensor);
            return sensor;
        }

        public async Task<PhSensor> GetPhSensorAsync(int id)
        {
            return await _phSensorRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PhSensor>> GetAllPhSensorsAsync()
        {
            return await _phSensorRepo.GetAllAsync();
        }

        public async Task UpdatePhSensorAsync(PhSensor sensor)
        {
            await _phSensorRepo.UpdateAsync(sensor);
        }

        public async Task DeletePhSensorAsync(int id)
        {
            await _phSensorRepo.DeleteAsync(id);
        }

        // Enable/Disable Ph Sensor
        public async Task EnablePhSensorAsync(int id)
        {
            var sensor = await _phSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = true;
                await _phSensorRepo.UpdateAsync(sensor);
            }
        }

        public async Task DisablePhSensorAsync(int id)
        {
            var sensor = await _phSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = false;
                await _phSensorRepo.UpdateAsync(sensor);
            }
        }

        // CRUD Operations for Light Sensor
        public async Task<LightSensor> AddLightSensorAsync(LightSensor sensor)
        {
            await _lightSensorRepo.AddAsync(sensor);
            return sensor;
        }

        public async Task<LightSensor> GetLightSensorAsync(int id)
        {
            return await _lightSensorRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LightSensor>> GetAllLightSensorsAsync()
        {
            return await _lightSensorRepo.GetAllAsync();
        }

        public async Task UpdateLightSensorAsync(LightSensor sensor)
        {
            await _lightSensorRepo.UpdateAsync(sensor);
        }

        public async Task DeleteLightSensorAsync(int id)
        {
            await _lightSensorRepo.DeleteAsync(id);
        }

        // Enable/Disable Light Sensor
        public async Task EnableLightSensorAsync(int id)
        {
            var sensor = await _lightSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = true;
                await _lightSensorRepo.UpdateAsync(sensor);
            }
        }

        public async Task DisableLightSensorAsync(int id)
        {
            var sensor = await _lightSensorRepo.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.IsEnabled = false;
                await _lightSensorRepo.UpdateAsync(sensor);
            }
        }
    }
}

