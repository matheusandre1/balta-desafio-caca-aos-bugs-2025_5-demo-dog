using System.ComponentModel.DataAnnotations;

namespace BugStore.Domain.Entities;

public class Customer
{
    [Key]
    public Guid Id { get;  set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }

    public void UpdateWith(string? name, string? email, string? phone, DateTime? birthDate)
    {
        if (name != null) Name = name;
        if (email != null) Email = email;
        if (phone != null) Phone = phone;
        if (birthDate.HasValue) BirthDate = birthDate.Value;
    }
}