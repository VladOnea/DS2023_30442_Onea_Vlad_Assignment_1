namespace EnergyManagementSystem.Services
{
    public interface IRabbitMQPublisher
    {
        void PublishMessage<T>(T message);
    }
}
