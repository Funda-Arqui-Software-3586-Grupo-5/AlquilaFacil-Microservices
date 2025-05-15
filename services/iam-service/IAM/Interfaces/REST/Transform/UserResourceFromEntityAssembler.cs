using System.Diagnostics;
using IAM.Domain.Model.Aggregates;
using IAM.Interfaces.REST.Resources;

namespace IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User? user)
    {
        Console.WriteLine("User Name is " + user?.Username);
        Debug.Assert(user != null, nameof(user) + " != null");
        return new UserResource(user.Id, user.Username);
    }
}