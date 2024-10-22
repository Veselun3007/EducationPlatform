using MediatR;

namespace CourseService.Application.Abstractions {
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
    public interface ICommand : IRequest<Result>, IBaseCommand;
    public interface IBaseCommand;
}
