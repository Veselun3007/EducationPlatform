using MediatR;

namespace StudentResult.Application.Abstractions {
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
    public interface ICommand : IRequest<Result>, IBaseCommand;
    public interface IBaseCommand;
}
