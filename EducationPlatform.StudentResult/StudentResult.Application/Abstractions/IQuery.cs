using MediatR;

namespace StudentResult.Application.Abstractions {
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery;
    public interface IBaseQuery;
}
