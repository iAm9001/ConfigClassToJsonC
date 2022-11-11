namespace ConfigClassToJsonCTests.Configuration;

public class ConfigurationClassUndocumentedExample
{
       
    public string LocalPathSomeFile { get; set; }

      
    public bool Active { get; set; }

    public int UndocumentedNumber { get; set; }
        
       
    public Dictionary<string, string> SuperImportantDictionary { get; set; }
}