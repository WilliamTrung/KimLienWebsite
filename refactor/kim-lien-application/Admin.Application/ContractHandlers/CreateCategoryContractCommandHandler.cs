using Admin.Application.Abstractions;
using Admin.Application.Commands.Category;
using Admin.Contract.Commands;
using AutoMapper;
using MediatR;

namespace Admin.Application.ContractHandlers
{
    public class CreateCategoryContractCommandHandler(IMapper mapper, ICategoryService service) : IRequestHandler<CreateCategoryContractCommand>
    {
        public async Task Handle(CreateCategoryContractCommand request, CancellationToken cancellationToken)
        {
            var command = mapper.Map<CreateCategoryCommand>(request);
            await service.Create(command, cancellationToken);
        }
    }
}
