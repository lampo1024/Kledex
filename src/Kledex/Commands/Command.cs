﻿namespace Kledex.Commands
{
    public class Command : ICommand
    {
        public bool? PublishEvents { get; set; }
        public bool? UseAmbientTransaction { get; set; }
    }
}
