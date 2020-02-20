using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ferrum.Core.Extensions
{
    public static class EnumSerialzationExtensions
    {
        /// <summary>
        /// Add serializers for each of the Enum types in the Ferrum.Core.Enums.Serializable namespace.
        /// </summary>
        /// <param name="jsonConverters"></param>
        public static void AddEnumSerializers(this IList<JsonConverter> jsonConverters)
        {
            var types = Assembly.GetAssembly(typeof(EnumJsonConverter<>)).GetTypes()
                .Where(t => t.FullName.StartsWith("Ferrum.Core.Enums.Serializable"))
                .Where(t => t.BaseType == typeof(Enum));
            
            foreach(var type in types)
            {
                var converterType = typeof(EnumJsonConverter<>);
                var constructedType = converterType.MakeGenericType(type);
                var instance = Activator.CreateInstance(constructedType) as JsonConverter;
                jsonConverters.Add(instance);
            }                
        }
    }

    /// <summary>
    /// Default Enum serializer to read/write string representation of enum members instead of underlying numeric values.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var strValue = reader.GetString();
            var success = Enum.TryParse<TEnum>(strValue, out var result);
            return success ? result : default;
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
