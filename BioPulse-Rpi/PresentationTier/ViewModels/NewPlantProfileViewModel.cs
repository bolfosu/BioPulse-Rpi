using ReactiveUI;
using System.Reactive;

namespace PresentationTier.ViewModels
{
    public class NewPlantProfileViewModel : ReactiveObject
    {
        private string _profileName;
        private string _phMin;
        private string _phMax;
        private string _tempMin;
        private string _tempMax;
        private string _ecMin;
        private string _ecMax;
        private string _lightMin;
        private string _lightMax;

        public NewPlantProfileViewModel()
        {
            // Initialize commands
            SaveProfileCommand = ReactiveCommand.Create(SaveProfile);
            ActivateProfileCommand = ReactiveCommand.Create(ActivateProfile);

            // Initialize properties with default or empty values
            ProfileName = string.Empty;
            PhMin = string.Empty;
            PhMax = string.Empty;
            TempMin = string.Empty;
            TempMax = string.Empty;
            EcMin = string.Empty;
            EcMax = string.Empty;
            LightMin = string.Empty;
            LightMax = string.Empty;
        }

        // Profile name
        public string ProfileName
        {
            get => _profileName;
            set => this.RaiseAndSetIfChanged(ref _profileName, value);
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

        // Commands for save and activate actions
        public ReactiveCommand<Unit, Unit> SaveProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> ActivateProfileCommand { get; }

        // Save the new profile
        private void SaveProfile()
        {
            // Logic to save the profile (e.g., database or collection)
            System.Console.WriteLine("Saving profile:");
            System.Console.WriteLine($"Name: {ProfileName}");
            System.Console.WriteLine($"pH: {PhMin} - {PhMax}");
            System.Console.WriteLine($"Temperature: {TempMin} - {TempMax}");
            System.Console.WriteLine($"EC: {EcMin} - {EcMax}");
            System.Console.WriteLine($"Light: {LightMin} - {LightMax}");

            // Add additional logic to persist this data
        }

        // Activate the new profile
        private void ActivateProfile()
        {
            // Logic to activate the profile (e.g., notify other parts of the system)
            System.Console.WriteLine($"Activating profile: {ProfileName}");
        }
    }
}
