using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Common.Application.ProductViewService.Commands
{
    public class SetProductViewCommand : IRequest
    {
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
        public DbContext DbContext { get; set; } = null!;
    }
}
