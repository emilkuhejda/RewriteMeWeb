using MediatR;

namespace RewriteMe.WebApi.Queries
{
    public abstract class QueryBase<TResult> : IRequest<TResult> where TResult : class
    {
    }
}
