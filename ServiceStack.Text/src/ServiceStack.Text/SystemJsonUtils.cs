#if NET8_0_OR_GREATER

#nullable enable
using System.Collections.Generic;
using System.Text.Json;

namespace ServiceStack.Text;

public class SystemJsonUtils
{
    public static object? AsObject(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                // For numbers, try to parse as different numeric types
                if (element.TryGetInt32(out int intValue))
                    return intValue;
                if (element.TryGetInt64(out long longValue))
                    return longValue;
                if (element.TryGetDouble(out double doubleValue))
                    return doubleValue;
                return element.GetRawText(); // Fallback
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
                return null;
            case JsonValueKind.Object:
                // For objects, create a Dictionary
                var obj = new Dictionary<string, object?>();
                foreach (var property in element.EnumerateObject())
                {
                    obj[property.Name] = AsObject(property.Value);
                }
                return obj;
            case JsonValueKind.Array:
                // For arrays, create a List
                var array = new List<object?>();
                foreach (var item in element.EnumerateArray())
                {
                    array.Add(AsObject(item));
                }
                return array;
            default:
                return element.GetRawText();
        }
    }    
}

#endif
