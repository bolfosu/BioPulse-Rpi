using Microsoft.Extensions.Logging;
using System;
using System.Device.I2c;
using System.Text;
using System.Threading;

namespace LogicLayer.Services
{
    public class I2cReadingService
    {
        private readonly SensorDataIngestionService _ingestionService;
        private readonly ILogger<I2cReadingService> _logger;
        private readonly I2cDevice _device;
        private readonly Timer _timer;
        private readonly StringBuilder _buffer = new(); // Buffer for accumulating partial data

        public I2cReadingService(
            SensorDataIngestionService ingestionService,
            ILogger<I2cReadingService> logger,
            int busId,
            int address,
            int intervalMs = 1000)
        {
            _ingestionService = ingestionService;
            _logger = logger;

            var settings = new I2cConnectionSettings(busId, address);
            _device = I2cDevice.Create(settings);

            _logger.LogInformation("I2C Reading Service initialized. Bus ID: {BusId}, Address: {Address}", busId, address);

            _timer = new Timer(ReadAndIngest, null, 0, intervalMs);
        }

        private async void ReadAndIngest(object state)
        {
            try
            {
                string jsonReading = ReadJsonFromI2c();
                if (!string.IsNullOrEmpty(jsonReading))
                {
                    // Validate and process data
                    if (IsValidJson(jsonReading))
                    {
                        await _ingestionService.ProcessReadingAsync(jsonReading);
                        _logger.LogInformation("Processed I2C reading: {Reading}", jsonReading);
                    }
                    else
                    {
                        _logger.LogWarning("Invalid JSON data received: {RawData}", jsonReading);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing I2C reading.");
            }
        }

        private string ReadJsonFromI2c()
        {
            try
            {
                Span<byte> buffer = stackalloc byte[32];
                _device.Read(buffer);

                var rawData = Encoding.ASCII.GetString(buffer).TrimEnd('\0');
                _logger.LogInformation("Raw I2C data: {RawData}", rawData);

                // Append data to the buffer
                _buffer.Append(rawData);

                // Find the first and last JSON object boundaries
                int startIndex = _buffer.ToString().IndexOf('{');
                int endIndex = _buffer.ToString().LastIndexOf('}');
                if (startIndex != -1 && endIndex > startIndex)
                {
                    // Extract and return complete JSON object
                    var jsonString = _buffer.ToString(startIndex, endIndex - startIndex + 1);
                    _buffer.Remove(0, endIndex + 1); // Remove processed data
                    return jsonString;
                }

                return string.Empty; // No complete JSON object yet
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading from I2C device.");
                return string.Empty;
            }
        }

        private bool IsValidJson(string json)
        {
            try
            {
                System.Text.Json.JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
