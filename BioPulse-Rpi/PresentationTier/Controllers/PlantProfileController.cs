using System.Collections.Generic;
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
            try
            {
                var profile = await _plantProfileService.GetPlantProfileByIdAsync(id);
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPlantProfile([FromBody] CreatePlantProfileDto createDto)
        {
            if (createDto == null) return BadRequest("Invalid plant profile data.");

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlantProfile(int id, [FromBody] UpdatePlantProfileDto updateDto)
        {
            if (updateDto == null) 
                return BadRequest("Invalid plant profile data.");

            if (id != updateDto.Id)
                return BadRequest("ID in the URL does not match ID in the body.");

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

            try
            {
                await _plantProfileService.UpdatePlantProfileAsync(profile);
                return Ok("Plant profile updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                // Log the exception details (optional)
                return StatusCode(500, "An error occurred while updating the plant profile.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantProfile(int id)
        {
            await _plantProfileService.DeletePlantProfileAsync(id);
            return Ok("Plant profile deleted successfully.");
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivatePlantProfile(int id)
        {
            try
            {
                await _plantProfileService.ActivateProfileAsync(id);
                return Ok($"Plant profile with ID {id} activated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePlantProfile()
        {
            var activeProfile = await _plantProfileService.GetActiveProfileAsync();
            if (activeProfile == null)
                return NotFound("No active plant profile found.");

            return Ok(new PlantProfileDto
            {
                Id = activeProfile.Id,
                Name = activeProfile.Name,
                IsDefault = activeProfile.IsDefault,
                PhMin = activeProfile.PhMin,
                PhMax = activeProfile.PhMax,
                TemperatureMin = activeProfile.TemperatureMin,
                TemperatureMax = activeProfile.TemperatureMax,
                LightOnTime = activeProfile.LightOnTime,
                LightOffTime = activeProfile.LightOffTime,
                LightMin = activeProfile.LightMin,
                LightMax = activeProfile.LightMax,
                EcMin = activeProfile.EcMin,
                EcMax = activeProfile.EcMax
            });
        }
    }
}
