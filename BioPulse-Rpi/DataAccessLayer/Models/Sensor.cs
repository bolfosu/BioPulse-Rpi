using DataAccessLayer.Models;

public class Sensor
{
    public int Id { get; set; }
    // Primary key for the Sensor entity, uniquely identifies each sensor.

    public string Name { get; set; }
    // Human-readable name or label for the sensor (e.g., "Temperature Sensor 1").

    public string ExternalSensorId { get; set; }
    //To store the external sensor id

    public SensorType SensorType { get; set; }
    // Enum representing the type of sensor (e.g., Temperature, pH, EC).

    
    public string ConnectionDetails { get; set; }
    // Stores details specific to the sensor's connection configuration.
    // Example: For wireless sensors, this could include BLE-specific details.

  
    public ICollection<SensorReading> SensorReadings { get; set; }
    // Navigation property for the one-to-many relationship with `SensorReading`.
    // Allows accessing all readings associated with this sensor.
}
