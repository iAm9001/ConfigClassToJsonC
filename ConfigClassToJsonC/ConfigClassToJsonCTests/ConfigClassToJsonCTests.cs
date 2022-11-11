using System.Text.Encodings.Web;
using System.Text.Json;
using ConfigClassToJsonC;
using ConfigClassToJsonCTests.Configuration;

namespace ConfigClassToJsonCTests;

public class ConfigClassToJsonCTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(true, ExpectedResult = true, Author = "iAm9001",
        Description = "Tests a C# class that is known to have XMLDOC documentation.")]
    [TestCase(false, ExpectedResult = false, Author = "iAm9001",
        Description = "Tests a C# class that is known NOT to have XMLDOC documentation.")]
    public bool Test1(bool documentedClassTest)
    {
        bool serializedContentContainsComments = false;
        string serializedClassJson = null;

        if (documentedClassTest)
        {
            var configWithDocumentation = new ConfigurationClassDocumentedExample()
            {
                Active = true,
                UndocumentedNumber = 3,
                SuperImportantDictionary = new Dictionary<string, string>()
                    { { "Key1", "Val1" }, { "SuperImportantKey", "Maybe not so important" } },
                LocalPathSomeFile = "c:\\Somewhere\\Somehow.txt"
            };

            serializedClassJson = configWithDocumentation.SerializeObjectWithComments();

            TestContext.WriteLine(
                $"Serialized output of documented class {nameof(ConfigurationClassDocumentedExample)}:");
            TestContext.WriteLine(serializedClassJson);

            Assert.That(!string.IsNullOrWhiteSpace(serializedClassJson),
                "The serialized output should contain JSON, but it was null or whitespace.");
            Assert.DoesNotThrow(() =>
                JsonSerializer.Deserialize<ConfigurationClassDocumentedExample>(serializedClassJson,
                    new JsonSerializerOptions()
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }), "Expected the deserialization of the serialized JSON to succeed, but it did not.");

            Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<ConfigurationClassDocumentedExample>(serializedClassJson,
                    new JsonSerializerOptions()
                    {
                        ReadCommentHandling = JsonCommentHandling.Disallow,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }), "Expected the deserialization of the serialized JSON to FAIL, but it did not.");

            serializedContentContainsComments =
                serializedClassJson.Contains("/*") && serializedClassJson.Contains("*/");
        }
        else
        {
            var configWithoutDocumentation = new ConfigurationClassUndocumentedExample()
            {
                Active = true,
                UndocumentedNumber = 3,
                SuperImportantDictionary = new Dictionary<string, string>()
                    { { "Key1", "Val1" }, { "SuperImportantKey", "Maybe not so important" } },
                LocalPathSomeFile = "c:\\Somewhere\\Somehow.txt"
            };

            serializedClassJson = configWithoutDocumentation.SerializeObjectWithComments();
            TestContext.WriteLine(
                $"Serialized output of undocumented class {nameof(ConfigurationClassUndocumentedExample)}:");
            TestContext.WriteLine(serializedClassJson);

            Assert.That(!string.IsNullOrWhiteSpace(serializedClassJson),
                "The serialized output should contain JSON, but it was null or whitespace.");
            Assert.DoesNotThrow(() =>
                JsonSerializer.Deserialize<ConfigurationClassUndocumentedExample>(serializedClassJson,
                    new JsonSerializerOptions()
                    {
                        ReadCommentHandling = JsonCommentHandling.Disallow,
                        Encoder = JavaScriptEncoder.Default
                    }), "Expected the deserialization of the serialized JSON to succeed, but it did not.");

            serializedContentContainsComments =
                serializedClassJson.Contains("/*") && serializedClassJson.Contains("/*");
        }

        return serializedContentContainsComments;
    }
}