﻿using System.Threading.Tasks;
using Buddy.Domain.Events;
using Core.Application.Command;
using MediatR;

namespace Buddy.Application.CommandService.Buddy.JoinGroup
{
    public class JoinGroupBuddyAddedListener: CommandListener<BuddyAdded>
    {
        public JoinGroupBuddyAddedListener(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task Handle(BuddyAdded notification)
        {
            var command = new JoinGroupCommand(notification.BuddyId, notification.Id, notification.Type);
            await Mediator.Publish(command);
        }
    }
}
