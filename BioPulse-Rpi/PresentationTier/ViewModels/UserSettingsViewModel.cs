using LogicLayer.Services;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using System;

namespace PresentationTier.ViewModels
{
    public class UserSettingsViewModel : ReactiveObject
    {
        private readonly UserManagementService _userService;

        private string _newEmail;
        private string _repeatEmail;
        private string _oldPassword;
        private string _newPassword;
        private string _repeatPassword;
        private string _newSecurityQuestion;
        private string _newSecurityAnswer;
        private string _errorMessage;

        public string NewEmail
        {
            get => _newEmail;
            set => this.RaiseAndSetIfChanged(ref _newEmail, value);
        }

        public string RepeatEmail
        {
            get => _repeatEmail;
            set => this.RaiseAndSetIfChanged(ref _repeatEmail, value);
        }

        public string OldPassword
        {
            get => _oldPassword;
            set => this.RaiseAndSetIfChanged(ref _oldPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => this.RaiseAndSetIfChanged(ref _newPassword, value);
        }

        public string RepeatPassword
        {
            get => _repeatPassword;
            set => this.RaiseAndSetIfChanged(ref _repeatPassword, value);
        }

        public string NewSecurityQuestion
        {
            get => _newSecurityQuestion;
            set => this.RaiseAndSetIfChanged(ref _newSecurityQuestion, value);
        }

        public string NewSecurityAnswer
        {
            get => _newSecurityAnswer;
            set => this.RaiseAndSetIfChanged(ref _newSecurityAnswer, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public UserSettingsViewModel(UserManagementService userService)
        {
            _userService = userService;

            SaveCommand = ReactiveCommand.CreateFromTask(SaveSettingsAsync);
        }

        private async Task SaveSettingsAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(NewEmail) && NewEmail != RepeatEmail)
                {
                    ErrorMessage = "Emails do not match.";
                    return;
                }

                if (!string.IsNullOrEmpty(NewPassword) && NewPassword != RepeatPassword)
                {
                    ErrorMessage = "Passwords do not match.";
                    return;
                }

                await _userService.UpdateUserSettingsAsync(
                    userId: 1, // Replace with the actual logged-in user ID
                    newEmail: NewEmail,
                    newPassword: !string.IsNullOrEmpty(NewPassword) ? NewPassword : null,
                    newPhoneNumber: null, // Add phone number field if needed
                    newSecurityQuestion: NewSecurityQuestion,
                    newSecurityAnswer: NewSecurityAnswer
                );

                ErrorMessage = "Settings updated successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}