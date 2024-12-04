

namespace DataAccessLayer.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsWireless { get; set; }
        public double LastReading { get; set; }
        public DateTime LastReadingTime { get; set; }
        public byte Address { get; set; }

        public SensorType SensorType { get; set; }



    }
}
