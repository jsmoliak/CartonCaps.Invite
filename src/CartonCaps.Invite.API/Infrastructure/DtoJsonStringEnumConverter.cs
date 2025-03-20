using System.Text.Json;
using System.Text.Json.Serialization;

namespace CartonCaps.Invite.API.Infrastructure
{
    /// <summary>
    /// A JSON converter for serializing and deserializing enum values as strings.
    /// </summary>
    /// <typeparam name="T">The enum type to convert.</typeparam>
    public class DtoJsonStringEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        // <summary>
        /// Reads and converts JSON to an enum value.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted enum value.</returns>
        /// <exception cref="JsonException">Thrown when the JSON value is not a valid enum member.</exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();

            if (!Enum.TryParse<T>(value, true, out var result) || !Enum.IsDefined(typeof(T), result))
            {
                throw new JsonException($"Invalid value '{value}' for {typeof(T)}. Valid values are: {string.Join(", ", Enum.GetNames(typeof(T)))}");
            }

            return result;
        }

        /// <summary>
        /// Writes an enum value as JSON.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
        /// <param name="value">The enum value to write.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
