using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Contracts;
using System.Threading.Tasks;
using Tournament.Api.Extensions;
using Tournament.Core.Repositories;
using Tournament.Data.Data;
using Tournament.Data.Repositories;
using Tournament.Services;

namespace Tournament.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TournamentApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentApiContext") ?? throw new InvalidOperationException("Connection string 'TournamentApiContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers(o => o.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();

            AddServices(builder.Services);


            builder.Services.AddAutoMapper(typeof(TournamentMappings));

            var app = builder.Build();

            await app.SeedDataAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddLazy<ITournamentService>();
            services.AddLazy<IGameService>();
        }
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLazy<TService>(this IServiceCollection services) where TService : class
        {
            return services.AddScoped(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
        }
    }
}
