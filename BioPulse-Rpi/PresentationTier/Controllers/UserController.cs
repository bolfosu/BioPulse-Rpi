using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationTier.DTOs.UserDTOs;
namespace PresentationTier.Controllers;

[Route("api/users")] // Ensure this route is defined
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManagementService _userService;

    public UserController(UserManagementService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")] // This becomes "http://localhost:5000/api/users/register"
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            await _userService.RegisterAsync(
                registerDto.Name,
                registerDto.Email,
                registerDto.Password,
                registerDto.SecurityQuestion,
                registerDto.SecurityAnswer,
                registerDto.PhoneNumber
            );
            return Ok("User registered successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("authenticate")] // This becomes "http://localhost:5000/api/users/authenticate"
    public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
    {
        try
        {
            var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("{email}/security-question")]
    public async Task<IActionResult> GetSecurityQuestion(string email)
    {
        var question = await _userService.GetSecurityQuestionAsync(email);
        if (question == null)
            return NotFound("User not found.");

        return Ok(question);
    }


    [HttpPost("recover-password")]
    public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordDto dto)
    {
        try
        {
            await _userService.RecoverPasswordAsync(dto.Email, dto.SecurityQuestion, dto.SecurityAnswer, dto.NewPassword);
            return Ok("Password reset successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving user by ID: {ex.Message}");
        }
    }
    
    [HttpPut("{id}/email")]
    public async Task<IActionResult> UpdateEmail(int id, [FromBody] UpdateEmailDto dto)
    {
        try
        {
            await _userService.UpdateUserSettingsAsync(
                userId: id,
                newEmail: dto.NewEmail,
                newPassword: null,
                newPhoneNumber: null,
                newSecurityQuestion: null,
                newSecurityAnswer: null
            );

            return Ok("Email updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred while updating the email: {ex.Message}");
        }
    }
        
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDto dto)
        {
            try
            {
                await _userService.UpdateUserSettingsAsync(
                    userId: id,
                    newEmail: null,
                    newPassword: dto.NewPassword,
                    newPhoneNumber: null,
                    newSecurityQuestion: null,
                    newSecurityAnswer: null
                );

                return Ok("Password updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"An error occurred while updating the password: {ex.Message}");
            }
        }

    [HttpPut("{id}/security")]
        public async Task<IActionResult> UpdateSecurity(int id, [FromBody] UpdateSecurityDto dto)
        {
            try
            {
                await _userService.UpdateUserSettingsAsync(
                    userId: id,
                    newEmail: null,
                    newPassword: null,
                    newPhoneNumber: null,
                    newSecurityQuestion: dto.NewSecurityQuestion,
                    newSecurityAnswer: dto.NewSecurityAnswer
                );

                return Ok("Security question and answer updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"An error occurred while updating the security information: {ex.Message}");
            }
        }
    
}
