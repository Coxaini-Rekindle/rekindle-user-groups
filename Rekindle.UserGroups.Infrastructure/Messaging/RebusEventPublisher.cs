using Rebus.Bus;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts;

namespace Rekindle.UserGroups.Infrastructure.Messaging;

/// <summary>
/// Implementation of IEventPublisher using Rebus
/// </summary>
public class RebusEventPublisher : IEventPublisher
{
    private readonly IBus _bus;

    public RebusEventPublisher(IBus bus)
    {
        _bus = bus;
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : Event
    {
        await _bus.Publish(@event);
    }
}
