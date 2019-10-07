using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Domain.Interfaces.Repositories;

namespace RewriteMe.DataAccess.Repositories
{
    public class LanguageVersionRepository : ILanguageVersionRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public LanguageVersionRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task UpdateAndroidSendStatusAsync(Guid languageVersionId, bool status)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.LanguageVersions.SingleOrDefaultAsync(x => x.Id == languageVersionId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.SentOnAndroid = status;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateOsxSendStatusAsync(Guid languageVersionId, bool status)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.LanguageVersions.SingleOrDefaultAsync(x => x.Id == languageVersionId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.SentOnOsx = status;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
