using System;
using System.Reactive;
using System.Threading.Tasks;
using LogicLayer.Services;
using ReactiveUI;

namespace PresentationTier.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private string _email;
        private string _password;
        private string _errorMessage;

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

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToRegisterCommand { get; }
        public ReactiveCommand<Unit, Unit> GuestModeCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToPasswordRecoveryCommand { get; }

        public Action NavigateToRegister { get; set; }
        public Action EnterGuestMode { get; set; } // Action for guest mode
        public Action NavigateToPasswordRecovery { get; set; } // Action for password recovery

        // Callback for successful login
        public Action OnLoginSuccess { get; set; }

        private readonly UserManagementService _userService;

        public LoginViewModel(UserManagementService userService)
        {
            _userService = userService;

            LoginCommand = ReactiveCommand.CreateFromTask(PerformLoginAsync);
            NavigateToRegisterCommand = ReactiveCommand.Create(() => NavigateToRegister?.Invoke());
            GuestModeCommand = ReactiveCommand.Create(() => EnterGuestMode?.Invoke());
            NavigateToPasswordRecoveryCommand = ReactiveCommand.Create(() => NavigateToPasswordRecovery?.Invoke());
        }

        public void ResetFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }

        private async Task PerformLoginAsync()
        {
            Console.WriteLine("Login Command Triggered"); // Debug log

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Email or Password cannot be empty.";
                return;
            }

            try
            {
                var user = await _userService.AuthenticateAsync(Email, Password);
                Console.WriteLine($"User {user.Name} logged in successfully!");
                ErrorMessage = string.Empty;

                // Notify MainWindowViewModel about successful login
                OnLoginSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }
    }
}
