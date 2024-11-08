

namespace DataAccessLayer.Models
{
   public class PlantProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public double PhMin { get; set; }
        public double PhMax { get; set; }
        public double TemperatureMin { get; set; }
        public double TemperatureMax { get; set; }
        public TimeSpan LightOnTime { get; set; } 
        public TimeSpan LightOffTime { get; set; }
        public double LuxMin { get; set; }
        public double LuxMax { get; set; }
        public double EcMin { get; set; }
        public double EcMax { get; set; }
    }
}
