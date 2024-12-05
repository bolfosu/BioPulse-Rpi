using DataAccessLayer.Models;
using LogicLayer.Services;
using ReactiveUI;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace PresentationTier.ViewModels
{
    public class PlantProfileViewModel : ReactiveObject
    {
        private readonly PlantProfileService _plantProfileService;
        private PlantProfile _selectedProfile;

        public PlantProfileViewModel(PlantProfileService plantProfileService)
        {
            _plantProfileService = plantProfileService;

            Profiles = new ObservableCollection<PlantProfile>();

            // Commands
            LoadProfilesCommand = ReactiveCommand.CreateFromTask(LoadProfilesAsync);
            SaveProfileCommand = ReactiveCommand.CreateFromTask(SaveProfileAsync);
            ActivateProfileCommand = ReactiveCommand.Create(ActivateProfile);
            CreateNewProfileCommand = ReactiveCommand.Create(CreateNewProfile);
            SelectPredefinedProfileCommand = ReactiveCommand.Create<string>(SelectPredefinedProfile);

            // Load profiles on initialization
            LoadProfilesCommand.Execute().Subscribe();
        }

        public ObservableCollection<PlantProfile> Profiles { get; }

        public PlantProfile SelectedProfile
        {
            get => _selectedProfile;
            set => this.RaiseAndSetIfChanged(ref _selectedProfile, value);
        }

        public string PhMin { get; set; }
        public string PhMax { get; set; }
        public string TempMin { get; set; }
        public string TempMax { get; set; }
        public string EcMin { get; set; }
        public string EcMax { get; set; }
        public string LightMin { get; set; }
        public string LightMax { get; set; }

        public ReactiveCommand<Unit, Unit> LoadProfilesCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> ActivateProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateNewProfileCommand { get; }
        public ReactiveCommand<string, Unit> SelectPredefinedProfileCommand { get; }

        private async Task LoadProfilesAsync()
        {
            try
            {
                var profiles = await _plantProfileService.GetAllPlantProfilesAsync();
                Profiles.Clear();
                foreach (var profile in profiles)
                {
                    Profiles.Add(profile);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error loading profiles: {ex.Message}");
            }
        }

        private async Task SaveProfileAsync()
        {
            if (SelectedProfile == null) return;

            SelectedProfile.PhMin = double.Parse(PhMin);
            SelectedProfile.PhMax = double.Parse(PhMax);
            SelectedProfile.TemperatureMin = double.Parse(TempMin);
            SelectedProfile.TemperatureMax = double.Parse(TempMax);
            SelectedProfile.EcMin = double.Parse(EcMin);
            SelectedProfile.EcMax = double.Parse(EcMax);
            SelectedProfile.LightMin = double.Parse(LightMin);
            SelectedProfile.LightMax = double.Parse(LightMax);

            await _plantProfileService.UpdatePlantProfileAsync(SelectedProfile);
        }

        private void ActivateProfile()
        {
            if (SelectedProfile == null) return;
            // Add activation logic
        }

        private void CreateNewProfile()
        {
            var newProfile = new PlantProfile { Name = "New Profile" };
            Profiles.Add(newProfile);
            SelectedProfile = newProfile;

            PhMin = string.Empty;
            PhMax = string.Empty;
            TempMin = string.Empty;
            TempMax = string.Empty;
            EcMin = string.Empty;
            EcMax = string.Empty;
            LightMin = string.Empty;
            LightMax = string.Empty;
        }

        private void SelectPredefinedProfile(string profileName)
        {
            var predefinedProfiles = new Dictionary<string, PlantProfile>
            {
                { "Cabbage", new PlantProfile { Name = "Cabbage", PhMin = 5.5, PhMax = 6.5, TemperatureMin = 18, TemperatureMax = 24, EcMin = 1.5, EcMax = 2.5, LightMin = 400, LightMax = 800 } },
                { "Salad", new PlantProfile { Name = "Salad", PhMin = 6.0, PhMax = 7.0, TemperatureMin = 15, TemperatureMax = 22, EcMin = 1.2, EcMax = 2.0, LightMin = 300, LightMax = 600 } },
                { "Cucumber", new PlantProfile { Name = "Cucumber", PhMin = 5.8, PhMax = 6.5, TemperatureMin = 20, TemperatureMax = 28, EcMin = 1.8, EcMax = 2.7, LightMin = 500, LightMax = 900 } },
                { "Tomato", new PlantProfile { Name = "Tomato", PhMin = 5.5, PhMax = 6.5, TemperatureMin = 18, TemperatureMax = 24, EcMin = 2.0, EcMax = 3.5, LightMin = 600, LightMax = 1000 } },
            };

            if (predefinedProfiles.TryGetValue(profileName, out var profile))
            {
                SelectedProfile = profile;
                PhMin = profile.PhMin.ToString();
                PhMax = profile.PhMax.ToString();
                TempMin = profile.TemperatureMin.ToString();
                TempMax = profile.TemperatureMax.ToString();
                EcMin = profile.EcMin.ToString();
                EcMax = profile.EcMax.ToString();
                LightMin = profile.LightMin.ToString();
                LightMax = profile.LightMax.ToString();
            }
        }
    }
}
