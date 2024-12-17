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
        private string _errorMessage;

        public PlantProfileViewModel(PlantProfileService plantProfileService)
        {
            _plantProfileService = plantProfileService;

            Profiles = new ObservableCollection<PlantProfile>();

            // Commands
            LoadProfilesCommand = ReactiveCommand.CreateFromTask(LoadProfilesAsync);
            SaveProfileCommand = ReactiveCommand.CreateFromTask(SaveProfileAsync);
            ActivateProfileCommand = ReactiveCommand.CreateFromTask(ActivateProfileAsync);
            CreateNewProfileCommand = ReactiveCommand.Create(CreateNewProfile);
            DeleteProfileCommand = ReactiveCommand.CreateFromTask(DeleteProfileAsync);

            LoadProfilesCommand.Execute().Subscribe();
        }

        public ObservableCollection<PlantProfile> Profiles { get; }

        public PlantProfile SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);
                UpdateFieldsForSelectedProfile();
            }
        }

        public string NewProfileName
        {
            get => _newProfileName;
            set => this.RaiseAndSetIfChanged(ref _newProfileName, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        private string _phMin, _phMax, _tempMin, _tempMax, _ecMin, _ecMax, _lightMin, _lightMax;

        public string PhMin { get => _phMin; set => this.RaiseAndSetIfChanged(ref _phMin, value); }
        public string PhMax { get => _phMax; set => this.RaiseAndSetIfChanged(ref _phMax, value); }
        public string TempMin { get => _tempMin; set => this.RaiseAndSetIfChanged(ref _tempMin, value); }
        public string TempMax { get => _tempMax; set => this.RaiseAndSetIfChanged(ref _tempMax, value); }
        public string EcMin { get => _ecMin; set => this.RaiseAndSetIfChanged(ref _ecMin, value); }
        public string EcMax { get => _ecMax; set => this.RaiseAndSetIfChanged(ref _ecMax, value); }
        public string LightMin { get => _lightMin; set => this.RaiseAndSetIfChanged(ref _lightMin, value); }
        public string LightMax { get => _lightMax; set => this.RaiseAndSetIfChanged(ref _lightMax, value); }

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
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading profiles: {ex.Message}";
            }
        }

        private async Task SaveProfileAsync()
        {
            try
            {
                if (SelectedProfile == null || string.IsNullOrWhiteSpace(NewProfileName))
                {
                    ErrorMessage = "Profile name cannot be empty.";
                    return;
                }

                SelectedProfile.Name = NewProfileName;
                SelectedProfile.PhMin = double.Parse(PhMin);
                SelectedProfile.PhMax = double.Parse(PhMax);
                SelectedProfile.TemperatureMin = double.Parse(TempMin);
                SelectedProfile.TemperatureMax = double.Parse(TempMax);
                SelectedProfile.EcMin = double.Parse(EcMin);
                SelectedProfile.EcMax = double.Parse(EcMax);
                SelectedProfile.LightMin = double.Parse(LightMin);
                SelectedProfile.LightMax = double.Parse(LightMax);

                if (SelectedProfile.Id == 0)
                    await _plantProfileService.AddPlantProfileAsync(SelectedProfile);
                else
                    await _plantProfileService.UpdatePlantProfileAsync(SelectedProfile);

                await LoadProfilesAsync();
                ErrorMessage = "Profile saved successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error saving profile: {ex.Message}";
            }
        }

        private async Task ActivateProfileAsync()
        {
            try
            {
                if (SelectedProfile == null)
                {
                    ErrorMessage = "No profile selected to activate.";
                    return;
                }

                await _plantProfileService.ActivateProfileAsync(SelectedProfile.Id);

                // Update message after successful activation
                ErrorMessage = $"Plant profile '{SelectedProfile.Name}' activated successfully.";

                // Refresh profiles to reflect the new active state
                await LoadProfilesAsync();

                // Keep fields updated for the selected profile
                UpdateFieldsForSelectedProfile();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error activating profile: {ex.Message}";
            }
        }


        private void CreateNewProfile()
        {
            SelectedProfile = new PlantProfile();
            ClearFields();
            ErrorMessage = "Insert values for the new profile.";
        }

        private async Task DeleteProfileAsync()
        {
            try
            {
                if (SelectedProfile == null)
                {
                    ErrorMessage = "No profile selected to delete.";
                    return;
                }

                await _plantProfileService.DeletePlantProfileAsync(SelectedProfile.Id);
                Profiles.Remove(SelectedProfile);
                SelectedProfile = null;

                ErrorMessage = "Profile deleted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting profile: {ex.Message}";
            }
        }

        private void UpdateFieldsForSelectedProfile()
        {
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
            ErrorMessage = string.Empty;
        }
    }
}
