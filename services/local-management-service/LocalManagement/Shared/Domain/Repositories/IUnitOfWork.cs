namespace LocalManagement.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}