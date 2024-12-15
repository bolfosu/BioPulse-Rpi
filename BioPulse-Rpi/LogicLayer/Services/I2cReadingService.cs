using Microsoft.Extensions.Logging;
using System.Device.I2c;
using System.Text;

namespace LogicLayer.Services
{
    public class I2cReadingService
    {
        private readonly SensorDataIngestionService _ingestionService;
        private readonly ILogger<I2cReadingService> _logger;
        private readonly I2cDevice _device;
        private readonly Timer _timer;

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
                var jsonReading = ReadJsonFromI2c();
                if (!string.IsNullOrEmpty(jsonReading))
                {
                    await _ingestionService.ProcessReadingAsync(jsonReading);
                    _logger.LogInformation("Processed I2C reading: {Reading}", jsonReading);
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

                return rawData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading from I2C device.");
                return string.Empty;
            }
        }
    }
}
