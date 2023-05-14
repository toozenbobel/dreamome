using MediatR;
using Serilog;

namespace Common.PipelineBehaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Log.Information("Started executing {Request}", request);

        var response = await next();

        Log.Information("Execution for {Request} complete", request.GetType().Name);

        return response;
    }
}