using HomeExpanses.Application.Abstractions.Services;
using HomeExpanses.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HomeExpanses.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<ITransacaoService, TransacaoService>();
            services.AddScoped<ITotaisService, TotaisService>();

            return services;
        }
    }
}