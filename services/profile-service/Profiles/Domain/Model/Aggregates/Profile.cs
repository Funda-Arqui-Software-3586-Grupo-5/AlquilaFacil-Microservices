using AlquilaFacilPlatform.Profiles.Domain.Model.Commands;
using AlquilaFacilPlatform.Profiles.Domain.Model.ValueObjects;

namespace Profiles.Domain.Model.Aggregates;

public partial class Profile
{
    public Profile()
    {
        Name = new PersonName();
        Birth = new DateOfBirth();
        PhoneN = new Phone();
        DocumentN = new DocumentNumber();
        UserId = 0;
        PhotoUrl = string.Empty;
    }
    
    public Profile(CreateProfileCommand command)
    {
        Name = new PersonName(command.Name, command.FatherName, command.MotherName);
        Birth = new DateOfBirth(command.DateOfBirth);
        PhoneN = new Phone(command.Phone);
        DocumentN = new DocumentNumber(command.DocumentNumber);
        UserId = command.UserId;
        PhotoUrl = command.PhotoUrl;
    }
    
    public void Update(UpdateProfileCommand command)
    {
        Name = new PersonName(command.Name, command.FatherName, command.MotherName);
        Birth = new DateOfBirth(command.DateOfBirth);
        PhoneN = new Phone(command.Phone);
        DocumentN = new DocumentNumber(command.DocumentNumber);
        PhotoUrl = command.PhotoUrl;
    }

    public int Id { get; }

    public string PhotoUrl { get; set; }
    public PersonName Name { get; private set; }
    public DateOfBirth Birth { get; private set; }
    public Phone PhoneN { get; private set; }
    public DocumentNumber DocumentN { get; private set; }
    

    //public User User { get; internal set; }
    //public int UserId { get; internal set; }
    
    public int UserId { get; set; }

    public string FullName => Name.FullName;
    public string BirthDate => Birth.BirthDate;
    public string PhoneNumber => PhoneN.PhoneNumber;
    public string NumberDocument => DocumentN.NumberDocument;
    
}