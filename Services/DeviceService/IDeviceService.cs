using EnergyManagementSystem.Dto;
using EnergyManagementSystem.Models;

namespace EnergyManagementSystem.Services.DeviceService
{
    public interface IDeviceService
    {
        public  Task<List<GetAllDevicesDto>> GetAllDevices();

        public Task<Device?> AddDevice(DeviceDto device);

        public Task<List<Device>> GetUserDevices(int userid);

        public Task UpdateDevice(int deviceId, DeviceDto device);

        public Task DeleteDevice(int deviceId);

        public Task PublishDeviceSyncMessages();


    }
}
