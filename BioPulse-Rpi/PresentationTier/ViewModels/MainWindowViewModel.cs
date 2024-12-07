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
            LoginViewModel = loginViewModel;
            RegistrationViewModel = registrationViewModel;
            PasswordRecoveryViewModel = passwordRecoveryViewModel;
            DashboardViewModel = dashboardViewModel;
            PlantProfileViewModel = plantProfileViewModel;
            DeviceSettingsViewModel = deviceSettingsViewModel;
            UserSettingsViewModel = userSettingsViewModel;

            CurrentView = LoginViewModel;

            // Commands to navigate between views
            NavigateToDashboardCommand = ReactiveCommand.Create(() =>
            {
                CurrentView = DashboardViewModel;
                return CurrentView;
            });

            NavigateToPlantProfileCommand = ReactiveCommand.Create(() =>
            {
                CurrentView = PlantProfileViewModel;
                return CurrentView;
            });

            NavigateToDeviceSettingsCommand = ReactiveCommand.Create(() =>
            {
                CurrentView = DeviceSettingsViewModel;
                return CurrentView;
            });

            NavigateToUserSettingsCommand = ReactiveCommand.Create(() =>
            {
                CurrentView = UserSettingsViewModel;
                return CurrentView;
            });

            // Login navigation
            LoginViewModel.OnLoginSuccess = HandleLoginSuccess;
            LoginViewModel.NavigateToRegister = () => CurrentView = RegistrationViewModel;
            LoginViewModel.NavigateToPasswordRecovery = () => CurrentView = PasswordRecoveryViewModel;

           
        }

        public ReactiveCommand<Unit, ReactiveObject> NavigateToDashboardCommand { get; }
        public ReactiveCommand<Unit, ReactiveObject> NavigateToPlantProfileCommand { get; }
        public ReactiveCommand<Unit, ReactiveObject> NavigateToDeviceSettingsCommand { get; }
        public ReactiveCommand<Unit, ReactiveObject> NavigateToUserSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToCreateProfileViewCommand { get; } // New Command

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

        private void HandleLoginSuccess()
        {
            IsAuthenticated = true;
            CurrentView = DashboardViewModel;
        }

      
    }
}
