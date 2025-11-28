using Common.Kernel.Models.Implementations;
using MediatR;

namespace Admin.Contract.Commands
{
    public class CreateProductContractCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Guid> CategoryIds { get; set; } = null!; // List of associated category IDs
        public List<AssetDto> Images { get; set; } = null!;
    }
}
