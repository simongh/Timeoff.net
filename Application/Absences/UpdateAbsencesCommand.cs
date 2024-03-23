using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Absences
{
    public record UpdateAbsencesCommand : IRequest<AbsencesViewModel>, Commands.IValidated
    {
        public int Id { get; set; }

        public double Adjustment { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateAbsencesCommandHandler : IRequestHandler<UpdateAbsencesCommand, AbsencesViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateAbsencesCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<AbsencesViewModel> Handle(UpdateAbsencesCommand request, CancellationToken cancellationToken)
        {
            if (!await _dataContext.Users
                .Where(u => u.UserId == request.Id && u.CompanyId == _currentUserService.CompanyId)
                .AnyAsync())
            {
                throw new NotFoundException();
            }

            var adj = await _dataContext.UserAllowanceAdjustments
                .Where(u => u.User.CompanyId == _currentUserService.CompanyId && u.UserId == request.Id)
                .Where(u => u.Year == DateTime.Today.Year)
                .FirstOrDefaultAsync();

            if (adj == null)
            {
                adj = new()
                {
                    UserId = request.Id,
                    Year = DateTime.Today.Year,
                };
                _dataContext.UserAllowanceAdjustments.Add(adj);
            }
            adj.Adjustment = (int)request.Adjustment;

            await _dataContext.SaveChangesAsync();

            var result = await _dataContext.GetAbsencesAync(_currentUserService.CompanyId, request.Id);
            result.Messages = ResultModels.FlashResult.Success("Adjustment updated");
            return result;
        }
    }
}