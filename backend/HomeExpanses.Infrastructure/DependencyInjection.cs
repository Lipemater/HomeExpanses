using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Infrastructure.Persistence;
using HomeExpanses.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HomeExpanses.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = configuration["Database:Provider"] ?? "Sqlite";

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("A string de conexão não foi configurada.");

            
            services.AddDbContext<AppDbContext>(options =>
            {
                if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlite(connectionString);
                    return;
                }

                throw new InvalidOperationException($"Provider de banco não suportado: {provider}.");
            });

            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            services.AddScoped<ITotaisRepository, TotaisRepository>();

            services.AddScoped<IUnitOfWork>(ServiceProvider => ServiceProvider.GetRequiredService<AppDbContext>());

            return services;            
        }
    }
}