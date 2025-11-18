using Common.Kernel.Models.Implementations;
using MediatR;

namespace Admin.Contract.Commands
{
    public class CreateCategoryContractCommand : IRequest
    {
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
        public List<AssetDto>? Pictures { get; set; }
    }
}
