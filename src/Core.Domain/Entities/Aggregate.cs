﻿using System.Collections.Generic;
using System.Linq;
using Core.Domain.Events;

namespace Core.Domain.Entities
{
    public abstract class Aggregate<TEntity>: IAggregate
    {
        private readonly IList<IEvent> _uncommittedEvents = new List<IEvent>();

        public void Rehydrate(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Apply(@event);
            }
        }

        public string Id { get; protected set; }
        protected int Version { get; private set; }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents.AsEnumerable();
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void Publish(IEvent e)
        {
            _uncommittedEvents.Add(e);
            Apply(e);
        }

        private void Apply(IEvent @event)
        {
            Version++;
            RedirectToWhen.InvokeEventOptional<Aggregate<TEntity>, TEntity>(this, @event);
        }
    }
}
