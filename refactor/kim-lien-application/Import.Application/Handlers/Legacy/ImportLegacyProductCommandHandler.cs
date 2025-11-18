using Import.Application.Commands.Legacy;
using Legacy.Contract.Commands;
using MediatR;

namespace Import.Application.Handlers.Legacy
{
    public class ImportLegacyProductCommandHandler(ISender sender) : IRequestHandler<ImportLegacyProductCommand>
    {
        public async Task Handle(ImportLegacyProductCommand request, CancellationToken cancellationToken)
        {
            var fetchCommand = new FetchProductCommand();
            var data = await sender.Send(fetchCommand, cancellationToken);
            foreach (var item in data)
            {
                var images = item.Pictures?.Split(",").ToList();
                var assets = images?.Select(url =>
                {
                    var cleanUrl = url.Split('?')[0]; // remove query params if exist

                    return new Common.Kernel.Models.Implementations.AssetDto
                    {
                        Url = url,
                        Name = Path.GetFileNameWithoutExtension(cleanUrl),
                        CreatedAt = DateTime.UtcNow,
                    };
                }).ToList();
                var adminCommand = new Admin.Contract.Commands.CreateProductContractCommand
                {
                    Id = item.Id,
                    Name = item.Name ?? string.Empty,
                    Description = item.Description ?? string.Empty,
                    Images = assets!,
                };
                await sender.Send(adminCommand, cancellationToken);
            }
        }
    }
}
