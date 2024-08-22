using Core.Commands.v1.Book.Create;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Extensions
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBookCommand).Assembly));

            return services;
        }
    }
}
