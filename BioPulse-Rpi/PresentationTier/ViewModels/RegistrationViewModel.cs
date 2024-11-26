using System;
using System.Reactive;
using System.Threading.Tasks;
using LogicLayer.Services;
using ReactiveUI;

namespace PresentationTier.ViewModels
{
    public class RegistrationViewModel : ReactiveObject
    {
        private string _name;
        private string _email;
        private string _password;
        private string _securityQuestion;
        private string _securityAnswer;
        private string _errorMessage;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string SecurityQuestion
        {
            get => _securityQuestion;
            set => this.RaiseAndSetIfChanged(ref _securityQuestion, value);
        }

        public string SecurityAnswer
        {
            get => _securityAnswer;
            set => this.RaiseAndSetIfChanged(ref _securityAnswer, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToLoginCommand { get; }

        public Action NavigateToLogin { get; set; }

        private readonly UserManagementService _userService;

        public RegistrationViewModel(UserManagementService userService)
        {
            _userService = userService;

            RegisterCommand = ReactiveCommand.CreateFromTask(PerformRegistrationAsync);
            NavigateToLoginCommand = ReactiveCommand.Create(() => NavigateToLogin?.Invoke());
        }

        private async Task PerformRegistrationAsync()
        {
            Console.WriteLine("Register Command Triggered"); // Debug log

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            try
            {
                await _userService.RegisterAsync(Name, Email, Password, SecurityQuestion, SecurityAnswer);
                Console.WriteLine($"User {Name} registered successfully!");
                ErrorMessage = string.Empty;
                NavigateToLogin?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Console.WriteLine($"Registration failed: {ex.Message}");
            }
        }
    }
}
