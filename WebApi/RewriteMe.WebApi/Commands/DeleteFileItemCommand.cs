using System;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.WebApi.Commands
{
    public class DeleteFileItemCommand : CommandBase<OkDto>
    {
        public Guid FileItemId { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
