using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
    public class UserRepoTest : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly AppDbContext _dbContext;
        private readonly UserRepo _userRepo;

        public UserRepoTest()
        {
            // Set up DbContext with in-memory database for testing
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("UserTestDatabase"));

            // Register repository using AppDbContext
            serviceCollection.AddScoped<UserRepo>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            _userRepo = _serviceProvider.GetRequiredService<UserRepo>();
        }

        [Fact]
        public async Task CanAddUserToDatabase()
        {
            // Arrange
            var user = new User
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                PasswordHash = "password123",
                SecurityQuestion = "What is your pet's name?",
                SecurityAnswerHash = "Fluffy",
                PhoneNumber = "123456789"
            };

            // Act
            await _userRepo.AddAsync(user);
            var result = await _userRepo.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john.doe@example.com", result.Email);
        }

        [Fact]
        public async Task CanUpdateUser()
        {
            // Arrange
            var user = new User
            {
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                PasswordHash = "securepassword",
                SecurityQuestion = "What is your mother's maiden name?",
                SecurityAnswerHash = "Johnson",
                PhoneNumber = "987654321"
            };
            await _userRepo.AddAsync(user);

            // Act
            user.PhoneNumber = "111222333";
            await _userRepo.UpdateAsync(user);
            var result = await _userRepo.GetByIdAsync(user.Id);

            // Assert
            Assert.Equal("111222333", result.PhoneNumber);
        }

        [Fact]
        public async Task CanDeleteUser()
        {
            // Arrange
            var user = new User
            {
                Name = "Mark Lee",
                Email = "mark.lee@example.com",
                PasswordHash = "pass456",
                SecurityQuestion = "What was the name of your first school?",
                SecurityAnswerHash = "Greenwood",
                PhoneNumber = "555666777"
            };
            await _userRepo.AddAsync(user);

            // Act
            await _userRepo.DeleteAsync(user.Id);
            var result = await _userRepo.GetByIdAsync(user.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CanRetrieveUserByEmail()
        {
            // Arrange
            var user = new User
            {
                Name = "Alice Johnson",
                Email = "alice.johnson@example.com",
                PasswordHash = "mypassword",
                SecurityQuestion = "Where were you born?",
                SecurityAnswerHash = "New York",
                PhoneNumber = "123123123"
            };
            await _userRepo.AddAsync(user);

            // Act
            var result = await _userRepo.GetByEmailAsync("alice.johnson@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice Johnson", result.Name);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _serviceProvider.Dispose();
        }
    }
}
