using MediatR;

namespace ClothStoreApp.Handler.Infrastructures
{
    public interface IBroker
    {
        Task<TCommandResult> Command<TCommandResult>(ICommand<TCommandResult> command);
        Task<TCommandResult> Command<TCommandResult>(ICommand<TCommandResult> command, CancellationToken cancellationToken);
        Task Command(ICommand command);
        Task Command(ICommand command, CancellationToken cancellationToken);
        Task<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> query);
        Task<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> query, CancellationToken cancellationToken);
        Task Query(IQuery query);
        Task Query(IQuery query, CancellationToken cancellationToken);
        Task PublishEvents(INotification notification);
    }
}
