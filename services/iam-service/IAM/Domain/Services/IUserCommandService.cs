using IAM.Domain.Model.Aggregates;
using IAM.Domain.Model.Commands;

namespace IAM.Domain.Services;

public interface IUserCommandService
{
    Task<(User user, string token)> Handle(SignInCommand command);
    Task<User?> Handle(SignUpCommand command);
    Task<User?> Handle(UpdateUsernameCommand command);
    
}