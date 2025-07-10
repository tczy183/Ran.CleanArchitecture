namespace Ran.Ddd.Domain.Abstraction.Entities;

/// <summary>
/// Defines an interface to add a concurrency stamp to a class.
/// </summary>
public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}
