using MediatR;

namespace Timeoff.Application.Company
{
    public record BackupCommand : IRequest
    {
    }

    internal class BackupCommandHandler : IRequestHandler<BackupCommand>
    {
        public Task Handle(BackupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}