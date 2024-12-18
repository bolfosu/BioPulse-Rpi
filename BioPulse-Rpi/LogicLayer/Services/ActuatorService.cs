using System;
using System.Device.I2c;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LogicLayer.Services
{
    public class ActuatorService
    {
        private readonly ILogger<ActuatorService> _logger;
        private readonly int _busId = 1;
        private readonly int _address;

        public ActuatorService(ILogger<ActuatorService> logger, int actuatorAddress)
        {
            _logger = logger;
            _address = actuatorAddress;
        }

        public virtual async Task SwitchOnAsync()
        {
            await SendCommandAsync(0x01); // ON Command
            _logger.LogInformation("pH UP Actuator switched ON at address 0x{Address:X2}", _address);
        }

        public virtual async Task SwitchOffAsync()
        {
            await SendCommandAsync(0x00); // OFF Command
            _logger.LogInformation("pH UP Actuator switched OFF at address 0x{Address:X2}", _address);
        }

        private Task SendCommandAsync(byte command)
        {
            try
            {
                var settings = new I2cConnectionSettings(_busId, _address);
                using var device = I2cDevice.Create(settings);

                device.Write(new byte[] { command });
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send command to actuator at address 0x{Address:X2}", _address);
                return Task.FromException(ex);
            }
        }
    }
}
