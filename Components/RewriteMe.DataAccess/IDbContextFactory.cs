namespace RewriteMe.DataAccess
{
    public interface IDbContextFactory
    {
        AppDbContext Create();
    }
}
