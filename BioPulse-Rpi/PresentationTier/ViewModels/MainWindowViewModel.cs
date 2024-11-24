using ReactiveUI;

namespace PresentationTier.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ReactiveObject _currentView;

        public MainWindowViewModel(LoginViewModel loginViewModel, RegistrationViewModel registrationViewModel)
        {
            LoginViewModel = loginViewModel;
            RegistrationViewModel = registrationViewModel;

            LoginViewModel.NavigateToMain = NavigateToMain;
            LoginViewModel.NavigateToRegister = NavigateToRegistration;
            RegistrationViewModel.NavigateToLogin = NavigateToLogin;

            // Set initial view
            CurrentView = LoginViewModel;
        }

        public LoginViewModel LoginViewModel { get; }
        public RegistrationViewModel RegistrationViewModel { get; }

        public ReactiveObject CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        private void NavigateToMain()
        {
            // Logic for navigating to Main View can be added here
        }

        private void NavigateToRegistration()
        {
            CurrentView = RegistrationViewModel;
        }

        private void NavigateToLogin()
        {
            CurrentView = LoginViewModel;
        }
    }
}
