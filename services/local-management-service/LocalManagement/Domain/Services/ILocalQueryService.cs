using LocalManagement.Domain.Model.Queries;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Domain.Services;

public interface ILocalQueryService
{
    Task<IEnumerable<Local>> Handle(GetAllLocalsQuery query);
    Task<Local?> Handle(GetLocalByIdQuery query);
    HashSet<string> Handle(GetAllLocalDistrictsQuery query);
    
    Task<IEnumerable<Local>> Handle(GetLocalsByUserIdQuery query);
    Task<IEnumerable<Local>> Handle(GetLocalsByCategoryIdAndCapacityRangeQuery query);

    Task<bool> Handle(IsLocalOwnerQuery query);


}