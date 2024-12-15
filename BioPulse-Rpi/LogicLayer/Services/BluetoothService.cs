using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus;
using LogicLayer.Proxies;

namespace LogicLayer.Services
{
    public class BluetoothService
    {
        private readonly Connection _connection;
        private bool _isConnected;

        public BluetoothService()
        {
            _connection = new Connection(Address.System);
            _isConnected = false;
        }

        private async Task EnsureConnectedAsync()
        {
            try
            {
                if (!_isConnected)
                {
                    Console.WriteLine("Connecting to DBus...");
                    await _connection.ConnectAsync();
                    _isConnected = true;
                    Console.WriteLine("Connected to DBus.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to DBus: {ex.Message}");
                throw;
            }
        }

        public async Task EnsureAdapterIsReadyAsync()
        {
            await EnsureConnectedAsync();

            var adapterPath = "/org/bluez/hci0";
            var adapter = _connection.CreateProxy<IAdapter>("org.bluez", adapterPath);

            try
            {
                Console.WriteLine("Checking adapter properties...");
                var propertiesProxy = _connection.CreateProxy<IProperties>("org.bluez", adapterPath);

                var isPowered = (bool)await propertiesProxy.GetAsync("org.bluez.Adapter1", "Powered");

                if (!isPowered)
                {
                    Console.WriteLine("Adapter is not powered. Powering it on...");
                    await propertiesProxy.SetAsync("org.bluez.Adapter1", "Powered", true);
                    Console.WriteLine("Adapter powered on.");
                }
                else
                {
                    Console.WriteLine("Adapter is already powered on.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring adapter is ready: {ex.Message}");
                throw;
            }
        }


        public async Task<List<(string Name, string DevicePath)>> ScanForDevicesAsync()
        {
            try
            {
                await EnsureAdapterIsReadyAsync();

                var adapterPath = "/org/bluez/hci0";
                var adapter = _connection.CreateProxy<IAdapter>("org.bluez", adapterPath);

                Console.WriteLine("Starting BLE scan...");
                await adapter.StartDiscoveryAsync();

                await Task.Delay(10000);

                Console.WriteLine("Stopping BLE scan...");
                await adapter.StopDiscoveryAsync();

                var objectManager = _connection.CreateProxy<IObjectManager>("org.bluez", "/");
                var objects = await objectManager.GetManagedObjectsAsync();
                Console.WriteLine($"Found {objects.Count} managed objects.");

                var devices = objects
                    .Where(obj => obj.Key.ToString().Contains("dev_") && obj.Value.ContainsKey("org.bluez.Device1"))
                    .Select(obj =>
                    {
                        var name = obj.Value["org.bluez.Device1"].ContainsKey("Name")
                            ? (string)obj.Value["org.bluez.Device1"]["Name"]
                            : "Unknown Device";
                        return (name, obj.Key.ToString());
                    })
                    .ToList();

                foreach (var (name, path) in devices)
                {
                    Console.WriteLine($"Device Found: {name} at {path}");
                }

                return devices;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during ScanForDevicesAsync: {ex.Message}");
                return new List<(string, string)>();
            }
        }
        public async Task<string> ReadCharacteristicAsync(string devicePath)
        {
            try
            {
                Console.WriteLine($"Reading characteristic at: {devicePath}");
                var characteristic = _connection.CreateProxy<IGattCharacteristic>("org.bluez", devicePath);
                var data = await characteristic.ReadValueAsync(new Dictionary<string, object>());
                var dataString = BitConverter.ToString(data);
                Console.WriteLine($"Data read: {dataString}");
                return dataString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading characteristic: {ex.Message}");
                return null;
            }
        }

        public async Task ConnectToDeviceAsync(string devicePath)
        {
            try
            {
                await EnsureConnectedAsync();

                var device = _connection.CreateProxy<IDevice>("org.bluez", devicePath);
                await device.ConnectAsync();
                Console.WriteLine("Device connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to device: {ex.Message}");
            }
        }
    }
}
