namespace Kledex.Events
{
    public class Event : IEvent
    {
        public bool? UseAmbientTransaction { get; set; }
    }
}
