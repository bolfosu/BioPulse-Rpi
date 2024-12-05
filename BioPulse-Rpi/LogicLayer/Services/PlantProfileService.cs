using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class PlantProfileService
    {
        private readonly IRepository<PlantProfile> _plantProfileRepo;
        private readonly IRepository<Sensor> _sensorRepo;

        public PlantProfileService(IRepository<PlantProfile> plantProfileRepo, IRepository<Sensor> sensorRepo)
        {
            _plantProfileRepo = plantProfileRepo;
            _sensorRepo = sensorRepo;
        }

        // Add a new plant profile
        public async Task AddPlantProfileAsync(PlantProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));

            Console.WriteLine($"Adding new plant profile: {profile.Name}");
            await _plantProfileRepo.AddAsync(profile);
        }

        // Update an existing plant profile
        public async Task UpdatePlantProfileAsync(PlantProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));

            Console.WriteLine($"Updating plant profile: {profile.Name}");
            await _plantProfileRepo.UpdateAsync(profile);
        }

        // Get all plant profiles
        public async Task<IEnumerable<PlantProfile>> GetAllPlantProfilesAsync()
        {
            return await _plantProfileRepo.GetAllAsync();
        }

        // Delete a plant profile
        public async Task DeletePlantProfileAsync(int profileId)
        {
            Console.WriteLine($"Deleting plant profile with ID: {profileId}");
            await _plantProfileRepo.DeleteAsync(profileId);
        }

        // Logic for interacting with actuators based on thresholds
        public void HandleSensorReadings(Sensor sensor)
        {
            Console.WriteLine($"Handling sensor: {sensor.Name}, LastReading: {sensor.LastReading}");

            if (!sensor.IsEnabled) return;

            // Example: Interact with actuators based on sensor thresholds
            var actuatorLogic = new Dictionary<string, Action>
            {
                ["Temperature"] = () =>
                {
                    if (sensor.LastReading < 18)
                        Console.WriteLine("Turning on heater...");
                    else if (sensor.LastReading > 24)
                        Console.WriteLine("Turning on cooler...");
                },
                ["pH"] = () =>
                {
                    if (sensor.LastReading < 5.5)
                        Console.WriteLine("Adding pH buffer...");
                    else if (sensor.LastReading > 6.5)
                        Console.WriteLine("Adding acid...");
                },
                // Add additional sensor-logic mappings
            };

            if (actuatorLogic.ContainsKey(sensor.SensorType.ToString()))
            {
                actuatorLogic[sensor.SensorType.ToString()].Invoke();
            }
        }
    }
}
