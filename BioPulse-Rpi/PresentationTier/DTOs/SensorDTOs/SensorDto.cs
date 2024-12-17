using System.Collections.Generic;
using DataAccessLayer.Models;

namespace PresentationTier.DTOs.SensorDTOs;

public class SensorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ExternalSensorId { get; set; }
    public SensorType SensorType { get; set; }
    public string ConnectionDetails { get; set; }
    public List<SensorReadingDto> SensorReadings { get; set; }

}