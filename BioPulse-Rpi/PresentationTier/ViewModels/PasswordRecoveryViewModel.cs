using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using LogicLayer.Services;

namespace PresentationTier.ViewModels
{
    public class PasswordRecoveryViewModel : ReactiveObject
    {
        private string _email;
        private string _securityQuestion;
        private string _securityAnswer;
        private string _newPassword;
        private string _confirmPassword;
        private string _errorMessage;

        public string Email
        {
            get => _email;
            set
            {
                this.RaiseAndSetIfChanged(ref _email, value);
                if (!string.IsNullOrEmpty(value))
                {
                    FetchSecurityQuestionAsync(); // Fetch the security question dynamically
                }
                else
                {
                    SecurityQuestion = string.Empty;
                }
            }
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

        public string NewPassword
        {
            get => _newPassword;
            set => this.RaiseAndSetIfChanged(ref _newPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => this.RaiseAndSetIfChanged(ref _confirmPassword, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> RecoverPasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToLoginCommand { get; }

        public Action NavigateToLogin { get; set; }

        private readonly UserManagementService _userService;

        public PasswordRecoveryViewModel(UserManagementService userService)
        {
            _userService = userService;

            RecoverPasswordCommand = ReactiveCommand.CreateFromTask(PerformPasswordRecoveryAsync);
            NavigateToLoginCommand = ReactiveCommand.Create(() => NavigateToLogin?.Invoke());
        }

        /// <summary>
        /// Resets all fields in the ViewModel and notifies the UI to refresh.
        /// </summary>
        public void ResetFields()
        {
            _email = string.Empty;
            this.RaisePropertyChanged(nameof(Email));

            _securityQuestion = string.Empty;
            this.RaisePropertyChanged(nameof(SecurityQuestion));

            _securityAnswer = string.Empty;
            this.RaisePropertyChanged(nameof(SecurityAnswer));

            _newPassword = string.Empty;
            this.RaisePropertyChanged(nameof(NewPassword));

            _confirmPassword = string.Empty;
            this.RaisePropertyChanged(nameof(ConfirmPassword));

            _errorMessage = string.Empty;
            this.RaisePropertyChanged(nameof(ErrorMessage));

            Console.WriteLine("ResetFields executed. All fields cleared.");
        }

        /// <summary>
        /// Fetch the security question associated with the email.
        /// </summary>
        private async Task FetchSecurityQuestionAsync()
        {
            try
            {
                var question = await _userService.GetSecurityQuestionAsync(Email);
                SecurityQuestion = question ?? "Email not found.";
            }
            catch (Exception ex)
            {
                SecurityQuestion = "Error retrieving security question.";
                Console.WriteLine($"Error fetching security question: {ex.Message}");
            }
        }

        /// <summary>
        /// Performs password recovery.
        /// </summary>
        private async Task PerformPasswordRecoveryAsync()
        {
            if (string.IsNullOrEmpty(SecurityAnswer) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            try
            {
                await _userService.RecoverPasswordAsync(Email, SecurityQuestion, SecurityAnswer, NewPassword);
                ErrorMessage = string.Empty;
                NavigateToLogin?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
