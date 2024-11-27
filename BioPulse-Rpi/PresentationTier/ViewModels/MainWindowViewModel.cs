using ReactiveUI;
using System;

namespace PresentationTier.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ReactiveObject _currentView;

        public MainWindowViewModel(LoginViewModel loginViewModel, RegistrationViewModel registrationViewModel)
        {
            LoginViewModel = loginViewModel;
            RegistrationViewModel = registrationViewModel;

            // Set navigation actions
            LoginViewModel.NavigateToRegister = () => CurrentView = RegistrationViewModel;
            RegistrationViewModel.NavigateToLogin = () => CurrentView = LoginViewModel;

            // Default view
            CurrentView = LoginViewModel;
        }

        public LoginViewModel LoginViewModel { get; }
        public RegistrationViewModel RegistrationViewModel { get; }

        public ReactiveObject CurrentView
        {
            get => _currentView;
            set
            {
                Console.WriteLine($"Navigating to: {value.GetType().Name}");
                this.RaiseAndSetIfChanged(ref _currentView, value);
            }
        }

        // Methods to navigate between views
        public void NavigateToLoginView()
        {
            CurrentView = LoginViewModel;
        }

        public void NavigateToRegistrationView()
        {
            CurrentView = RegistrationViewModel;
        }
    }
}
