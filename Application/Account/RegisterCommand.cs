using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Timeoff.Application.Account
{
    public record RegisterCommand : Types.RegisterModel, IRequest<RegisterViewModel?>, Commands.IValidated
    {
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterViewModel?>
    {
        private readonly Types.Options _options;
        private readonly IDataContext _dataContext;
        private readonly Services.IUsersService _usersService;
        private readonly Services.IEmailTemplateService _templateService;

        public RegisterCommandHandler(
            IOptions<Types.Options> options,
            IDataContext dataContext,
            Services.IUsersService usersService,
            Services.IEmailTemplateService templateService)
        {
            _options = options.Value;
            _dataContext = dataContext;
            _usersService = usersService;
            _templateService = templateService;
        }

        public async Task<RegisterViewModel?> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (!_options.AllowNewAccountCreation)
                return null;

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
                Departments = new[]
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

                company.Departments.First().Manager = user;
                await _dataContext.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                tx.Dispose();

                return Errored(ResultModels.FlashResult.WithError("Unable to create company. A database error occurred"));
            }
            return new()
            {
                Success = true,
            };
        }

        private RegisterViewModel Errored(ResultModels.FlashResult errors)
        {
            return new()
            {
                TimeZones = Services.TimeZoneService.TimeZones,
                Countries = Services.CountriesService.Countries,
                Result = errors,
            };
        }
    }
}