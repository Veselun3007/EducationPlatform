using MediatR;

namespace CourseService.Application.Abstractions {
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery;
    public interface IBaseQuery;
}
