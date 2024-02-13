using MediatR;

namespace ClothStoreApp.Handler.Infrastructures
{
    public class Broker : IBroker
    {
        private readonly IMediator _mediator;
        public Broker(IMediator mediator)
        {

            _mediator = mediator;

        }

        public async Task<TCommandResult> Command<TCommandResult>(ICommand<TCommandResult> command)
        {
            return await _mediator.Send(command);
        }

        public async Task<TCommandResult> Command<TCommandResult>(ICommand<TCommandResult> command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task Command(ICommand command)
        {
            await _mediator.Send(command);
        }

        public async Task Command(ICommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
        }

        public async Task PublishEvents(INotification notification)
        {
            await _mediator.Publish(notification);
        }

        public async Task<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> query)
        {
            return await _mediator.Send(query);
        }

        public async Task<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        public async Task Query(IQuery query)
        {
            await _mediator.Send(query);
        }

        public async Task Query(IQuery query, CancellationToken cancellationToken)
        {
            await _mediator.Send(query, cancellationToken);
        }
    }
}
