using System;

namespace PresentationTier.DTOs.SensorDTOs;

public class SensorReadingDto
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double? Value { get; set; }
}