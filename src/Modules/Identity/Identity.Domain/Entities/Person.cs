namespace Identity.Domain.Entities;

public class Person : BaseEntity
{
    public string Identification { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}