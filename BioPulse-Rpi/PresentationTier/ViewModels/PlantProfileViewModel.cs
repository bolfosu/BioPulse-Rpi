using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace PresentationTier.ViewModels
{
    public class PlantProfileViewModel : ReactiveObject
    {
        private string _selectedProfile;
        private string _phMin;
        private string _phMax;
        private string _tempMin;
        private string _tempMax;
        private string _ecMin;
        private string _ecMax;
        private string _lightMin;
        private string _lightMax;

        public PlantProfileViewModel()
        {
            // Initialize the list of plant profiles
            Profiles = new ObservableCollection<string> { "Cabbage", "Salad", "Cucumber", "Tomato" };

            // Initialize commands
            SelectProfileCommand = ReactiveCommand.Create<string>(SelectProfile);
            CreateNewProfileCommand = ReactiveCommand.Create(CreateNewProfile);
            SaveCommand = ReactiveCommand.Create(SaveProfile);
            ActivateCommand = ReactiveCommand.Create(ActivateProfile);
            NavigateToCreateProfileCommand = ReactiveCommand.Create(NavigateToCreateProfile);

            // Example defaults for sensor ranges
            PhMin = "5.5";
            PhMax = "6.5";
            TempMin = "18";
            TempMax = "24";
            EcMin = "1.5";
            EcMax = "2.5";
            LightMin = "400";
            LightMax = "800";
        }

        // List of profiles
        public ObservableCollection<string> Profiles { get; }

        // Selected profile
        public string SelectedProfile
        {
            get => _selectedProfile;
            set => this.RaiseAndSetIfChanged(ref _selectedProfile, value);
        }

        // Sensor thresholds
        public string PhMin { get => _phMin; set => this.RaiseAndSetIfChanged(ref _phMin, value); }
        public string PhMax { get => _phMax; set => this.RaiseAndSetIfChanged(ref _phMax, value); }
        public string TempMin { get => _tempMin; set => this.RaiseAndSetIfChanged(ref _tempMin, value); }
        public string TempMax { get => _tempMax; set => this.RaiseAndSetIfChanged(ref _tempMax, value); }
        public string EcMin { get => _ecMin; set => this.RaiseAndSetIfChanged(ref _ecMin, value); }
        public string EcMax { get => _ecMax; set => this.RaiseAndSetIfChanged(ref _ecMax, value); }
        public string LightMin { get => _lightMin; set => this.RaiseAndSetIfChanged(ref _lightMin, value); }
        public string LightMax { get => _lightMax; set => this.RaiseAndSetIfChanged(ref _lightMax, value); }

        // Commands
        public ReactiveCommand<string, Unit> SelectProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateNewProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> ActivateCommand { get; }
        public ReactiveCommand<Unit, Unit> NavigateToCreateProfileCommand { get; } // New Command

        // Method to handle selecting a profile
        private void SelectProfile(string profile)
        {
            SelectedProfile = profile;
            // Logic to load the selected profile's data (populate min/max values)
            // Example: You could fetch data from a database here
            PhMin = "5.5";
            PhMax = "6.5";
            TempMin = "18";
            TempMax = "24";
            EcMin = "1.5";
            EcMax = "2.5";
            LightMin = "400";
            LightMax = "800";
        }

        // Method to create a new profile
        private void CreateNewProfile()
        {
            var newProfile = "New Profile"; // Placeholder name
            Profiles.Add(newProfile);
            SelectedProfile = newProfile;

            // Clear fields for the new profile
            PhMin = string.Empty;
            PhMax = string.Empty;
            TempMin = string.Empty;
            TempMax = string.Empty;
            EcMin = string.Empty;
            EcMax = string.Empty;
            LightMin = string.Empty;
            LightMax = string.Empty;
        }

        // Save profile changes
        private void SaveProfile()
        {
            // Logic to save the current profile's data
            // Example: Save to a database or a file
            System.Console.WriteLine($"Profile '{SelectedProfile}' saved with the following values:");
            System.Console.WriteLine($"pH: {PhMin} - {PhMax}");
            System.Console.WriteLine($"Temperature: {TempMin} - {TempMax}");
            System.Console.WriteLine($"EC: {EcMin} - {EcMax}");
            System.Console.WriteLine($"Light: {LightMin} - {LightMax}");
        }

        // Activate the current profile
        private void ActivateProfile()
        {
            // Logic to activate the current profile
            // Example: Send activation command to the system
            System.Console.WriteLine($"Profile '{SelectedProfile}' activated.");
        }

        // Navigate to the NewProfileView
        private void NavigateToCreateProfile()
        {
            // Logic to navigate to the NewProfileView
            System.Console.WriteLine("Navigating to the New Profile Creation Page...");
        }
    }
}
