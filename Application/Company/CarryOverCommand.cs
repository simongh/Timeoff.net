using MediatR;

namespace Timeoff.Application.Company
{
    public record CarryOverCommand : IRequest
    {
    }

    internal class CarryOverCommandHandler : IRequestHandler<CarryOverCommand>
    {
        public Task Handle(CarryOverCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}