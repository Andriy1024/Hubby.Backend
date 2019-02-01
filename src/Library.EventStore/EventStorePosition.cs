﻿using System;
using Core.Application;

namespace Library.EventStore
{
    public class EventStorePosition: IReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long CommitPosition { get; set; }
        public long PreparedPosition { get; }
        public long PreparePosition { get; set; }

        public EventStorePosition(string name, long commitPosition, long preparedPosition)
        {
            Name = name;
            CommitPosition = commitPosition;
            PreparedPosition = preparedPosition;
        }
    }
}
