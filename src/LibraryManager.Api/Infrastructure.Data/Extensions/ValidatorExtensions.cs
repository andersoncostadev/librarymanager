using Core.Commands.v1.Book.Create;
using Core.Commands.v1.Book.Delete;
using Core.Commands.v1.Book.Update;
using Core.Commands.v1.Loan.Create;
using Core.Commands.v1.User.Create;
using Core.Commands.v1.User.Delete;
using Core.Commands.v1.User.Update;
using Core.Queries.v1.Book.GetId;
using Core.Queries.v1.User.GetId;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Extensions
{
    public static class ValidatorExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateBookCommand>, CreateBookCommandValidator>();
            services.AddTransient<IValidator<GetIdBookQuery>, GetIdBookQueryValidator>();
            services.AddTransient<IValidator<DeleteBookCommand>, DeleteBookCommandValidator>();
            services.AddTransient<IValidator<UpdateBookCommand>, UpdateBookCommandValidator>();

            services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            services.AddTransient<IValidator<GetIdUserQuery>, GetIdUserQueryValidator>();
            services.AddTransient<IValidator<DeleteUserCommand>, DeleteUserCommandValidator>();
            services.AddTransient<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

            services.AddTransient<IValidator<CreateLoanCommand>, CreateLoanCommandValidator>();

            return services;
        }
    }
}
