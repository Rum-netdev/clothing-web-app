using MediatR;

namespace ClothStoreApp.Handler.Infrastructures
{
    public interface ICommand<TResult> : IRequest<TResult>
    {
    }

    public interface ICommand : IRequest { }
}
