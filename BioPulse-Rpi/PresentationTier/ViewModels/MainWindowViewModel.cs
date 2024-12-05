using ReactiveUI;

namespace PresentationTier.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ReactiveObject _currentView;
        private bool _isAuthenticated;

        public MainWindowViewModel(
            LoginViewModel loginViewModel,
            RegistrationViewModel registrationViewModel,
            PasswordRecoveryViewModel passwordRecoveryViewModel,
            DashboardViewModel dashboardViewModel,
            PlantProfileViewModel plantProfileViewModel,
            DeviceSettingsViewModel deviceSettingsViewModel,
            UserSettingsViewModel userSettingsViewModel)
        {
            // Assign view models to properties
            LoginViewModel = loginViewModel;
            RegistrationViewModel = registrationViewModel;
            PasswordRecoveryViewModel = passwordRecoveryViewModel;
            DashboardViewModel = dashboardViewModel;
            PlantProfileViewModel = plantProfileViewModel;
            DeviceSettingsViewModel = deviceSettingsViewModel;
            UserSettingsViewModel = userSettingsViewModel;

            // Default view is the login page
            CurrentView = LoginViewModel;

            // Hook up navigation actions in LoginViewModel
            LoginViewModel.NavigateToRegister = () => CurrentView = RegistrationViewModel;
            LoginViewModel.NavigateToPasswordRecovery = () => CurrentView = PasswordRecoveryViewModel;
            LoginViewModel.OnLoginSuccess = HandleLoginSuccess;

            // Hook up navigation actions in RegistrationViewModel and PasswordRecoveryViewModel
            RegistrationViewModel.NavigateToLogin = () => CurrentView = LoginViewModel;
            PasswordRecoveryViewModel.NavigateToLogin = () => CurrentView = LoginViewModel;
        }

        // Properties for view models
        public LoginViewModel LoginViewModel { get; }
        public RegistrationViewModel RegistrationViewModel { get; }
        public PasswordRecoveryViewModel PasswordRecoveryViewModel { get; }
        public DashboardViewModel DashboardViewModel { get; }
        public PlantProfileViewModel PlantProfileViewModel { get; }
        public DeviceSettingsViewModel DeviceSettingsViewModel { get; }
        public UserSettingsViewModel UserSettingsViewModel { get; }

        // Property for determining if the user is authenticated
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => this.RaiseAndSetIfChanged(ref _isAuthenticated, value);
        }

        // CurrentView determines which view is displayed
        public ReactiveObject CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        // Navigation methods for the post-login UI
        public void NavigateToDashboardView() => CurrentView = DashboardViewModel;
        public void NavigateToPlantProfileView() => CurrentView = PlantProfileViewModel;
        public void NavigateToDeviceSettingsView() => CurrentView = DeviceSettingsViewModel;
        public void NavigateToUserSettingsView() => CurrentView = UserSettingsViewModel;

        // Handle login success to switch to the authenticated UI
        private void HandleLoginSuccess()
        {
            IsAuthenticated = true; // Mark user as authenticated
            CurrentView = DashboardViewModel; // Default view after login
        }
    }
}
