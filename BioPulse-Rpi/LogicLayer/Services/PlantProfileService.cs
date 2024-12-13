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

       
        public async Task<PlantProfile> GetPlantProfileByIdAsync(int id)
        {
            var profile = await _plantProfileRepo.GetByIdAsync(id);
            if (profile == null)
                throw new KeyNotFoundException($"Plant profile with ID {id} not found.");

            return profile;
        }
    }
}
