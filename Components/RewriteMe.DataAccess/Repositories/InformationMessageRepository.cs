using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Messages;

namespace RewriteMe.DataAccess.Repositories
{
    public class InformationMessageRepository : IInformationMessageRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public InformationMessageRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(InformationMessage informationMessage)
        {
            using (var context = _contextFactory.Create())
            {
                await context.InformationMessages.AddAsync(informationMessage.ToInformationMessageEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<InformationMessage>> GetAllAsync(DateTime updatedAfter)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.InformationMessages
                    .Where(x => x.DateCreated >= updatedAfter)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return entities.Select(x => x.ToInformationMessage());
            }
        }
    }
}
