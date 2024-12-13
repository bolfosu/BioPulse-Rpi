using DataAccessLayer.Models;

public class Sensor
{
    public int Id { get; set; }
    // Primary key for the Sensor entity, uniquely identifies each sensor.

    public string Name { get; set; }
    // Human-readable name or label for the sensor (e.g., "Temperature Sensor 1").

    public bool IsEnabled { get; set; }
    // Indicates whether the sensor readings to be stored in db or not.

    public bool IsWireless { get; set; }
    // Specifies if the sensor is wireless (true) or wired (false).

    public string HardwareAddress { get; set; }
    // Stores the hardware-specific address of the sensor:
    // - For wireless sensors: Could be a MAC address.
    // - For wired sensors: Could be a GPIO pin or bus address.

    public SensorType SensorType { get; set; }
    // Enum representing the type of sensor (e.g., Temperature, pH, EC).

    public string ConnectionDetails { get; set; }
    // Stores details specific to the sensor's connection configuration.
    // Example: For wireless sensors, this could include BLE-specific details.

    public string Metadata { get; set; }
    // Optional field for storing additional configuration or information as JSON or key-value pairs.
    // Example: Calibration data, measurement units, or device-specific settings.


    public ICollection<SensorReading> SensorReadings { get; set; }
    // Navigation property for the one-to-many relationship with `SensorReading`.
    // Allows accessing all readings associated with this sensor.
}
