using System;
using dotnet.Common.ErrorHandling;


namespace LogicLayer.Utilities
{
    /// <summary>
    /// Provides high-level management of I2C communication, encapsulating error handling and utility functions.
    /// </summary>
    public class I2CManager
    {
        private readonly II2CHardware _i2cHardware;

        /// <summary>
        /// Initializes a new instance of the <see cref="I2CManager"/> class.
        /// </summary>
        /// <param name="i2cHardware">The hardware implementation for I2C communication.</param>
        public I2CManager(II2CHardware i2cHardware)
        {
            _i2cHardware = i2cHardware;
        }

        /// <summary>
        /// Reads a single byte from the specified register of the I2C device.
        /// </summary>
        /// <param name="busId">The I2C bus ID.</param>
        /// <param name="devAddr">The address of the I2C device.</param>
        /// <param name="regAddr">The register address to read from.</param>
        /// <returns>The read byte if successful; otherwise, null.</returns>
        public unsafe byte? ReadByte(int busId, byte devAddr, byte regAddr)
        {
            return _i2cHardware.ReadByte(busId, devAddr, regAddr).Match<byte?>(
                success => success.Data,
                error =>
                {
                    Console.WriteLine($"I2C ReadByte Error: {error.ErrorMessage}");
                    return null;
                });
        }

        /// <summary>
        /// Writes a single byte to the specified register of the I2C device.
        /// </summary>
        /// <param name="busId">The I2C bus ID.</param>
        /// <param name="devAddr">The address of the I2C device.</param>
        /// <param name="regAddr">The register address to write to.</param>
        /// <param name="data">The data byte to write.</param>
        public unsafe void WriteByte(int busId, byte devAddr, byte regAddr, byte data)
        {
            _i2cHardware.WriteByte(busId, devAddr, regAddr, data).Match(
                success => Console.WriteLine("WriteByte successful."),
                error => Console.WriteLine($"I2C WriteByte Error: {error.ErrorMessage}")
            );
        }
    }
}
