using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Routify.Data.Utils;

public class ValueComparers
{
    public static ValueComparer<Dictionary<string, string>> StringDictionary => new(
        (c1, c2) => StringDictionaryEquals(c1, c2),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToDictionary());
    
    private static bool StringDictionaryEquals(
        Dictionary<string, string>? c1,
        Dictionary<string, string>? c2)
    {
        if (c1 is null && c2 is null)
            return true;

        if (c1 is null || c2 is null)
            return false;

        return c1.SequenceEqual(c2);
    }
}