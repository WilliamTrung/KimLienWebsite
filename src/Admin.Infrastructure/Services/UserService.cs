using System.Linq.Expressions;
using Admin.Application.Abstractions;
using Admin.Application.Commands.User;
using Admin.Application.Models.User;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.Infrastructure.Pagination;
using Common.Kernel.Dependencies;
using Common.Kernel.Response.Pagination;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Services
{
    public class UserService : PaginationServiceBase<User, QueryUserCommand, UserDto>, IUserService, IScoped
    {
        private readonly AdminDbContext _dbContext;
        private readonly DbSet<User> _users;
        private readonly UserManager<User> _userManager;
        public UserService(IMapper mapper, 
            AdminDbContext dbContext,
            UserManager<User> userManager) : base(mapper, dbContext)
        {
            _userManager = userManager;
            _users = dbContext.Users;
            _dbContext = dbContext;
        }

        public async Task CreateUser(CreateUserCommand request, CancellationToken ct)
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName ?? request.Email,
                PhoneNumber = request.PhoneNumber,
                Region = request.Region
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public Task Delete(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetDetail(GetDetailUserCommand request, CancellationToken ct)
        {
            var query = PredicateBuilder.New<User>();
            if (Guid.TryParse(request.Value, out var guid))
            {
                query = query.Or(u => u.Id == guid);
            }
            query = query.Or(u => u.Email == request.Value);
            var user = await _users.AsNoTracking()
                .Where(query)
                .FirstOrDefaultAsync(ct) ?? throw new KeyNotFoundException("user_not_found");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<PaginationResponse<UserDto>> GetPaginationResponse(QueryUserCommand request, CancellationToken ct)
        {
            return await ToPaginationResponse(request, BuildQuery);
        }

        public async Task ModifyUser(ModifyUserCommand request, CancellationToken ct)
        {
            if(Guid.TryParse(request.Id, out var id))
            {
                var user = _users.Where(u => u.Id == id).FirstOrDefault();
                if (user is null)
                    throw new KeyNotFoundException("user_not_found");
                user.DisplayName = request.DisplayName ?? user.DisplayName;
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                user.Region = request.Region ?? user.Region;
                _users.Update(user);
                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
                    if (!result.Succeeded)
                        throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
                }
                await _dbContext.SaveChangesAsync(ct);
                return;
            }
            throw new ArgumentException("invalid_user_id");
        }

        private static Expression<Func<User, bool>> BuildQuery(QueryUserCommand request)
        {
            var query = PredicateBuilder.New<User>(true);
            if (request.Filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(request.Filter.Email))
                    query = query.And(u => !string.IsNullOrWhiteSpace(u.Email) && u.Email.Contains(request.Filter.Email));
                if (!string.IsNullOrWhiteSpace(request.Filter.DisplayName))
                {
                    var nameQuery = request.Filter.DisplayName.Trim().ToLower();
                    query = query.And(u => u.DisplayName != null &&
                    EF.Functions.ILike(
                                EF.Functions.Unaccent(u.DisplayName).ToLower().Trim(),
                                EF.Functions.Unaccent($"%{nameQuery}%")));
                }
                if (!string.IsNullOrWhiteSpace(request.Filter.Region))
                    query = query.And(u => u.Region == request.Filter.Region);
            }
            return query;
        }
    }
}
