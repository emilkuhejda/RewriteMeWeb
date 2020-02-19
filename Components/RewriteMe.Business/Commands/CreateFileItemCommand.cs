﻿using System;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.Business.Commands
{
    public class CreateFileItemCommand : CommandBase<FileItemDto>
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public string FileName { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
