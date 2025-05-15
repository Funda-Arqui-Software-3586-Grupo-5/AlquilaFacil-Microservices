using IAM.Application.Internal.OutboundServices;
using IAM.Domain.Model.Aggregates;
using IAM.Domain.Model.Commands;
using IAM.Domain.Respositories;
using IAM.Domain.Services;
using IAM.Infrastructure.Hashing.BCrypt.Services;
//using Profiles.Domain.Model.Aggregates;
//using Profiles.Domain.Repositories;
//using Profiles.Infrastructure.Persistence.EFC.Repositories;
using IAM.Shared.Domain.Repositories;

namespace IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash) ||
            !command.Email.Contains('@'))
            throw new Exception("Invalid email or password");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    public async Task<User?> Handle(SignUpCommand command)
    {
        const string symbols = "!@#$%^&*()_-+=[{]};:>|./?";
        if (command.Password.Length < 8 || !command.Password.Any(char.IsDigit) || !command.Password.Any(char.IsUpper) ||
            !command.Password.Any(char.IsLower) || !command.Password.Any(c => symbols.Contains(c)))
            throw new Exception(
                "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character");
        if(!command.Email.Contains('@'))
            throw new Exception("Invalid email address");

        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashedPassword, command.Email);
        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync(); // Save the user first
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }


        return user;
    }

    public async Task<User?> Handle(UpdateUsernameCommand command)
    {
        var userToUpdate = await userRepository.FindByIdAsync(command.Id);
        if (userToUpdate == null)
            throw new Exception("User not found");
        var userExists = userRepository.ExistsByUsername(command.Username);
        if (userExists)
        {
            throw new Exception("This username already exists");
        }

        userToUpdate.UpdateUsername(command.Username);
        await unitOfWork.CompleteAsync();
        return userToUpdate;
    }
    
}

