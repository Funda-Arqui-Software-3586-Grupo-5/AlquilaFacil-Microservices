namespace LocalManagement.Domain.Model.Commands;

public record CreateCommentCommand(int UserId, int LocalId, string Text, int Rating );