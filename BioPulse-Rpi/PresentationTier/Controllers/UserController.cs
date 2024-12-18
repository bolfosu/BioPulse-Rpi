// File: PresentationTier/Controllers/UserController.cs

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationTier.DTOs;
using PresentationTier.DTOs.UserDTOs;

namespace PresentationTier.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManagementService _userService;

        public UserController(UserManagementService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (userDto.Name == null || userDto.Email == null || userDto.Password == null ||
                userDto.SecurityQuestion == null || userDto.SecurityAnswer == null)
            {
                return BadRequest("Missing required registration fields.");
            }

            try
            {
                await _userService.RegisterAsync(
                    userDto.Name,
                    userDto.Email,
                    userDto.Password,
                    userDto.SecurityQuestion,
                    userDto.SecurityAnswer,
                    userDto.PhoneNumber
                );
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
        {
            if (userDto.Email == null || userDto.Password == null)
            {
                return BadRequest("Email and Password are required for authentication.");
            }

            try
            {
                var user = await _userService.AuthenticateAsync(userDto.Email, userDto.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the security question for a given email.
        /// </summary>
        [HttpGet("{email}/security-question")]
        public async Task<IActionResult> GetSecurityQuestion(string email)
        {
            var question = await _userService.GetSecurityQuestionAsync(email);
            if (question == null)
                return NotFound("User not found.");

            return Ok(question);
        }

        /// <summary>
        /// Recovers a user's password.
        /// </summary>
        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] UserDto userDto)
        {
            if (userDto.Email == null || userDto.SecurityQuestion == null ||
                userDto.SecurityAnswer == null || userDto.NewPassword == null)
            {
                return BadRequest("Missing required fields for password recovery.");
            }

            try
            {
                await _userService.RecoverPasswordAsync(
                    userDto.Email,
                    userDto.SecurityQuestion,
                    userDto.SecurityAnswer,
                    userDto.NewPassword
                );
                return Ok("Password reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
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

        /// <summary>
        /// Updates a user's email.
        /// </summary>
        [HttpPut("{id}/email")]
        public async Task<IActionResult> UpdateEmail(int id, [FromBody] UserDto userDto)
        {
            if (userDto.NewEmail == null)
            {
                return BadRequest("NewEmail is required for updating email.");
            }

            try
            {
                var user = await _userService.GetByIdAsync(id); // Retrieve the existing user
                user.Email = userDto.NewEmail; // Update email

                await _userService.UpdateUserSettingsAsync(user); // Pass updated user object
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

        /// <summary>
        /// Updates a user's password.
        /// </summary>
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UserDto userDto)
        {
            if (userDto.NewPassword == null)
            {
                return BadRequest("NewPassword is required for updating password.");
            }

            try
            {
                var user = await _userService.GetByIdAsync(id); // Retrieve the existing user
                user.PasswordHash = HashString(userDto.NewPassword); // Hash and update password

                await _userService.UpdateUserSettingsAsync(user); // Pass updated user object
                return Ok("Password updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the password: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a user's security question and answer.
        /// </summary>
        [HttpPut("{id}/security")]
        public async Task<IActionResult> UpdateSecurity(int id, [FromBody] UserDto userDto)
        {
            if (userDto.NewSecurityQuestion == null || userDto.NewSecurityAnswer == null)
            {
                return BadRequest("NewSecurityQuestion and NewSecurityAnswer are required for updating security information.");
            }

            try
            {
                var user = await _userService.GetByIdAsync(id); // Retrieve the existing user
                user.SecurityQuestion = userDto.NewSecurityQuestion; // Update security question
                user.SecurityAnswerHash = HashString(userDto.NewSecurityAnswer); // Hash and update answer

                await _userService.UpdateUserSettingsAsync(user); // Pass updated user object
                return Ok("Security question and answer updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the security information: {ex.Message}");
            }
        }

        /// <summary>
        /// Hashes a string using SHA256.
        /// </summary>
        private string HashString(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
