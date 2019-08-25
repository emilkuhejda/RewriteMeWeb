﻿using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IFileItemSourceRepository
    {
        Task AddAsync(FileItemSource fileItemSource);
    }
}