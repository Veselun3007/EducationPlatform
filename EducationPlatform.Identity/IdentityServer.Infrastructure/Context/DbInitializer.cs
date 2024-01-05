namespace IdentityServer.Infrastructure.Context
{
    public class DbInitializer
    {
        public static void Initialize(IdentityDBContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
