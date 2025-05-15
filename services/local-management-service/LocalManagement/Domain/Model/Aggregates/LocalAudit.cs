using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace LocalManagement.Domain.Model.Aggregates;

public partial class Local : IEntityWithCreatedUpdatedDate
{
    [Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
    
    [Column("UpdateAt")] public DateTimeOffset? UpdatedDate { get; set; }
}