﻿using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.Shared
{
    public class ReplayEventsCommand : DomainCommand
    {
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        ///     Replay events that have occurred from and including this moment.
        /// </summary>
        public long? FromTimestamp { get; set; }

        /// <summary>
        ///     Replay events that have occurred upto and including this moment.
        /// </summary>
        public long? ToTimestamp { get; set; }

        /// <summary>
        ///     Replay only events from exactly this type.
        ///     IF null, all event types will be replayed.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        ///     Replay only events that match this Topic.
        /// </summary>
        public string Topic { get; set; }
    }
}