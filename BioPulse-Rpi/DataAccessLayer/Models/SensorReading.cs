public class SensorReading
{
    public int Id { get; set; }
    // Primary key for the SensorReading entity, uniquely identifies each reading.

    public int SensorId { get; set; }
    // Foreign key referencing the associated Sensor.
    // Used to establish a relationship between the reading and the sensor.

    public Sensor Sensor { get; set; }
    // Navigation property for accessing the associated Sensor entity.

    public DateTime Timestamp { get; set; }
    // Records when the sensor reading was taken.


    public double? Value { get; set; }
    // Numeric representation of the reading's value.


    


}
