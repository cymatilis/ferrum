using Ferrum.Core.Extensions;
using Ferrum.Core.ServiceInterfaces;
using Ferrum.Gateway.Authentication;
using Ferrum.Gateway.Data;
using Ferrum.Gateway.ErrorHandling;
using Ferrum.Gateway.Integrations.FakeBank;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ferrum.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.AddEnumSerializers());

            services.AddDbContext<GatewayDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GatewayDb"),
                    sqlOpt => sqlOpt.EnableRetryOnFailure()));

            services.AddScoped<AuthoriseClient>();

            services.AddHttpClient<ICardAuthorisation, FakeBankAuthorisation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.MigrateAndSeedGatewayDb(development: true);
            }
            else
            {
                app.MigrateAndSeedGatewayDb(development: false);
            }
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
