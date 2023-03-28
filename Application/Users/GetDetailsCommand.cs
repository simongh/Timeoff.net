using MediatR;

namespace Timeoff.Application.Users
{
    public record GetDetailsCommand : IRequest<DetailsViewModel>
    {
        public int Id { get; init; }
    }

    internal class GetUserDetailsCommandHandler : IRequestHandler<GetDetailsCommand, DetailsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetUserDetailsCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<DetailsViewModel> Handle(GetDetailsCommand request, CancellationToken cancellationToken)
        {
            var result = await _dataContext.GetUserDetailsAsync(_currentUserService.CompanyId, request.Id)
                ?? throw new NotFoundException();
            return result;
        }
    }
}