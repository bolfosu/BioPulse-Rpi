using ReactiveUI;
using System;

namespace PresentationTier.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ReactiveObject _currentView;

        public MainWindowViewModel(LoginViewModel loginViewModel, RegistrationViewModel registrationViewModel, PasswordRecoveryViewModel passwordRecoveryViewModel)
        {
            LoginViewModel = loginViewModel;
            RegistrationViewModel = registrationViewModel;
            PasswordRecoveryViewModel = passwordRecoveryViewModel;

            // Set default view to LoginView
            CurrentView = LoginViewModel;

            // Bind navigation actions
            LoginViewModel.NavigateToRegister = () => NavigateToRegistrationView();
            LoginViewModel.NavigateToPasswordRecovery = () => NavigateToPasswordRecoveryView();
            RegistrationViewModel.NavigateToLogin = () => NavigateToLoginView();
            PasswordRecoveryViewModel.NavigateToLogin = () => NavigateToLoginView();
        }

        public LoginViewModel LoginViewModel { get; }
        public RegistrationViewModel RegistrationViewModel { get; }
        public PasswordRecoveryViewModel PasswordRecoveryViewModel { get; }

        public ReactiveObject CurrentView
        {
            get => _currentView;
            set
            {
                this.RaiseAndSetIfChanged(ref _currentView, value);

                // Reset fields when the view changes
                ResetFieldsForView(value);
            }
        }

        // Navigation Methods
        public void NavigateToLoginView()
        {
            CurrentView = LoginViewModel;
        }

        public void NavigateToRegistrationView()
        {
            CurrentView = RegistrationViewModel;
        }

        public void NavigateToPasswordRecoveryView()
        {
            CurrentView = PasswordRecoveryViewModel;
        }

        // Reset fields based on the active view
        private void ResetFieldsForView(ReactiveObject view)
        {
            if (view == LoginViewModel)
            {
                LoginViewModel.ResetFields();
            }
            else if (view == RegistrationViewModel)
            {
                RegistrationViewModel.ResetFields();
            }
            else if (view == PasswordRecoveryViewModel)
            {
                PasswordRecoveryViewModel.ResetFields();
            }
        }
    }
}
