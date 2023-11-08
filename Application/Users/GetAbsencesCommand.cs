using MediatR;

namespace Timeoff.Application.Users
{
    public record GetAbsencesCommand : IRequest<AbsencesViewModel>
    {
        public int Id { get; init; }
    }

    internal class GetAbsencesCommandHandler : IRequestHandler<GetAbsencesCommand, AbsencesViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetAbsencesCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<AbsencesViewModel> Handle(GetAbsencesCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetAbsencesAync(_currentUserService.CompanyId, request.Id);
        }
    }
}