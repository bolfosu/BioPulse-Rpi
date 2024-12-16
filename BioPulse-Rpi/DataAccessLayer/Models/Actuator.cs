

namespace DataAccessLayer.Models
{
    public class Actuator

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOn { get; set; }
        
        public ActuatorType ActuatorType { get; set; }
    }
}
