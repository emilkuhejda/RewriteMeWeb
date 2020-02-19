using System;
using MediatR;

namespace RewriteMe.Business.Commands
{
    public abstract class CommandBase<T> : IRequest<T> where T : class
    {
        public Guid UserId { get; set; }
    }
}
