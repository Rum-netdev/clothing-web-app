using MediatR;

namespace ClothStoreApp.Handler.Infrastructures
{
    public interface ICommandHandler<in TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult> where TCommand : ICommand<TCommandResult>
    {
    }

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
    {

    }
}
