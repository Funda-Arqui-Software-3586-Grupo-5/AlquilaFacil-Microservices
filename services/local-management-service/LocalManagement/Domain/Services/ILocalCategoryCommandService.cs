using LocalManagement.Domain.Model.Commands;

namespace LocalManagement.Domain.Services;

public interface ILocalCategoryCommandService
{
    Task Handle(SeedLocalCategoriesCommand command);
}