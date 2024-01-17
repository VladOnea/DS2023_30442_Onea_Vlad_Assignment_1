using EnergyManagementSystem.Data;
using EnergyManagementSystem.Dto;
using EnergyManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnergyManagementSystem.Services.DeviceService
{
    public class DeviceService : IDeviceService
    {

        private readonly DeviceDataContext _dbContext;
        private readonly IRabbitMQPublisher _publisher;

        public DeviceService(DeviceDataContext dbContext, IRabbitMQPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        public async Task<Device?> AddDevice(DeviceDto deviceDto)
        {
            var foundUser = new User();

            var hasOwner = !string.IsNullOrWhiteSpace(deviceDto.OwnerUsername);

            if (hasOwner)
            {
                foundUser = await _dbContext.Users
                   .AsNoTracking()
                   .FirstAsync(u => u.Username == deviceDto.OwnerUsername);
            }

            var device = new Device
            {
                Name = deviceDto.Name,
                Description = deviceDto.Description,
                Address = deviceDto.Address,
                EnergyConsumption = deviceDto.EnergyConsumption,
                UserId = hasOwner ? foundUser.Id : null,
            };

            _dbContext.Devices.Add(device);
            await _dbContext.SaveChangesAsync();

            return device;
        }


        public async Task DeleteDevice(int deviceId)
        {
            var user = await _dbContext.Devices.FirstOrDefaultAsync(u => u.Id == deviceId);

            if (user != null)
            {
                _dbContext.Devices.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<GetAllDevicesDto>> GetAllDevices()
        {

            var devices = await _dbContext.Devices
                .Include(u => u.User)
                .Select(u => new GetAllDevicesDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Description = u.Description,
                    Address = u.Address,
                    EnergyConsumption = u.EnergyConsumption,
                    OwnerUsername = u.User.Username

                }).ToListAsync();
            return devices;
        }

        public async Task<List<Device>> GetUserDevices(int userid)
        {
            var devices = await _dbContext.Devices.Where(u => u.UserId == userid).ToListAsync();
            return devices;
        }


        public async Task UpdateDevice(int deviceId, DeviceDto device)
        {
            var foundDevice = await _dbContext.Devices.FirstOrDefaultAsync(u => u.Id == deviceId);

            if (foundDevice is null)
            {
                return;
            }

            var foundUser = new User();

            if (!string.IsNullOrWhiteSpace(device.OwnerUsername))
            {
                foundUser = await _dbContext.Users
                    .AsNoTracking()
                    .FirstAsync(u => u.Username == device.OwnerUsername);
            }



            foundDevice.Name = device.Name;
            foundDevice.Description = device.Description;
            foundDevice.Address = device.Address;
            foundDevice.EnergyConsumption = device.EnergyConsumption;
            foundDevice.UserId = !string.IsNullOrWhiteSpace(device.OwnerUsername) ? foundUser.Id : null;

            await _dbContext.SaveChangesAsync();
        }

        public async Task PublishDeviceSyncMessages()
        {
            var devices = await _dbContext.Devices.Include(d => d.User).ToListAsync();
            foreach (var device in devices)
            {
                var syncDeviceDto = new SyncDeviceDto
                {
                    UserId = device.UserId,
                    DeviceId = device.Id,
                    MaxConsumption = device.EnergyConsumption
                };

                _publisher.PublishMessage(syncDeviceDto);
            }
        }
    }
}
