﻿using System;
using System.Collections.Generic;
using Core.Domain.Entities;

namespace Region.Domain
{
    public class Region : Aggregate<Region>
    {
        private IList<string> _groupIds;

        public void Create(string id, string name)
        {
            if(Version != 0) return;
            var e = new RegionCreated(id, name);
            Publish(e);
        }

        public void AddGroup(string groupId)
        {
            if (Version == 0)
                throw new InvalidOperationException("Group needs to be started first");

            if (_groupIds.Contains(groupId))
                throw new InvalidOperationException("Group already added to region");

            var e = new GroupAddedToRegion(Id, groupId);
            Publish(e);
        }

        private void When(RegionCreated e)
        {
            Id = e.Id;
            _groupIds = new List<string>();
        }

        private void When(GroupAddedToRegion e)
        {
            _groupIds.Add(e.GroupId);
        }
    }
}
