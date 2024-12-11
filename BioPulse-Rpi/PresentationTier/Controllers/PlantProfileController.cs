using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationTier.DTOs.PlantProfileDTOs;

namespace PresentationTier.Controllers
{
    [ApiController]
    [Route("api/plant-profiles")]
    public class PlantProfileController : ControllerBase
    {
        private readonly PlantProfileService _plantProfileService;

        public PlantProfileController(PlantProfileService plantProfileService)
        {
            _plantProfileService = plantProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlantProfiles()
        {
            var profiles = await _plantProfileService.GetAllPlantProfilesAsync();
            var profileDtos = profiles.Select(profile => new PlantProfileDto
            {
                Id = profile.Id,
                Name = profile.Name,
                IsDefault = profile.IsDefault,
                PhMin = profile.PhMin,
                PhMax = profile.PhMax,
                TemperatureMin = profile.TemperatureMin,
                TemperatureMax = profile.TemperatureMax,
                LightOnTime = profile.LightOnTime,
                LightOffTime = profile.LightOffTime,
                LightMin = profile.LightMin,
                LightMax = profile.LightMax,
                EcMin = profile.EcMin,
                EcMax = profile.EcMax
            });

            return Ok(profileDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlantProfileById(int id)
        {
            var profile = await _plantProfileService.GetPlantProfileByIdAsync(id);
            if (profile == null)
                return NotFound("Plant profile not found.");

            return Ok(new PlantProfileDto
            {
                Id = profile.Id,
                Name = profile.Name,
                IsDefault = profile.IsDefault,
                PhMin = profile.PhMin,
                PhMax = profile.PhMax,
                TemperatureMin = profile.TemperatureMin,
                TemperatureMax = profile.TemperatureMax,
                LightOnTime = profile.LightOnTime,
                LightOffTime = profile.LightOffTime,
                LightMin = profile.LightMin,
                LightMax = profile.LightMax,
                EcMin = profile.EcMin,
                EcMax = profile.EcMax
            });
        }


        [HttpPost]
        public async Task<IActionResult> AddPlantProfile([FromBody] CreatePlantProfileDto createDto)
        {
            var profile = new PlantProfile
            {
                Name = createDto.Name,
                IsDefault = createDto.IsDefault,
                PhMin = createDto.PhMin,
                PhMax = createDto.PhMax,
                TemperatureMin = createDto.TemperatureMin,
                TemperatureMax = createDto.TemperatureMax,
                LightOnTime = createDto.LightOnTime,
                LightOffTime = createDto.LightOffTime,
                LightMin = createDto.LightMin,
                LightMax = createDto.LightMax,
                EcMin = createDto.EcMin,
                EcMax = createDto.EcMax
            };

            await _plantProfileService.AddPlantProfileAsync(profile);
            return Ok("Plant profile created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlantProfile([FromBody] UpdatePlantProfileDto updateDto)
        {
            var profile = new PlantProfile
            {
                Id = updateDto.Id,
                Name = updateDto.Name,
                IsDefault = updateDto.IsDefault,
                PhMin = updateDto.PhMin,
                PhMax = updateDto.PhMax,
                TemperatureMin = updateDto.TemperatureMin,
                TemperatureMax = updateDto.TemperatureMax,
                LightOnTime = updateDto.LightOnTime,
                LightOffTime = updateDto.LightOffTime,
                LightMin = updateDto.LightMin,
                LightMax = updateDto.LightMax,
                EcMin = updateDto.EcMin,
                EcMax = updateDto.EcMax
            };

            await _plantProfileService.UpdatePlantProfileAsync(profile);
            return Ok("Plant profile updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantProfile(int id)
        {
            await _plantProfileService.DeletePlantProfileAsync(id);
            return Ok("Plant profile deleted successfully.");
        }
    }
}
