using System.ComponentModel.DataAnnotations;

namespace Ran.Core.Ran.Exceptions;

public interface IHasValidationErrors
{
    IList<ValidationResult> ValidationErrors { get; }
}
