using System;
using System.Device.I2c;


namespace LogicLayer.Utilities
{
    /// <summary>
    /// Provides low-level functionality for I2C communication with devices.
    /// Handles initialization, reading, and writing to I2C devices.
    /// </summary>
    public class I2CHardware 
    {
        /// <summary>
        /// Validates the device address to ensure it falls within the I2C address range.
        /// </summary>
        /// <param name="devAddr">The device address to validate.</param>
        /// <returns>True if the address is valid; otherwise, false.</returns>
        public bool ValidateDeviceAddress(byte devAddr)
        {
            return devAddr >= 0x08 && devAddr <= 0x77;
        }

        /// <summary>
        /// Initializes the I2C connection with the specified bus ID and device address.
        /// </summary>
        /// <param name="busId">The I2C bus ID.</param>
        /// <param name="devAddr">The address of the I2C device.</param>
        /// <returns>
        /// A response indicating whether the initialization was successful or an error occurred.
        /// </returns>
        public unsafe Either<Response<bool>, I2cError> Initialize(int busId, byte devAddr)
        {
            if (!ValidateDeviceAddress(devAddr))
                return new I2cError("Invalid device address.");

            try
            {
                var connectionSettings = new I2cConnectionSettings(busId, devAddr);
                using var i2cDevice = I2cDevice.Create(connectionSettings);
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new I2cError($"Initialization failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads a single byte from the specified register of the I2C device.
        /// </summary>
        /// <param name="busId">The I2C bus ID.</param>
        /// <param name="devAddr">The address of the I2C device.</param>
        /// <param name="regAddr">The register address to read from.</param>
        /// <returns>
        /// A response containing the read byte or an error if the operation fails.
        /// </returns>
        public unsafe Either<Response<byte>, I2cError> ReadByte(int busId, byte devAddr, byte regAddr)
        {
            if (!ValidateDeviceAddress(devAddr))
                return new I2cError("Invalid device address.");

            try
            {
                using var i2cDevice = I2cDevice.Create(new I2cConnectionSettings(busId, devAddr));
                i2cDevice.WriteByte(regAddr);
                byte readData = i2cDevice.ReadByte();
                return new Response<byte>(readData);
            }
            catch (Exception ex)
            {
                return new I2cError($"ReadByte failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Writes a single byte to the specified register of the I2C device.
        /// </summary>
        /// <param name="busId">The I2C bus ID.</param>
        /// <param name="devAddr">The address of the I2C device.</param>
        /// <param name="regAddr">The register address to write to.</param>
        /// <param name="data">The data byte to write.</param>
        /// <returns>
        /// A response indicating whether the operation was successful or an error occurred.
        /// </returns>
        public unsafe Either<Response<bool>, I2cError> WriteByte(int busId, byte devAddr, byte regAddr, byte data)
        {
            if (!ValidateDeviceAddress(devAddr))
                return new I2cError("Invalid device address.");

            try
            {
                using var i2cDevice = I2cDevice.Create(new I2cConnectionSettings(busId, devAddr));
                var buffer = new[] { regAddr, data };
                i2cDevice.Write(buffer);
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new I2cError($"WriteByte failed: {ex.Message}");
            }
        }
    }
}
