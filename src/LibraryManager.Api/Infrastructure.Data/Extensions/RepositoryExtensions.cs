using Core.Repositories;
using Infrastructure.Data.SqlDb;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            return services;
        }
    }
}
