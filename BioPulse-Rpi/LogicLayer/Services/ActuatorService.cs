using System;
using System.Device.I2c;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;

namespace LogicLayer.Services
{
    public class ActuatorService
    {
        private readonly IRepository<Actuator> _actuatorRepo;
        private readonly ILogger<ActuatorService> _logger;
        private readonly int _busId = 1;
        private readonly int _actuatorAddress;

        public ActuatorService(IRepository<Actuator> actuatorRepo, ILogger<ActuatorService> logger, int actuatorAddress = 0x60)
        {
            _actuatorRepo = actuatorRepo;
            _logger = logger;
            _actuatorAddress = actuatorAddress;
        }

        public async Task SwitchOnAsync(int actuatorId)
        {
            await UpdateActuatorStateAsync(actuatorId, true);
            SendCommand(0x01); // ON command
            _logger.LogInformation("Actuator switched ON at address 0x{Address:X2}", _actuatorAddress);
        }

        public async Task SwitchOffAsync(int actuatorId)
        {
            await UpdateActuatorStateAsync(actuatorId, false);
            SendCommand(0x00); // OFF command
            _logger.LogInformation("Actuator switched OFF at address 0x{Address:X2}", _actuatorAddress);
        }

        private async Task UpdateActuatorStateAsync(int actuatorId, bool isOn)
        {
            var actuator = await _actuatorRepo.GetByIdAsync(actuatorId);
            if (actuator == null)
            {
                _logger.LogWarning("Actuator with ID {ActuatorId} not found.", actuatorId);
                return;
            }

            actuator.IsOn = isOn;
            await _actuatorRepo.UpdateAsync(actuator);
        }

        private void SendCommand(byte command)
        {
            try
            {
                var settings = new I2cConnectionSettings(_busId, _actuatorAddress);
                using var device = I2cDevice.Create(settings);

                Span<byte> buffer = stackalloc byte[] { command };
                device.Write(buffer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send command to actuator at address 0x{Address:X2}", _actuatorAddress);
            }
        }
    }
}
