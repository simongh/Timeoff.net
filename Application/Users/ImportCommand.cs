using MediatR;

namespace Timeoff.Application.Users
{
    public record ImportCommand : IRequest<ResultModels.ApiResult>
    {
    }

    internal class ImportCommandHandler : IRequestHandler<ImportCommand, ResultModels.ApiResult>
    {
        public async Task<ResultModels.ApiResult> Handle(ImportCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}