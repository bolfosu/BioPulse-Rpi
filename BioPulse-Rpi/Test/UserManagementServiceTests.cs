using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserManagementServiceTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task RegisterUser_ShouldAddUser()
    {
        using var context = CreateInMemoryContext();
        var userRepository = new UserRepo(context);
        var userService = new UserManagementService(userRepository);

        await userService.RegisterUserAsync(
            name: "Boris The Blade",
            email: "boris@theblade.com",
            password: "password123",
            securityQuestion: "What is your pet's name?",
            securityAnswer: "Rex"
        );

        var user = await userRepository.GetByEmailAsync("boris@theblade.com");
        Assert.NotNull(user);
        Assert.Equal("Boris The Blade", user.Name);
    }

    [Fact]
    public async Task AuthenticateUser_ShouldReturnUser_WhenCredentialsAreValid()
    {
        using var context = CreateInMemoryContext();
        var userRepository = new UserRepo(context);
        var userService = new UserManagementService(userRepository);

        await userService.RegisterUserAsync(
            name: "John Doe",
            email: "john.doe@example.com",
            password: "password123",
            securityQuestion: "What is your pet's name?",
            securityAnswer: "Fluffy"
        );

        var user = await userService.AuthenticateUserAsync("john.doe@example.com", "password123");
        Assert.NotNull(user);
        Assert.Equal("John Doe", user.Name);
    }
}
