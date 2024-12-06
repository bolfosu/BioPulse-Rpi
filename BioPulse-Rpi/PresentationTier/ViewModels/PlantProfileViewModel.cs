using DataAccessLayer.Models;
using LogicLayer.Services;
using ReactiveUI;
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
        private string _newProfileName;

        public PlantProfileViewModel(PlantProfileService plantProfileService)
        {
            _plantProfileService = plantProfileService;

            Profiles = new ObservableCollection<PlantProfile>();

            // Commands
            LoadProfilesCommand = ReactiveCommand.CreateFromTask(LoadProfilesAsync);
            SaveProfileCommand = ReactiveCommand.CreateFromTask(SaveProfileAsync);
            ActivateProfileCommand = ReactiveCommand.Create(ActivateProfile);
            CreateNewProfileCommand = ReactiveCommand.Create(CreateNewProfile);
            DeleteProfileCommand = ReactiveCommand.CreateFromTask(DeleteProfileAsync);

            // Load profiles on initialization
            LoadProfilesCommand.Execute().Subscribe();
        }

        public ObservableCollection<PlantProfile> Profiles { get; }

        public PlantProfile SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);

                // Update UI fields when a profile is selected
                if (_selectedProfile != null)
                {
                    NewProfileName = _selectedProfile.Name;
                    PhMin = _selectedProfile.PhMin.ToString();
                    PhMax = _selectedProfile.PhMax.ToString();
                    TempMin = _selectedProfile.TemperatureMin.ToString();
                    TempMax = _selectedProfile.TemperatureMax.ToString();
                    EcMin = _selectedProfile.EcMin.ToString();
                    EcMax = _selectedProfile.EcMax.ToString();
                    LightMin = _selectedProfile.LightMin.ToString();
                    LightMax = _selectedProfile.LightMax.ToString();
                }
                else
                {
                    ClearFields();
                }
            }
        }

        public string NewProfileName
        {
            get => _newProfileName;
            set => this.RaiseAndSetIfChanged(ref _newProfileName, value);
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
        public ReactiveCommand<Unit, Unit> DeleteProfileCommand { get; }

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
            if (SelectedProfile == null || string.IsNullOrWhiteSpace(NewProfileName)) return;

            SelectedProfile.Name = NewProfileName;
            SelectedProfile.PhMin = double.Parse(PhMin);
            SelectedProfile.PhMax = double.Parse(PhMax);
            SelectedProfile.TemperatureMin = double.Parse(TempMin);
            SelectedProfile.TemperatureMax = double.Parse(TempMax);
            SelectedProfile.EcMin = double.Parse(EcMin);
            SelectedProfile.EcMax = double.Parse(EcMax);
            SelectedProfile.LightMin = double.Parse(LightMin);
            SelectedProfile.LightMax = double.Parse(LightMax);

            if (SelectedProfile.Id == 0) // New profile
            {
                await _plantProfileService.AddPlantProfileAsync(SelectedProfile);
            }
            else // Update existing profile
            {
                await _plantProfileService.UpdatePlantProfileAsync(SelectedProfile);
            }

            // Reload profiles
            await LoadProfilesAsync();
        }

        private void ActivateProfile()
        {
            if (SelectedProfile == null) return;
            System.Console.WriteLine($"Profile '{SelectedProfile.Name}' activated.");
        }

        private void CreateNewProfile()
        {
            SelectedProfile = new PlantProfile();
            ClearFields();
        }

        private async Task DeleteProfileAsync()
        {
            if (SelectedProfile == null) return;

            await _plantProfileService.DeletePlantProfileAsync(SelectedProfile.Id);

            Profiles.Remove(SelectedProfile);
            SelectedProfile = null;

            System.Console.WriteLine("Profile deleted successfully.");
        }

        private void ClearFields()
        {
            NewProfileName = string.Empty;
            PhMin = string.Empty;
            PhMax = string.Empty;
            TempMin = string.Empty;
            TempMax = string.Empty;
            EcMin = string.Empty;
            EcMax = string.Empty;
            LightMin = string.Empty;
            LightMax = string.Empty;
        }
    }
}
