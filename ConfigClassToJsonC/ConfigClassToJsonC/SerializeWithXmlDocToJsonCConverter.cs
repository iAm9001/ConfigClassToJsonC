#nullable enable
using System;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Namotion.Reflection;

namespace ConfigClassToJsonC
{

    /// <summary>
    /// The <see cref="SerializeWithXmlDocToJsonCConverter{T}"/> converter intercepts
    /// the serialization of a class and it's properties, extracts any XMLDOC summary
    /// documentation and serializes it overtop of the properties in the serialized
    /// output. If there are no source comments, no comment will be serialized.
    /// </summary>
    internal class SerializeWithXmlDocToJsonCConverter<T> : JsonConverter<T> where T : class, new()
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes to the JSON string with JSON comments appended, if applicable.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            Type valType = value.GetType();
            var valprops = valType.GetProperties();
            string classConfigComment = GenerateClassComments(valType);

            writer.WriteStartObject();
            if (!string.IsNullOrWhiteSpace(classConfigComment)) writer.WriteCommentValue(classConfigComment);
            writer.WriteStartObject(valType.Name);

            foreach (var p in valprops)
            {
                var pName = p.Name;
                var pVal = p.GetValue(value);
                string propComments = GeneratePropertyComments(p);
                if (!string.IsNullOrWhiteSpace(propComments)) writer.WriteCommentValue(propComments);
                string pValSerialized = JsonSerializer.Serialize(pVal,
                    new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                writer.WritePropertyName(pName);
                writer.WriteRawValue(pValSerialized);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();

        }

        /// <summary>
        /// Generates class comments based on XML summary documentation
        /// comments present on the type of the passed in type parameter.
        /// </summary>
        /// <param name="valType"></param>
        /// <returns></returns>
        internal string GenerateClassComments(Type valType)
        {
            string classConfigComment;
            string valTypeSummary = valType.GetXmlDocsSummary();
            string valTypesRemarks = valType.GetXmlDocsRemarks();
            string valTypesExamples = valType.GetXmlDocsTag("example");
            StringBuilder classCommentsSb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(valTypeSummary)) classCommentsSb.AppendLine(valTypeSummary);
            if (!string.IsNullOrWhiteSpace(valTypesRemarks)) classCommentsSb.AppendLine($"Remarks: {valTypesRemarks}");
            if (!string.IsNullOrWhiteSpace(valTypesExamples))
                classCommentsSb.AppendLine($"Examples: {valTypesExamples}");

            classConfigComment = classCommentsSb.ToString();
            return classConfigComment;
        }

        /// <summary>
        /// Generates property comments based on XML summary documentation
        /// comments present on the type of the passed in property parameter.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        internal string GeneratePropertyComments(PropertyInfo property)
        {
            string propertyComments;
            string propTypeSummary = property.GetXmlDocsSummary();
            string propTypeRemarks = property.GetXmlDocsRemarks();
            string propTypeExamples = property.GetXmlDocsTag("example");
            StringBuilder propCommentsSb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(propTypeSummary)) propCommentsSb.AppendLine(propTypeSummary);
            if (!string.IsNullOrWhiteSpace(propTypeRemarks)) propCommentsSb.AppendLine($"Remarks: {propTypeRemarks}");
            if (!string.IsNullOrWhiteSpace(propTypeExamples))
                propCommentsSb.AppendLine($"Examples: {propTypeExamples}");

            propertyComments = propCommentsSb.ToString();
            return propertyComments;
        }
    }
}