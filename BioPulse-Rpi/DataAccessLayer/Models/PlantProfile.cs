

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
        public DateTime LightOnTime { get; set; } 
        public DateTime LightOffTime { get; set; }
        public double LightMin { get; set; }
        public double LightMax { get; set; }
        public double EcMin { get; set; }
        public double EcMax { get; set; }
    }
}
