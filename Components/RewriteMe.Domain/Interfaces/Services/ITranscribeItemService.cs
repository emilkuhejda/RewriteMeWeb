﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ITranscribeItemService
    {
        Task<byte[]> GetSourceAsync(Guid userId, Guid transcribeItemId);

        Task<TranscribeItem> GetAsync(Guid transcribeItemId);

        Task<IEnumerable<TranscribeItem>> GetAllAsync(Guid fileItemId);

        Task<IEnumerable<TranscribeItem>> GetAllForUserAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(TranscribeItem transcribeItem);

        Task AddAsync(IEnumerable<TranscribeItem> transcribeItems);

        Task UpdateUserTranscriptAsync(Guid transcribeItemId, string transcript, DateTime dateUpdated, Guid applicationId);
    }
}
