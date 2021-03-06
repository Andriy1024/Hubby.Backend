﻿using Core.Domain;
using Core.Domain.Events;

namespace Buddy.Domain.Events
{
    public class BuddyRemoved: IEvent
    {
        public BuddyRemoved(string id, string buddyId)
        {
            Id = id;
            BuddyId = buddyId;
        }
        public string Id { get; }
        public string BuddyId { get; }
    }
}
