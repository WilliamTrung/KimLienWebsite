using Authen.Application.Models;
using MediatR;

namespace Authen.Application.Commands
{
    public class LoginCommand : LoginDto, IRequest<TokenPair>
    {
    }
}
