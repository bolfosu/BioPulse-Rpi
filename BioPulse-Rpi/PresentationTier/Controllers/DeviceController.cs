using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LogicLayer.Services;

namespace PresentationTier.Controllers;

[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly DeviceService _deviceService;

    public DeviceController(DeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet("scan")]
    public async Task<IActionResult> ScanForDevices()
    {
        var devices = await _deviceService.ScanAndRegisterDevicesAsync();
        return Ok(devices);
    }

    [HttpPost("{sensorId}/connect")]
    public async Task<IActionResult> ConnectToSensor(int sensorId)
    {
        var result = await _deviceService.ConnectToSensorAsync(sensorId);
        return result ? Ok() : BadRequest("Failed to connect to sensor.");
    }

    [HttpGet("{sensorId}/data")]
    public async Task<IActionResult> ReadSensorData(int sensorId)
    {
        var data = await _deviceService.ReadSensorDataAsync(sensorId);
        return data != null ? Ok(data) : NotFound("Sensor data not available.");
    }

    [HttpPost("{sensorId}/disconnect")]
    public async Task<IActionResult> DisconnectSensor(int sensorId)
    {
        var result = await _deviceService.DisconnectSensorAsync(sensorId);
        return result ? Ok() : BadRequest("Failed to disconnect sensor.");
    }
}

