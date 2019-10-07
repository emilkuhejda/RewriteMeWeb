using RewriteMe.Domain.Configuration;

namespace RewriteMe.Business.Configuration
{
    public static class InternalValues
    {
        public static InternalValue<bool> ReadSourceFromDatabase { get; } = new InternalValue<bool>("ReadSourceFromDatabase", false);
    }
}
