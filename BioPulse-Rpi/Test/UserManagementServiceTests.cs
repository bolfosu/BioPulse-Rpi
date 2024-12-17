using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LogicLayer.Tests
{
    public class UserManagementServiceTests
    {
        private readonly Mock<UserRepo> _mockUserRepo;
        private readonly UserManagementService _service;

        public UserManagementServiceTests()
        {
            _mockUserRepo = new Mock<UserRepo>(null);
            _service = new UserManagementService(_mockUserRepo.Object);
        }

        [Fact]
        public async Task RegisterAsync_ValidUser_ReturnsTrue()
        {
            // Arrange
            string name = "John Doe";
            string email = "johndoe@example.com";
            string password = "securepassword";
            string securityQuestion = "What is your pet's name?";
            string securityAnswer = "Fluffy";

            _mockUserRepo.Setup(repo => repo.EmailExistsAsync(email)).ReturnsAsync(false);

            // Act
            var result = await _service.RegisterAsync(name, email, password, securityQuestion, securityAnswer);

            // Assert
            Assert.True(result);
            _mockUserRepo.Verify(repo => repo.AddAsync(It.Is<User>(u =>
                u.Name == name &&
                u.Email == email &&
                u.SecurityQuestion == securityQuestion &&
                u.SecurityAnswerHash != null &&
                u.PasswordHash != null)), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_EmailAlreadyExists_ThrowsInvalidOperationException()
        {
            // Arrange
            string email = "johndoe@example.com";

            _mockUserRepo.Setup(repo => repo.EmailExistsAsync(email)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.RegisterAsync("John Doe", email, "password", "question", "answer"));
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsUser()
        {
            // Arrange
            string email = "johndoe@example.com";
            string password = "securepassword";
            string hashedPassword = _service.GetType()
                .GetMethod("HashString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(_service, new object[] { password }) as string;

            var user = new User { Id = 1, Email = email, PasswordHash = hashedPassword };

            _mockUserRepo.Setup(repo => repo.AuthenticateAsync(email, hashedPassword)).ReturnsAsync(user);

            // Act
            var result = await _service.AuthenticateAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            string email = "johndoe@example.com";
            string password = "wrongpassword";

            _mockUserRepo.Setup(repo => repo.AuthenticateAsync(email, It.IsAny<string>())).ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.AuthenticateAsync(email, password));
        }

        [Fact]
        public async Task RecoverPasswordAsync_ValidData_UpdatesPassword()
        {
            // Arrange
            string email = "johndoe@example.com";
            string securityQuestion = "What is your pet's name?";
            string securityAnswer = "Fluffy";
            string newPassword = "newsecurepassword";

            string hashedAnswer = _service.GetType()
                .GetMethod("HashString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(_service, new object[] { securityAnswer }) as string;

            var user = new User
            {
                Email = email,
                SecurityQuestion = securityQuestion,
                SecurityAnswerHash = hashedAnswer
            };

            _mockUserRepo.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act
            await _service.RecoverPasswordAsync(email, securityQuestion, securityAnswer, newPassword);

            // Assert
            _mockUserRepo.Verify(repo => repo.UpdateAsync(It.Is<User>(u =>
                u.Email == email &&
                u.PasswordHash != null)), Times.Once);
        }

        [Fact]
        public async Task RecoverPasswordAsync_InvalidSecurityData_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            string email = "johndoe@example.com";
            string securityQuestion = "What is your pet's name?";
            string securityAnswer = "WrongAnswer";

            var user = new User
            {
                Email = email,
                SecurityQuestion = securityQuestion,
                SecurityAnswerHash = "hashedAnswer"
            };

            _mockUserRepo.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.RecoverPasswordAsync(email, securityQuestion, securityAnswer, "newPassword"));
        }

        [Fact]
        public async Task GetSecurityQuestionAsync_ValidEmail_ReturnsSecurityQuestion()
        {
            // Arrange
            string email = "johndoe@example.com";
            string securityQuestion = "What is your pet's name?";

            var user = new User
            {
                Email = email,
                SecurityQuestion = securityQuestion
            };

            _mockUserRepo.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _service.GetSecurityQuestionAsync(email);

            // Assert
            Assert.Equal(securityQuestion, result);
        }

        [Fact]
        public async Task GetSecurityQuestionAsync_InvalidEmail_ReturnsNull()
        {
            // Arrange
            string email = "invalid@example.com";

            _mockUserRepo.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync((User?)null);

            // Act
            var result = await _service.GetSecurityQuestionAsync(email);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsUser()
        {
            // Arrange
            int userId = 1;
            var user = new User { Id = userId, Name = "John Doe" };

            _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _service.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            int userId = 1;

            _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(userId));
        }
    }
}
