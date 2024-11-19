namespace Ran.Core.Ran.Data;

public class ExtraPropertyDictionary : Dictionary<string, object?>
{
    public ExtraPropertyDictionary() { }

    public ExtraPropertyDictionary(IDictionary<string, object?> dictionary)
        : base(dictionary) { }
}
