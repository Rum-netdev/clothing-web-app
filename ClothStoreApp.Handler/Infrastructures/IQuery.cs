using MediatR;

namespace ClothStoreApp.Handler.Infrastructures
{
    public interface IQuery<TResult> : IRequest<TResult>
    {
    }

    public interface IQuery : IRequest { }
}
