namespace ConfigClassToJsonCTests.Configuration;

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