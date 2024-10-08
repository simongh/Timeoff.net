using CsvHelper;
using MediatR;
using System.Globalization;

namespace Timeoff.Application.Users
{
    public record SampleCommand : IRequest<Stream>
    { }

    internal class SampleCommandHandler : IRequestHandler<SampleCommand, Stream>
    {
        public async Task<Stream> Handle(SampleCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, leaveOpen: true);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<ImportMap>();

            csv.WriteHeader<ImportModel>();
            await csv.NextRecordAsync();

            await writer.FlushAsync();

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}