using System;
using System.Reactive;
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

        // Action for navigation
        public Action NavigateToLogin { get; set; }

        public RegistrationViewModel()
        {
            // Commands
            RegisterCommand = ReactiveCommand.Create(PerformRegistration);
            NavigateToLoginCommand = ReactiveCommand.Create(() => NavigateToLogin?.Invoke());
        }

        private void PerformRegistration()
        {
            // Placeholder registration logic
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            // Simulate successful registration
            ErrorMessage = string.Empty;
            NavigateToLogin?.Invoke();
        }
    }
}
