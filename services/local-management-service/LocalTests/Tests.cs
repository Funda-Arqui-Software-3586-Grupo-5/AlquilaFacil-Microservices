using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Commands;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using Moq;

namespace LocalTests;

public class Tests
{
    [Test]
    public void Create_Local_Must_To_Be_Working()
    {
        //Arrange
        var commandService = new Mock<ILocalCommandService>();
        var command = new CreateLocalCommand(
            "Test Local", 
            "Test Address", 
            "12345",
            "Test City",
            "Test State",
            100,
            "Test Phone",
            "Test Email",
            1,
            1,
            "Test Features",
            20
            );
        commandService.Setup(x => x.Handle(command));
        //Act
        commandService.Object.Handle(command);
        //Assert
        commandService.Verify(x => x.Handle(command),Times.Once);
    }

    [Test]
    public void Get_Local_By_Filter_Must_To_Be_Working()
    {
        //Arrange
        var queryService = new Mock<ILocalQueryService>();
        var query = new GetLocalsByCategoryIdAndCapacityRangeQuery(1, 10, 20);
        queryService.Setup(x => x.Handle(query));
        //Act
        queryService.Object.Handle(query);
        //Assert
        queryService.Verify(x => x.Handle(query), Times.Once);
    }
    
    [Test]
    public void Get_Local_By_Id_Must_To_Be_Working()
    {
        //Arrange
        var queryService = new Mock<ILocalQueryService>();
        var query = new GetLocalByIdQuery(1);
        queryService.Setup(x => x.Handle(query));
        //Act
        queryService.Object.Handle(query);
        //Assert
        queryService.Verify(x => x.Handle(query), Times.Once);
    }
    
    [Test]
    public void Create_Report_Must_To_Be_Working()
    {
        //Arrange
        var commandService = new Mock<IReportCommandService>();
        var command = new CreateReportCommand(
            1,
            "Test title",
            1,
            "Test Description"
        );
        commandService.Setup(x => x.Handle(command));
        //Act
        commandService.Object.Handle(command);
        //Assert
        commandService.Verify(x => x.Handle(command), Times.Once);
    }
    
    
    [Test]
    public void Create_Comment_Must_To_Be_Working()
    {
        //Arrange
        var commandService = new Mock<ICommentCommandService>();
        var command = new CreateCommentCommand(
            1,
            1,
            "Test Comment",
            5
        );
        commandService.Setup(x => x.Handle(command));
        //Act
        commandService.Object.Handle(command);
        //Assert
        commandService.Verify(x => x.Handle(command), Times.Once);
    }
    
}