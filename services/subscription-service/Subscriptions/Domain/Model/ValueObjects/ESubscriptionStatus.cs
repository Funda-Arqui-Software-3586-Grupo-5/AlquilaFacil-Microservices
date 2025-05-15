namespace Subscriptions.Domain.Model.ValueObjects;

public enum ESubscriptionStatus
{
    Active,
    Pending,
    Expired,
    Cancelled,
    Suspended,
    Trial,
    RenewalDue
}