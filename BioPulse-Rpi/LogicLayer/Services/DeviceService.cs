﻿using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class DeviceService
    {
        private readonly IRepository<Sensor> _sensorRepository;
        private readonly BluetoothService _bluetoothService;

        public DeviceService(IRepository<Sensor> sensorRepository, BluetoothService bluetoothService)
        {
            _sensorRepository = sensorRepository;
            _bluetoothService = bluetoothService;
        }

        public async Task<List<Sensor>> ScanAndRegisterDevicesAsync()
        {
            var devices = await _bluetoothService.ScanForDevicesAsync();
            var sensors = new List<Sensor>();

            foreach (var (name, devicePath) in devices)
            {
                var sensor = new Sensor
                {
                    Name = name,
                    IsEnabled = true,
                    IsWireless = true,
                    HardwareAddress = devicePath, // Store the device path for BlueZ
                    SensorType = SensorType.Temperature, // Placeholder for now
                    Metadata = "{ \"manufacturer\": \"ESP32\" }"
                };

                await _sensorRepository.AddAsync(sensor);
                sensors.Add(sensor);
            }

            return sensors;
        }

        public async Task<bool> ConnectToSensorAsync(int sensorId)
        {
            var sensor = await _sensorRepository.GetByIdAsync(sensorId);
            if (sensor == null || !sensor.IsWireless)
                return false;

            await _bluetoothService.ConnectToDeviceAsync(sensor.HardwareAddress);
            sensor.ConnectionDetails = "Connected";
            await _sensorRepository.UpdateAsync(sensor);

            return true;
        }

        public async Task<string> ReadSensorDataAsync(int sensorId)
        {
            var sensor = await _sensorRepository.GetByIdAsync(sensorId);
            if (sensor == null || !sensor.IsWireless)
                return null;

            // Update with specific service/characteristic paths for BlueZ
            var data = await _bluetoothService.ReadCharacteristicAsync(sensor.HardwareAddress);
            return data;
        }

        public async Task<bool> DisconnectSensorAsync(int sensorId)
        {
            var sensor = await _sensorRepository.GetByIdAsync(sensorId);
            if (sensor == null || !sensor.IsWireless)
                return false;

            // BlueZ does not require explicit disconnection in many cases
            sensor.ConnectionDetails = "Disconnected";
            await _sensorRepository.UpdateAsync(sensor);

            return true;
        }
    }
}
