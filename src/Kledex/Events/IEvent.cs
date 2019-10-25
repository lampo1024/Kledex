namespace Kledex.Events
{
    public interface IEvent
    {
        bool? UseAmbientTransaction { get; set; }
    }
}
