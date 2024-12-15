using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using LogicLayer.Services;
using ReactiveUI;

namespace PresentationTier.ViewModels
{
    public class DeviceSettingsViewModel : ReactiveObject
    {
        private readonly BluetoothService _bluetoothService;

        public DeviceSettingsViewModel(BluetoothService bluetoothService)
        {
            _bluetoothService = bluetoothService;
            ScannedDevices = new ObservableCollection<(string Name, string DevicePath)>();

            ScanCommand = ReactiveCommand.CreateFromTask(ScanForDevicesAsync);
            ConnectCommand = ReactiveCommand.CreateFromTask(ConnectToDeviceAsync);
        }

        public ObservableCollection<(string Name, string DevicePath)> ScannedDevices { get; }

        private (string Name, string DevicePath)? _selectedDevice;
        public (string Name, string DevicePath)? SelectedDevice
        {
            get => _selectedDevice;
            set => this.RaiseAndSetIfChanged(ref _selectedDevice, value);
        }

        public ReactiveCommand<Unit, Unit> ScanCommand { get; }
        public ReactiveCommand<Unit, Unit> ConnectCommand { get; }

        private async Task ScanForDevicesAsync()
        {
            ScannedDevices.Clear();
            var devices = await _bluetoothService.ScanForDevicesAsync();
            foreach (var device in devices)
            {
                ScannedDevices.Add(device);
            }
        }

        private async Task ConnectToDeviceAsync()
        {
            if (SelectedDevice.HasValue)
            {
                await _bluetoothService.ConnectToDeviceAsync(SelectedDevice.Value.DevicePath);
            }
        }
    }
}
