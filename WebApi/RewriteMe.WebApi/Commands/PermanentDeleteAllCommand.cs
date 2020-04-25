using System;
using System.Collections.Generic;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.WebApi.Commands
{
    public class PermanentDeleteAllCommand : CommandBase<OkDto>
    {
        public IEnumerable<Guid> FileItemIds { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
