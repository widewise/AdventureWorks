using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace AdventureWorks.Web.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Logger.Log.Error($"Request path: '{context.Request.RequestUri.AbsolutePath}':", context.Exception);

            return Task.FromResult<object>(null);
        }
    }
}