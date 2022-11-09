using System.Text.Encodings.Web;
using System.Text.Json;

namespace ConfigsClassXmlDocToJsonComments;

/// <summary>
/// The <see cref="ConfigsClassXmlDocToJsonComments"/> class is a simple
/// extension methods class that allows you to serialize a .NET class (along
/// with it's XMLDOC source comments, including those on it's properties)
/// to JSONC (JSON With Comments).
/// </summary>
public static class ConfigsClassXmlDocToJsonComments
{
    /// <summary>
    /// Serializes the passed in object to JSON with comments. If the type of the target
    /// object contains XMLDOC summary tags, they will be included in the JSONC output.
    /// </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string SerializeObjectWithComments<T>(this T target) where T : class, new()
    {
        var options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new SerializeWithXmlDocToJsonCConverter<T>() }
        };

        string jsonString = JsonSerializer.Serialize(target, options);
        jsonString = jsonString.Replace("*/{", "*/");

        return jsonString;
    }
}