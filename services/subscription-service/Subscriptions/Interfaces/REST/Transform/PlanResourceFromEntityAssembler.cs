using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Interfaces.REST.Resources;

namespace Subscriptions.Interfaces.REST.Transform;

public static class PlanResourceFromEntityAssembler
{
    public static PlanResource ToResourceFromEntity(Plan entity)
    {
        return new PlanResource(entity.Id, entity.Name, entity.Service, entity.Price);
    }

}