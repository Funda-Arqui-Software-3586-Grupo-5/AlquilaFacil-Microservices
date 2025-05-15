using LocalManagement.Domain.Model.Entities;
using LocalManagement.Interfaces.REST.Resources;

namespace LocalManagement.Interfaces.REST.Transform;

public static class LocalCategoryResourceFromEntityAssembler
{
    public static LocalCategoryResource ToResourceFromEntity(LocalCategory localCategory)
    {
        return new LocalCategoryResource(
            localCategory.Id,
            localCategory.Name,
            localCategory.PhotoUrl
            );
    }
}
