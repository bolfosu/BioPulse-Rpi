using ReactiveUI;
using System.Reactive;

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
            // Assign view models
            LoginViewModel = loginViewModel;
            RegistrationViewModel = registrationViewModel;
            PasswordRecoveryViewModel = passwordRecoveryViewModel;
            DashboardViewModel = dashboardViewModel;
            PlantProfileViewModel = plantProfileViewModel;
            DeviceSettingsViewModel = deviceSettingsViewModel;
            UserSettingsViewModel = userSettingsViewModel;

            // Default view
            CurrentView = LoginViewModel;

            // Bind navigation actions
            LoginViewModel.NavigateToRegister = () => CurrentView = RegistrationViewModel;
            LoginViewModel.NavigateToPasswordRecovery = () => CurrentView = PasswordRecoveryViewModel;
            LoginViewModel.OnLoginSuccess = HandleLoginSuccess;

            RegistrationViewModel.NavigateToLogin = () => CurrentView = LoginViewModel;
            PasswordRecoveryViewModel.NavigateToLogin = () => CurrentView = LoginViewModel;

            // Initialize commands
            NavigateToDashboardCommand = ReactiveCommand.Create(NavigateToDashboard);
            NavigateToPlantProfileCommand = ReactiveCommand.Create(NavigateToPlantProfile);
            NavigateToDeviceSettingsCommand = ReactiveCommand.Create(NavigateToDeviceSettings);
            NavigateToUserSettingsCommand = ReactiveCommand.Create(NavigateToUserSettings);
        }

        public ReactiveCommand<Unit, Unit> NavigateToDashboardCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToPlantProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToDeviceSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToUserSettingsCommand { get; }

        public LoginViewModel LoginViewModel { get; }
        public RegistrationViewModel RegistrationViewModel { get; }
        public PasswordRecoveryViewModel PasswordRecoveryViewModel { get; }
        public DashboardViewModel DashboardViewModel { get; }
        public PlantProfileViewModel PlantProfileViewModel { get; }
        public DeviceSettingsViewModel DeviceSettingsViewModel { get; }
        public UserSettingsViewModel UserSettingsViewModel { get; }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => this.RaiseAndSetIfChanged(ref _isAuthenticated, value);
        }

        public ReactiveObject CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        private void NavigateToDashboard()
        {
            CurrentView = DashboardViewModel;
        }

        private void NavigateToPlantProfile()
        {
            CurrentView = PlantProfileViewModel;
        }

        private void NavigateToDeviceSettings()
        {
            CurrentView = DeviceSettingsViewModel;
        }

        private void NavigateToUserSettings()
        {
            CurrentView = UserSettingsViewModel;
        }

        private void HandleLoginSuccess()
        {
            IsAuthenticated = true; // Mark user as logged in
            CurrentView = DashboardViewModel; // Default view after login
        }
    }
}
