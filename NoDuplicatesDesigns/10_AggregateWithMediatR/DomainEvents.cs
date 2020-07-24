using MediatR;
using System;
using System.Threading.Tasks;

namespace NoDuplicatesDesigns._10_AggregateWithMediatR
{
    public static class DomainActions
    {
        public static Func<IMediator> Mediator { get; set; }
        public static async Task RaiseEvent<T>(T args) where T : INotification
        {
            var mediator = Mediator.Invoke();
            await mediator.Publish<T>(args);
        }
        public static async Task ValidationRequest<T>(T args) where T : IRequest
        {
            var mediator = Mediator.Invoke();
            await mediator.Send(args);
        }
    }
}
