using System;
using System.Threading.Tasks;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationTier.DTOs;
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
}
