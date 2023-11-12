using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.UserDetails
{
    public record UpdateUserCommand : Types.UserDetailsModelBase, IRequest<DetailsViewModel?>, Commands.IValidated
    {
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, DetailsViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;
        private readonly Services.INewLeaveService _leaveService;

        public UpdateUserCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService,
            Services.INewLeaveService leaveService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
            _leaveService = leaveService;
        }

        public async Task<DetailsViewModel?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();
            if (request.Failures != null)
            {
                errors.AddRange(request.Failures);
            }

            var teamValid = await _dataContext.Teams
                .Where(d => d.CompanyId == _currentUserService.CompanyId && d.TeamId == request.TeamId)
                .AnyAsync();
            if (!teamValid)
            {
                errors.Add(new(nameof(request.TeamId), "Team could not be found"));
            }

            var user = await _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId)
                .Where(u => u.UserId == request.Id)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            else
            {
                if (!request.IsAdmin || !request.IsActive)
                {
                    var otherAdmins = await _dataContext.Users
                         .Where(u => u.CompanyId == _currentUserService.CompanyId)
                         .Where(u => u.IsAdmin && u.UserId != request.Id)
                         .AnyAsync();

                    if (!otherAdmins)
                        errors.Add(new(nameof(request.IsAdmin), "Cannot remove or deactivate the last admin"));
                }

                var emailUsed = await _dataContext.Users
                    .Where(u => u.Email == request.Email && u.UserId != user.UserId)
                    .AnyAsync();
                if (emailUsed)
                {
                    errors.Add(new(nameof(request.Email), "The email is already in use"));
                }
            }

            ResultModels.FlashResult messages;
            if (errors.Any())
            {
                messages = errors.ToFlashResult();
            }
            else
            {
                user!.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.StartDate = request.StartDate!.Value;
                user.EndDate = request.EndDate;
                user.IsActivated = request.IsActive;
                user.IsAdmin = request.IsAdmin;
                user.AutoApprove = request.AutoApprove;
                user.TeamId = request.TeamId;

                await _dataContext.SaveChangesAsync();
                _leaveService.ClearEmployeeCache();

                messages = ResultModels.FlashResult.Success($"Details for {request.Name} were updated");
            }

            var result = await _dataContext.GetUserDetailsAsync(_currentUserService.CompanyId, request.Id);
            if (result != null)
            {
                result.Messages = messages;
            }
            return result;
        }
    }
}