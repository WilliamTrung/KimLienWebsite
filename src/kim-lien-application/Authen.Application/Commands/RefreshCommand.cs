using Authen.Application.Models;
using MediatR;

namespace Authen.Application.Commands
{
    public class RefreshCommand : RefreshDto, IRequest<TokenPair>
    {
    }
}
