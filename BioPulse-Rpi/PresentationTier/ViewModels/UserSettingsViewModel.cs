using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using LogicLayer.Services;
using DataAccessLayer.Models;
using System;

namespace PresentationTier.ViewModels
{
    public class UserSettingsViewModel : ReactiveObject
    {
        private readonly UserManagementService _userService;

        private string _email;
        private string _password;
        private string _securityQuestion;
        private string _securityAnswer;
        private bool _isWaterLevelLowNotificationEnabled;
        private bool _isSensorNotChangingNotificationEnabled;
        private bool _isSensorOffNotificationEnabled;

        public UserSettingsViewModel(UserManagementService userService)
        {
            _userService = userService;

            SaveSettingsCommand = ReactiveCommand.CreateFromTask(SaveSettingsAsync);
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

        public bool IsWaterLevelLowNotificationEnabled
        {
            get => _isWaterLevelLowNotificationEnabled;
            set => this.RaiseAndSetIfChanged(ref _isWaterLevelLowNotificationEnabled, value);
        }

        public bool IsSensorNotChangingNotificationEnabled
        {
            get => _isSensorNotChangingNotificationEnabled;
            set => this.RaiseAndSetIfChanged(ref _isSensorNotChangingNotificationEnabled, value);
        }

        public bool IsSensorOffNotificationEnabled
        {
            get => _isSensorOffNotificationEnabled;
            set => this.RaiseAndSetIfChanged(ref _isSensorOffNotificationEnabled, value);
        }

        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }

        private async Task SaveSettingsAsync()
        {
            var user = new User
            {
                Email = Email,
                PasswordHash = HashPassword(Password), // Ensure hashing
                SecurityQuestion = SecurityQuestion,
                SecurityAnswerHash = HashPassword(SecurityAnswer),
                IsWaterLevelLowNotificationEnabled = IsWaterLevelLowNotificationEnabled,
                IsSensorNotChangingNotificationEnabled = IsSensorNotChangingNotificationEnabled,
                IsSensorOffNotificationEnabled = IsSensorOffNotificationEnabled
            };

            await _userService.UpdateUserSettingsAsync(user);
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }
    }
}
