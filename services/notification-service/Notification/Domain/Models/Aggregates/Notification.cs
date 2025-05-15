using Notification.Domain.Models.Commands;

namespace Notification.Domain.Models.Aggregates;

public partial class Notification
{
    public int Id { get; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
}

public partial class Notification
{
    public Notification()
    {
        Title = string.Empty;
        Description = string.Empty;
        UserId = 0;
    }

    public Notification(CreateNotificationCommand command)
    {
        Title = command.Title;
        Description = command.Description;
        UserId = command.UserId;
    }
}