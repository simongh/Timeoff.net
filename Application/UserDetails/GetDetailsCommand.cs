using MediatR;

namespace Timeoff.Application.UserDetails
{
    public record GetDetailsCommand : IRequest<DetailsViewModel>
    {
        public int Id { get; init; }
    }

    internal class GetUserDetailsCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<GetDetailsCommand, DetailsViewModel>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<DetailsViewModel> Handle(GetDetailsCommand request, CancellationToken cancellationToken)
        {
            var result = await _dataContext.GetUserDetailsAsync(request.Id)
                ?? throw new NotFoundException();
            return result;
        }
    }
}