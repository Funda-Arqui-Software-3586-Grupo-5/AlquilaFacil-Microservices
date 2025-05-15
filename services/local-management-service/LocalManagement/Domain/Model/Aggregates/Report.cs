using LocalManagement.Domain.Model.Commands;

namespace LocalManagement.Domain.Model.Aggregates;

public partial class Report
{
    public int Id { get; }
    public int LocalId { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}


public partial class Report
{
    public Report()
    {
        LocalId = 0;
        Title = string.Empty;
        UserId = 0;
        Description = string.Empty;
    }

    public Report(CreateReportCommand command)
    {
        LocalId = command.LocalId;
        Title = command.Title;
        UserId = command.UserId;
        Description = command.Description;
        CreatedAt = DateTime.Now;
    }
    
    
}