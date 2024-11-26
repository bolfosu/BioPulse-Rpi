using ReactiveUI;
using System;
using System.Reactive;

namespace PresentationTier.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ReactiveObject _currentView;

        public MainWindowViewModel(LoginViewModel loginViewModel, RegistrationViewModel registrationViewModel)
        {
            LoginViewModel = loginViewModel;
            RegistrationViewModel = registrationViewModel;
            

            // Default to LoginView
            CurrentView = LoginViewModel;

            // Navigation Commands
            NavigateToLoginCommand = ReactiveCommand.Create(() => { CurrentView = LoginViewModel; return Unit.Default; });
            NavigateToRegistrationCommand = ReactiveCommand.Create(() => { CurrentView = RegistrationViewModel; return Unit.Default; });
            
        }

        public LoginViewModel LoginViewModel { get; }
        public RegistrationViewModel RegistrationViewModel { get; }

        public ReactiveObject CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public ReactiveCommand<Unit, Unit> NavigateToLoginCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToRegistrationCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToSimplePageCommand { get; }
    }
}