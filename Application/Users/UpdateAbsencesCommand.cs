using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record UpdateAbsencesCommand : IRequest<ResultModels.AllowanceSummaryResult>, Commands.IValidated
    {
        public int Id { get; set; }

        public double Adjustment { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateAbsencesCommandHandler(IDataContext dataContext)
        : IRequestHandler<UpdateAbsencesCommand, ResultModels.AllowanceSummaryResult>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<ResultModels.AllowanceSummaryResult> Handle(UpdateAbsencesCommand request, CancellationToken cancellationToken)
        {
            if (!await _dataContext.Users
                .Where(u => u.UserId == request.Id)
                .AnyAsync())
            {
                throw new NotFoundException();
            }

            var adj = await _dataContext.UserAllowanceAdjustments
                .Where(u => u.UserId == request.Id)
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

            return await _dataContext.GetAllowanceAsync(request.Id, DateTime.Today.Year);
        }
    }
}