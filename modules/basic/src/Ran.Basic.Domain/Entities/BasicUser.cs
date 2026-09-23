using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Basic.Entities;

public sealed class BasicUser : AggregateRoot<Guid>
{
    private BasicUser() { }

    public BasicUser(Guid id, string userName, string displayName, string email)
    {
        Id = id;
        SetProfile(userName, displayName, email);
        SecurityStamp = Guid.NewGuid().ToString("N");
    }

    public string UserName { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string? PhoneNumber { get; private set; }

    public string? PasswordHash { get; private set; }

    public string SecurityStamp { get; private set; } = string.Empty;

    public Guid? DepartmentId { get; private set; }

    public bool IsActive { get; private set; } = true;

    public void SetProfile(string userName, string displayName, string email)
    {
        UserName = BasicCheck.Required(userName, 64, nameof(userName));
        DisplayName = BasicCheck.Required(displayName, 128, nameof(displayName));
        Email = BasicCheck.Required(email, 256, nameof(email));
    }

    public void SetPhoneNumber(string? phoneNumber)
    {
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber)
            ? null
            : BasicCheck.Required(phoneNumber, 32, nameof(phoneNumber));
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = BasicCheck.Required(passwordHash, 512, nameof(passwordHash));
        SecurityStamp = Guid.NewGuid().ToString("N");
    }

    public void SetDepartment(Guid? departmentId) => DepartmentId = departmentId;

    public void SetActive(bool isActive) => IsActive = isActive;
}
