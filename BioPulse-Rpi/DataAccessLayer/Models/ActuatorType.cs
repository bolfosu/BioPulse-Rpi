

namespace DataAccessLayer.Models
{
    public enum ActuatorType
    {
        PhUp, // Actuator for increasing pH levels
        PhDown, // Actuator for decreasing pH levels
        EcUp, // Actuator for increasing electrical conductivity
        EcDown, // Actuator for decreasing electrical conductivity
        LightOnOff, // Actuator for turning light on or off
        WaterPumpOnOff, // Actuator for turning water pump on or off
    }
}
