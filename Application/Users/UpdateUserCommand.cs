using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record UpdateUserCommand : UserDetailsModelBase, IRequest<UserDetailsViewModel?>, Commands.IValidated
    {
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDetailsViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateUserCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<UserDetailsViewModel?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();
            if (request.Failures != null)
            {
                errors.AddRange(request.Failures);
            }

            var deptValid = await _dataContext.Departments
                .Where(d => d.CompanyId == _currentUserService.CompanyId && d.DepartmentId == request.DepartmentId)
                .AnyAsync();
            if (!deptValid)
            {
                errors.Add(new(nameof(request.DepartmentId), "Department could not be found"));
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
                user.StartDate = request.StartDate;
                user.EndDate = request.EndDate;
                user.IsActivated = request.IsActive;
                user.IsAdmin = request.IsAdmin;
                user.AutoApprove = request.AutoApprove;
                user.DepartmentId = request.DepartmentId;

                await _dataContext.SaveChangesAsync();

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