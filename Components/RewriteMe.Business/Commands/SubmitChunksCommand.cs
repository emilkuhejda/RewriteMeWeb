using System;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Business.Commands
{
    public class SubmitChunksCommand : CommandBase<FileItemDto>
    {
        public Guid FileItemId { get; set; }

        public int ChunksCount { get; set; }

        public StorageSetting ChunksStorageSetting { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
