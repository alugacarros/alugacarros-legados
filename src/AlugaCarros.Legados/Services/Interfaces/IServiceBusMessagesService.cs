public interface IServiceBusMessagesService
{
    Task Publish(string queue, object message);
}