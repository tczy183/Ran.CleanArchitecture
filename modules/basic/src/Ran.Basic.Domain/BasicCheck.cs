namespace Ran.Basic;

internal static class BasicCheck
{
    public static string Required(string value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", parameterName);
        }
        if (value.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                $"Value cannot be longer than {maxLength} characters."
            );
        }

        return value.Trim();
    }
}
