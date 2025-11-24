using MediatR;

namespace Admin.Contract.Commands
{
    public class InsertProductCategoryContractCommand : IRequest
    {
        public Guid ProductId { get; set; } // ID of the product to associate categories with
        public List<Guid> CategoryIds { get; set; } = null!; // List of associated category IDs
    }
}
