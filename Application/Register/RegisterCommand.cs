using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Timeoff.ResultModels;

namespace Timeoff.Application.Register
{
    public record RegisterCommand : IRequest<ApiResult>, Commands.IValidated
    {
        public string? CompanyName { get; init; }

        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? Email { get; init; }

        public string? Password { get; init; }

        public string? ConfirmPassword { get; init; }

        public string? Country { get; init; }

        public string? TimeZone { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class RegisterCommandHandler(
        IOptions<Types.Options> options,
        IDataContext dataContext,
        Services.IUsersService usersService,
        Services.IEmailTemplateService templateService)
        : IRequestHandler<RegisterCommand, ApiResult>
    {
        private readonly Types.Options _options = options.Value;
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.IUsersService _usersService = usersService;
        private readonly Services.IEmailTemplateService _templateService = templateService;

        public async Task<ApiResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (!_options.AllowNewAccountCreation)
                return new()
                {
                    Errors = ["New registrations are not allowed"]
                };

            if (!request.Failures.IsValid())
            {
                return Errored(request.Failures.ToFlashResult());
            }

            if (await _dataContext.Users.FindByEmail(request.Email).AnyAsync())
            {
                return Errored(ResultModels.FlashResult.WithError("The email address is already in use"));
            }

            var user = new Entities.User
            {
                FirstName = request.FirstName!,
                LastName = request.LastName!,
                Email = request.Email!,
                Password = _usersService.HashPassword(request.Password!),
                IsAdmin = true,
                IsActivated = true,
                StartDate = DateTime.Today,
            };

            var company = new Entities.Company
            {
                Name = request.CompanyName!,
                TimeZone = request.TimeZone!,
                Country = request.Country!,
                StartOfNewYear = 1,
                LeaveTypes = new[]
                {
                    new Entities.LeaveType
                    {
                        Name = "Holiday",
                        Colour = "leave_type_color_1"
                    },
                    new Entities.LeaveType
                    {
                        Name = "Sickness",
                        Colour = "leave_type_color_1",
                        UseAllowance = false,
                    },
                },
                Schedule = new(),
                Teams = new[]
                {
                    new Entities.Team
                    {
                        Name = "General",
                        Users = new[]
                        {
                            user,
                        },
                    },
                },
            };

            company.Users.Add(user);

            _dataContext.Companies.Add(company);

            var tx = _dataContext.BeginTransaction();
            try
            {
                await _dataContext.SaveChangesAsync();

                var email = _templateService.ConfirmRegistration(user);
                _dataContext.EmailAudits.Add(email);

                company.Teams.First().Manager = user;
                await _dataContext.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                tx.Dispose();

                return Errored(ResultModels.FlashResult.WithError("Unable to create company. A database error occurred"));
            }
            return new();
        }

        private ApiResult Errored(ResultModels.FlashResult errors)
        {
            return new()
            {
                Errors = errors.Errors,
            };
        }
    }
}