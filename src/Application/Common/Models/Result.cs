namespace Application.Common.Models;

public class Result(bool succeeded, IEnumerable<string> errors)
{
    public bool Succeeded { get; init; } = succeeded;

    public string[] Errors { get; init; } = errors.ToArray();

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}