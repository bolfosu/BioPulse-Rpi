using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PresentationTier.Controllers;

[ApiController]
[Route("api/bluetooth")]
public class BluetoothController : ControllerBase
{
    private readonly BluetoothService _bluetoothService;

    public BluetoothController(BluetoothService bluetoothService)
    {
        _bluetoothService = bluetoothService;
    }

    [HttpGet("scan")]
    public async Task<IActionResult> ScanForDevices()
    {
        try
        {
            Console.WriteLine("ScanForDevices endpoint hit.");
            var devices = await _bluetoothService.ScanForDevicesAsync();
            return Ok(devices);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ScanForDevices: {ex.Message}");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }


    [HttpPost("connect")]
    public async Task<IActionResult> ConnectToDevice([FromQuery] string devicePath)
    {
        await _bluetoothService.ConnectToDeviceAsync(devicePath);
        return Ok("Connected to device.");
    }
}
