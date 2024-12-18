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

        public PlantProfileService(IRepository<PlantProfile> plantProfileRepo)
        {
            _plantProfileRepo = plantProfileRepo;
        }

        public async Task AddPlantProfileAsync(PlantProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            await _plantProfileRepo.AddAsync(profile);
        }

        public async Task UpdatePlantProfileAsync(PlantProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            await _plantProfileRepo.UpdateAsync(profile);
        }

        public async Task<IEnumerable<PlantProfile>> GetAllPlantProfilesAsync()
        {
            return await _plantProfileRepo.GetAllAsync();
        }

        public async Task DeletePlantProfileAsync(int profileId)
        {
            await _plantProfileRepo.DeleteAsync(profileId);
        }

        public async Task<PlantProfile> GetPlantProfileByIdAsync(int id)
        {
            var profile = await _plantProfileRepo.GetByIdAsync(id);
            if (profile == null) throw new KeyNotFoundException($"Plant profile with ID {id} not found.");
            return profile;
        }

        public async Task ActivateProfileAsync(int profileId)
        {
            var profiles = await _plantProfileRepo.GetAllAsync();

            foreach (var profile in profiles)
            {
                profile.Active = profile.Id == profileId;
                await _plantProfileRepo.UpdateAsync(profile);
            }
        }

        public async Task<PlantProfile> GetActiveProfileAsync()
        {
            var profiles = await _plantProfileRepo.GetAllAsync();
            return profiles.FirstOrDefault(p => p.Active);
        }
    }
}