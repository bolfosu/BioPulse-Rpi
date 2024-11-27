using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
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

        public SensorManagementService(
            TemperatureSensorRepo temperatureSensorRepo,
            EcSensorRepo ecSensorRepo,
            PhSensorRepo phSensorRepo,
            LightSensorRepo lightSensorRepo)
        {
            _temperatureSensorRepo = temperatureSensorRepo;
            _ecSensorRepo = ecSensorRepo;
            _phSensorRepo = phSensorRepo;
            _lightSensorRepo = lightSensorRepo;
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

