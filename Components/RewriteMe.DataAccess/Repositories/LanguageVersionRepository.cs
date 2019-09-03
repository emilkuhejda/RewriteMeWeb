using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Messages;

namespace RewriteMe.DataAccess.Repositories
{
    public class LanguageVersionRepository : ILanguageVersionRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public LanguageVersionRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task UpdateAsync(LanguageVersion languageVersion)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = languageVersion.ToLanguageVersionEntity();
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateAndroidSendStatus(Guid languageVersionId, bool status)
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

        public async Task UpdateOsxSendStatus(Guid languageVersionId, bool status)
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
