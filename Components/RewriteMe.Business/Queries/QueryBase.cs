﻿using MediatR;

namespace RewriteMe.Business.Queries
{
    public abstract class QueryBase<TResult> : IRequest<TResult> where TResult : class
    {
    }
}
