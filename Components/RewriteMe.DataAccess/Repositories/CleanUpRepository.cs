﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;

namespace RewriteMe.DataAccess.Repositories
{
    public class CleanUpRepository : ICleanUpRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public CleanUpRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CleanFileItemSourcesAsync(DateTime deleteBefore)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemIds = await context.FileItems
                    .Where(x => x.RecognitionState == RecognitionState.Completed && x.DateProcessed.HasValue &&
                                x.DateProcessed.Value < deleteBefore)
                    .Select(x => x.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task CleanTranscribeItemSourceAsync(DateTime deleteBefore)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemIds = await context.TranscribeItems
                    .Where(x => x.DateCreated < deleteBefore)
                    .Select(x => x.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
