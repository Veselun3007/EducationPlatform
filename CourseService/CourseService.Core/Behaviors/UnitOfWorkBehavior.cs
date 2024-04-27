using CourseService.Application.Abstractions;
using CourseService.Infrastructure.Interfaces;
using MediatR;
using System.Transactions;

namespace CourseService.Application.Behaviors {
    public class UnitOfWorkBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse> 
        //where TResponse : Result 
        {

        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkBehavior(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
            if(!typeof(TRequest).Name.EndsWith("Command")) return await next();

            //using (var transactionScope = new TransactionScope()) {
            //    var response = await next();
            //    await _unitOfWork.SaveChangesAsync();
            //    transactionScope.Complete();
            //    return response;
            //}
            
            var response = await next();
            //if(!response.IsSuccess) return response;
            //перевірка результату
            await _unitOfWork.SaveChangesAsync();
            return response;
        }
    }
}
