using EnergyManagementSystem.Dto;
using EnergyManagementSystem.Services;
using EnergyManagementSystem.Services.DeviceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;

namespace EnergyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        private readonly IDeviceService _deviceService;

        private readonly IRabbitMQPublisher _rabbitMQPublisher;
        public DeviceController(IDeviceService deviceService, IRabbitMQPublisher rabbitMQPublisher) 
        { 
            _deviceService = deviceService;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllDevicesDto>>> GetAllDevices()
        {
            return await _deviceService.GetAllDevices();
        }

        [HttpGet]
        [Route("getUserDevices/{id:int}")]
        public async Task<ActionResult<List<Device>>> GetUserDevices(int id)
        {
            return await _deviceService.GetUserDevices(id);
        }


        [HttpPost]
        public async Task<ActionResult<Device>> AddDevice(DeviceDto request)
        {
            var addedDevice =await _deviceService.AddDevice(request);

            return addedDevice is null ? BadRequest() : addedDevice;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<List<Device>>> UpdateDevice(int id, DeviceDto request)
        {
            await  _deviceService.UpdateDevice(id, request);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<List<Device>>> DeleteDevice(int id)
        {
            await  _deviceService.DeleteDevice(id);
            return Ok();
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishDevices()
        {
            await _deviceService.PublishDeviceSyncMessages();
            return Ok();
        }


    }
}
