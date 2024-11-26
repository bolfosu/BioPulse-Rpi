using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UserManagementServiceTests
    {
        private readonly IServiceProvider _serviceProvider;

        public UserManagementServiceTests()
        {
            // Set up DbContext with in-memory database for testing
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("UserManagementTestDatabase"));

            // Register repository and service
            serviceCollection.AddScoped<UserRepo>();
            serviceCollection.AddScoped<IRepository<User>, UserRepo>();
            serviceCollection.AddScoped<UserManagementService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private AppDbContext GetDbContext()
        {
            return _serviceProvider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task CanRegisterNewUser()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<UserManagementService>();

            var name = "Test User";
            var email = "test@example.com";
            var password = "password";
            var securityQuestion = "What is your pet's name?";
            var securityAnswer = "Fluffy";

            // Act
            var result = await service.RegisterAsync(name, email, password, securityQuestion, securityAnswer);

            // Assert
            Assert.True(result);

            var dbContext = GetDbContext();
            var addedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            Assert.NotNull(addedUser);
            Assert.Equal(name, addedUser.Name);
            Assert.Equal(email, addedUser.Email);
        }

        [Fact]
        public async Task CannotRegisterUserWithExistingEmail()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<UserManagementService>();

            var email = "existinguser@example.com";
            var dbContext = GetDbContext();

            // Add an existing user to the database
            await dbContext.Users.AddAsync(new User
            {
                Name = "Existing User",
                Email = email,
                PasswordHash = "hashedpassword",
                SecurityQuestion = "What is your pet's name?",
                SecurityAnswerHash = "hashedanswer"
            });
            await dbContext.SaveChangesAsync();

            // Act
            var result = await service.RegisterAsync(
                "New User",
                email,
                "password",
                "What is your favorite color?",
                "Blue");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CanAuthenticateUserWithValidCredentials()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<UserManagementService>();
            var email = "authuser@example.com";
            var password = "password";

            var passwordHash = HashPassword(password);

            var dbContext = GetDbContext();
            await dbContext.Users.AddAsync(new User
            {
                Name = "Auth User",
                Email = email,
                PasswordHash = passwordHash,
                SecurityQuestion = "What is your pet's name?",
                SecurityAnswerHash = "hashedanswer"
            });
            await dbContext.SaveChangesAsync();

            // Act
            var user = await service.AuthenticateAsync(email, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public async Task CannotAuthenticateUserWithInvalidCredentials()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<UserManagementService>();
            var email = "invaliduser@example.com";
            var password = "wrongpassword";

            var dbContext = GetDbContext();
            await dbContext.Users.AddAsync(new User
            {
                Name = "Invalid User",
                Email = email,
                PasswordHash = HashPassword("correctpassword"),
                SecurityQuestion = "What is your pet's name?",
                SecurityAnswerHash = "hashedanswer"
            });
            await dbContext.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                service.AuthenticateAsync(email, password));
        }

        private string HashPassword(string input)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
