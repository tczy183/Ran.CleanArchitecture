namespace Ran.Ddd.Domain.Abstractions.Tenant;

[Flags]
public enum MultiTenancySides 
{
    /// <summary>
    /// Tenant side.
    /// </summary>
    Tenant = 1,

    /// <summary>
    /// Host side.
    /// </summary>
    Host = 2,

    /// <summary>
    /// Both sides
    /// </summary>
    Both = Tenant | Host,
}
