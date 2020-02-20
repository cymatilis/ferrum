using Ferrum.Core.Domain;
using Ferrum.Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Ferrum.Gateway.Data
{
    public static class GatewayContextSeeder
    {
        public static string DefaultClientName = "Acme Limited (Default Client)";
        public static string DefaultUserId = "a37b078f-31e5-437a-8006-3766930ff670";
        public static string DefaultSecret = "Password123!";

        public static void MigrateAndSeedGatewayDb(this IApplicationBuilder app, bool development = false)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<GatewayDbContext>())
            {
                context.Migrate();
                if(development)
                    context.Seed();
            }                
        }

        private static void Migrate(this GatewayDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
        }

        private static void Seed(this GatewayDbContext context)
        {
            context.AddDefaultClient().AddDefaultClientLogin();
            context.SaveChanges();
        }

        internal static Tuple<Client, GatewayDbContext> AddDefaultClient(this GatewayDbContext dbContext)
        {
            var defaultClient = dbContext.Clients
                .Include(c => c.UserAccounts)
                .FirstOrDefault(c => c.Name == DefaultClientName);

            if (defaultClient == null)
            {
                defaultClient = new Client { Name = DefaultClientName };
                dbContext.Add(defaultClient);
            }

            dbContext.SaveChanges();

            return new Tuple<Client, GatewayDbContext>(defaultClient, dbContext);
        }

        internal static void AddDefaultClientLogin(this Tuple<Client, GatewayDbContext> clientResult)
        {
            var client = clientResult.Item1;
            var dbContext = clientResult.Item2;
            var defaultUserIdGuid = new Guid(DefaultUserId);

            var defaultLogin = client.UserAccounts.FirstOrDefault(login => login.UserId == defaultUserIdGuid);
            if (defaultLogin == null)
            {
                defaultLogin = UserAccount.CreateNewUser();
                defaultLogin.ClientId = client.Id;
                defaultLogin.UserId = defaultUserIdGuid;
                defaultLogin.UserSecret = PasswordUtils.HashPassword(DefaultSecret, defaultLogin.Salt);
                
                dbContext.Add(defaultLogin);
            }
        }
    }
}
