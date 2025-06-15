using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGlm.JContent.Segment;

[JsonDerivedType(typeof(TextSegment))]
[JsonDerivedType(typeof(ImageSegment))]
[JsonDerivedType(typeof(PythonCodeExecuteSegment))]
[JsonDerivedType(typeof(PythonExecutionSegment))]
[JsonDerivedType(typeof(PythonSywErrorSegment))]
[JsonConverter(typeof(SegmentConverter))]
public class BaseSegment
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

public class SegmentConverter : JsonConverter<BaseSegment>
{
    private readonly Dictionary<string, Type> _segmentTypes = 
        typeof(BaseSegment).Assembly
        .GetTypes()
        .Where(x => x.IsDefined(typeof(SegmentTypeAttribute), true))
        .Select(t => (type: t, attr :t.GetCustomAttribute<SegmentTypeAttribute>()!))
        .ToDictionary(t => t.attr.Type, t => t.type);

    public override BaseSegment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var jsonNode = jsonDoc.RootElement; // 避免二次解析

        string typeName = jsonNode.GetProperty("type").GetString() ?? "";
        if (_segmentTypes.TryGetValue(typeName, out Type? targetType))
        {
            // 关键：传递 options 并直接反序列化
            return jsonNode.Deserialize(targetType, options) as BaseSegment;
        }
        throw new JsonException($"Unknown segment type: {typeName}");
    }

    //Only deserialization is required, so no serialization is implemented
    public override void Write(Utf8JsonWriter writer, BaseSegment value, JsonSerializerOptions options)
    {
        
    }
}