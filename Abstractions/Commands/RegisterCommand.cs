using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Timeoff.Commands
{
    public record RegisterCommand : Types.RegisterModel, IRequest<ResultModels.RegisterViewModel?>
    {
    }

    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResultModels.RegisterViewModel?>
    {
        private readonly Types.Options _options;
        private readonly IDataContext _dataContext;
        private readonly Services.IUsersService _usersService;

        public RegisterCommandHandler(
            IOptions<Types.Options> options,
            IDataContext dataContext,
            Services.IUsersService usersService)
        {
            _options = options.Value;
            _dataContext = dataContext;
            _usersService = usersService;
        }

        public async Task<ResultModels.RegisterViewModel?> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (!_options.AllowNewAccountCreation)
                return null;

            if (await _dataContext.Users.FindByEmail(request.Email).AnyAsync())
            {
                return new()
                {
                    TimeZones = Services.TimeZoneService.TimeZones,
                    Countries = Services.CountriesService.Countries,
                    Result = new()
                    {
                        Errors = new[]
                        {
                            "The email address is already in use",
                        },
                    },
                };
            }

            var user = new Entities.User
            {
                Name = request.FirstName!,
                LastName = request.LastName!,
                Email = request.Email!,
                Password = _usersService.HashPassword(request.Password!),
                Admin = true,
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
                        Colour = "#22AA66"
                    },
                    new Entities.LeaveType
                    {
                        Name = "Sickness",
                        Colour = "#459FF3",
                        UseAllowance = false,
                    },
                },
                Schedule = new(),
                Departments = new[]
                {
                    new Entities.Department
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
            await _dataContext.SaveChangesAsync();

            company.Departments.First().Manager = user;
            await _dataContext.SaveChangesAsync();

            return new()
            {
                Success = true,
            };
        }
    }
}