using IAM.Domain.Model.Aggregates;
using IAM.Domain.Model.Queries;
using IAM.Domain.Respositories;
using IAM.Domain.Services;

namespace IAM.Application.Internal.QueryServices;

public class UserQueryService (IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await userRepository.ListAsync();
    }
    
    public async Task<User?> Handle(GetUserByEmailQuery query)
    {
        return await userRepository.FindByEmailAsync(query.Email);
    }

    public async Task<string?> Handle(GetUsernameByIdQuery query)
    {
        return await userRepository.GetUsernameByIdAsync(query.UserId);
    }

    public bool Handle(UserExistsQuery query)
    {
        return userRepository.ExistsById(query.UserId);
    }
}