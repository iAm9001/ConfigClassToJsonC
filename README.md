# ConfigClassToJsonC
This library can be used to take any C# class (but specifically with configuration style classes in mind) and export it to a JSON string, along with it's XmlDoc output to the JSON file along with it. This is useful for creating ad-hoc documentation for your config files for when other people need to make modifications to them.

## Documented C# Class Example
This C# class will be used for demonstration purposes. Two examples below demonstrate how an object
of this type is serialized using the standard techniques, and how it is serialized when using the
library's extension method.
```C#
/// <summary>
/// This configuration class represents all of the required properties to create
/// this super example configuration object, along with it's documentation (for some things).
/// </summary>
public class ConfigurationClassDocumentedExample
{
    /// <summary>
    /// Gets or sets the local directory for some file.
    /// </summary>
    /// <remarks>
    /// This must be the ENTIRE path, not just the file name.
    /// </remarks>
    /// <example>c:\LocalFolder\somefile.txt </example>
    public string LocalPathSomeFile { get; set; }

    /// <summary>
    /// Gets or sets whether or not this is active. Whatever that means.
    /// </summary>
    public bool Active { get; set; }

    public int UndocumentedNumber { get; set; }
        
    /// <summary>
    /// Gets or sets this super important dictionary. It's a dictionary of key type string, value type string!
    /// </summary>
    /// <remarks>
    /// Extra comments here
    /// </remarks>
    /// <example> "Key" : "Value" </example>
    public Dictionary<string, string> SuperImportantDictionary { get; set; }
}
```

#### Old Way:
##### Serialized using this standard technique
```C#
var configWithDocumentation = new ConfigurationClassDocumentedExample()
            {
                Active = true,
                UndocumentedNumber = 3,
                SuperImportantDictionary = new Dictionary<string, string>()
                    { { "Key1", "Val1" }, { "SuperImportantKey", "Maybe not so important" } },
                LocalPathSomeFile = "c:\\Somewhere\\Somehow.txt"
            };
            
var options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
string jsonOutput = JsonSerializer.Serialize(configWithDocumentation, options);
```
##### Output JSON String (notice it is not very helpful in describing the property meanings)
```JSON
{
  "ConfigurationClassUndocumentedExample": {
    "LocalPathSomeFile": "c:\\Somewhere\\Somehow.txt",
    "Active": true,
    "UndocumentedNumber": 3,
    "SuperImportantDictionary": {"Key1":"Val1","SuperImportantKey":"Maybe not so important"}
  }
}
```

#### New Way (serializing to JSONC (JSON with Comments))
##### Serialized using the SerializeWithComments extension method technique
```C#
var configWithDocumentation = new ConfigurationClassDocumentedExample()
            {
                Active = true,
                UndocumentedNumber = 3,
                SuperImportantDictionary = new Dictionary<string, string>()
                    { { "Key1", "Val1" }, { "SuperImportantKey", "Maybe not so important" } },
                LocalPathSomeFile = "c:\\Somewhere\\Somehow.txt"
            };
string jsonOutput = configWithDocumentation.SerializeObjectWithComments();
```
##### Output JSONC String (yes, I know it isn't real JSON, it has comments, but this is perfectly acceptable for .NET configuration files!)
```JSONC
{
  /*This configuration class represents all of the required properties to create
this super example configuration object, along with it's documentation (for some things).
*/
  "ConfigurationClassDocumentedExample": {
    /*Gets or sets the local directory for some file.
Remarks: This must be the ENTIRE path, not just the file name.
Examples: c:\LocalFolder\somefile.txt 
*/
    "LocalPathSomeFile": "c:\\Somewhere\\Somehow.txt"
    /*Gets or sets whether or not this is active. Whatever that means.
*/,
    "Active": true,
    "UndocumentedNumber": 3
    /*Gets or sets this super important dictionary. It's a dictionary of key type string, value type string!
Remarks: Extra comments here
Examples: "Key" : "Value" 
*/,
    "SuperImportantDictionary": {"Key1":"Val1","SuperImportantKey":"Maybe not so important"}
  }
}
```
