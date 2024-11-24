using System;
using System.Reactive;
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

        // Actions for navigation
        public Action NavigateToMain { get; set; }
        public Action NavigateToRegister { get; set; }

        public LoginViewModel()
        {
            // Commands
            LoginCommand = ReactiveCommand.Create(PerformLogin);
            NavigateToRegisterCommand = ReactiveCommand.Create(() => NavigateToRegister?.Invoke());
        }

        private void PerformLogin()
        {
            // Login logic here
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Email or Password cannot be empty.";
                return;
            }

            // Placeholder success logic
            ErrorMessage = string.Empty;
            NavigateToMain?.Invoke();
        }
    }
}
