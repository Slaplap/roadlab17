using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Interon.Roadlab.LIMS.DTO.Quicktype
{
    public partial class SageCategory
    {
        public static Dictionary<string, SageCategory> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, SageCategory>>(json, Interon.Roadlab.LIMS.DTO.Quicktype.Converter.Settings);
    }
    public enum Image { ImagesPlaceholderNoImagePng };

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, SageCategory> self) => JsonConvert.SerializeObject(self, Interon.Roadlab.LIMS.DTO.Quicktype.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ImageConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class DecodeArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(List<long>);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new List<long>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var converter = ParseStringConverter.Singleton;
                var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                value.Add(arrayItem);
                reader.Read();
            }
            return value;
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (List<long>)untypedValue;
            writer.WriteStartArray();
            foreach (var arrayItem in value)
            {
                var converter = ParseStringConverter.Singleton;
                converter.WriteJson(writer, arrayItem, serializer);
            }
            writer.WriteEndArray();
            return;
        }

        public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class ImageConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Image) || t == typeof(Image?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "/images/placeholder_no_image.png")
            {
                return Image.ImagesPlaceholderNoImagePng;
            }
            throw new Exception("Cannot unmarshal type Image");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Image)untypedValue;
            if (value == Image.ImagesPlaceholderNoImagePng)
            {
                serializer.Serialize(writer, "/images/placeholder_no_image.png");
                return;
            }
            throw new Exception("Cannot marshal type Image");
        }

        public static readonly ImageConverter Singleton = new ImageConverter();
    }
    public partial class SageCategory
    {
        [JsonProperty("categoryIDs")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public List<long> CategoryIDs { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("enabled")] public bool Enabled { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("image")] public Image Image { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("optionalAddOnIDs")] public List<object> OptionalAddOnIDs { get; set; }

        [JsonProperty("productIDs")] public List<object> ProductIDs { get; set; }

        [JsonProperty("propertiesIDs")] public List<object> PropertiesIDs { get; set; }

        [JsonProperty("purchasable")] public bool Purchasable { get; set; }

        [JsonProperty("requiredAddOnIDs")] public List<object> RequiredAddOnIDs { get; set; }

        [JsonProperty("roleIDs")] public List<long> RoleIDs { get; set; }

        [JsonProperty("sortOrder")] public long SortOrder { get; set; }
    }
}