using RewriteMe.Domain.Configuration;

namespace RewriteMe.Business.Configuration
{
    public static class InternalValues
    {
        public static InternalValue<bool> StoreDataInDatabase { get; } = new InternalValue<bool>("StoreDataInDatabase", false);
    }
}
