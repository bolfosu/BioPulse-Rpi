using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;
using LogicLayer.Proxies; // Updated namespace for proxies

namespace LogicLayer.Services
{
    public class BluetoothService
    {
        private readonly Connection _connection;

        public BluetoothService()
        {
            _connection = new Connection(Address.System); // Connect to the system bus
        }

        public async Task<List<(string Name, string DevicePath)>> ScanForDevicesAsync()
        {
            try
            {
                Console.WriteLine("Connecting to DBus...");
                await _connection.ConnectAsync();
                Console.WriteLine("Connected to DBus.");

                var adapterPath = "/org/bluez/hci0";
                Console.WriteLine($"Using adapter path: {adapterPath}");
                var adapter = _connection.CreateProxy<IAdapter>("org.bluez", adapterPath);

                Console.WriteLine("Starting BLE scan...");
                await adapter.StartDiscoveryAsync();

                await Task.Delay(10000); // Scan for 10 seconds

                Console.WriteLine("Stopping BLE scan...");
                await adapter.StopDiscoveryAsync();

                var objectManager = _connection.CreateProxy<IObjectManager>("org.bluez", "/");
                Console.WriteLine("Fetching managed objects...");
                var objects = await objectManager.GetManagedObjectsAsync();
                Console.WriteLine($"Found {objects.Count} managed objects.");

                var devices = new List<(string Name, string DevicePath)>();
                foreach (var obj in objects)
                {
                    if (obj.Key.ToString().Contains("dev_") && obj.Value.ContainsKey("org.bluez.Device1"))
                    {
                        var name = obj.Value["org.bluez.Device1"].ContainsKey("Name")
                            ? (string)obj.Value["org.bluez.Device1"]["Name"]
                            : "Unknown Device";

                        devices.Add((name, obj.Key.ToString()));
                        Console.WriteLine($"Device Found: {name} at {obj.Key}");
                    }
                }

                return devices;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during ScanForDevicesAsync: {ex.Message}");
                throw; // Re-throw the exception for the controller to handle
            }
        }

        public async Task ConnectToDeviceAsync(string devicePath)
        {
            try
            {
                Console.WriteLine($"Connecting to device: {devicePath}");
                var device = _connection.CreateProxy<IDevice>("org.bluez", devicePath);
                await device.ConnectAsync();
                Console.WriteLine("Device connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to device: {ex.Message}");
            }
        }

        public async Task<string> ReadCharacteristicAsync(string characteristicPath)
        {
            try
            {
                Console.WriteLine($"Reading characteristic at: {characteristicPath}");
                var characteristic = _connection.CreateProxy<IGattCharacteristic>("org.bluez", characteristicPath);
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
    }
}
