using System.Threading.Tasks;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserSubscriptionRepository
    {
        Task AddAsync(UserSubscription userSubscription);
    }
}
