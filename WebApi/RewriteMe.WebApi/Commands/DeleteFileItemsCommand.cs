using System;
using System.Collections.Generic;
using RewriteMe.Domain.Dtos;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Commands
{
    public class DeleteFileItemsCommand : CommandBase<OkDto>
    {
        public IEnumerable<DeletedFileItemModel> FileItems { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
