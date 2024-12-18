using System;
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
        public async Task<IActionResult> AddPlantProfile([FromBody] PlantProfileDto dto)
        {
            if (dto == null) return BadRequest("Invalid plant profile data.");

            var profile = new PlantProfile
            {
                Name = dto.Name,
                IsDefault = dto.IsDefault ?? false, // Default to false if null
                PhMin = dto.PhMin ?? 0,
                PhMax = dto.PhMax ?? 0,
                TemperatureMin = dto.TemperatureMin ?? 0,
                TemperatureMax = dto.TemperatureMax ?? 0,
                LightOnTime = dto.LightOnTime ?? DateTime.MinValue,
                LightOffTime = dto.LightOffTime ?? DateTime.MinValue,
                LightMin = dto.LightMin ?? 0,
                LightMax = dto.LightMax ?? 0,
                EcMin = dto.EcMin ?? 0,
                EcMax = dto.EcMax ?? 0
            };

            await _plantProfileService.AddPlantProfileAsync(profile);
            return Ok("Plant profile created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlantProfile(int id, [FromBody] PlantProfileDto dto)
        {
            if (dto == null || dto.Id != id)
                return BadRequest("Invalid plant profile data or ID mismatch.");

            var profile = new PlantProfile
            {
                Id = id,
                Name = dto.Name,
                IsDefault = dto.IsDefault ?? false,
                Active = dto.Active ?? false,
                PhMin = dto.PhMin ?? 0,
                PhMax = dto.PhMax ?? 0,
                TemperatureMin = dto.TemperatureMin ?? 0,
                TemperatureMax = dto.TemperatureMax ?? 0,
                LightOnTime = dto.LightOnTime ?? DateTime.MinValue,
                LightOffTime = dto.LightOffTime ?? DateTime.MinValue,
                LightMin = dto.LightMin ?? 0,
                LightMax = dto.LightMax ?? 0,
                EcMin = dto.EcMin ?? 0,
                EcMax = dto.EcMax ?? 0
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
