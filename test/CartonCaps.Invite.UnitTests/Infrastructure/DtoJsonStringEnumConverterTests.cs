using CartonCaps.Invite.API.Infrastructure;
using System.Text;
using System.Text.Json;

namespace CartonCaps.Invite.UnitTests.Infrastructure
{
    public class DtoJsonStringEnumConverterTests
    {
        private enum TestEnum
        {
            Value1,
            Value2,
            ValueThree
        }

        // Tests that the Read method correctly converts a valid enum string to the corresponding enum value.
        [Fact]
        public void Read_ValidEnumValue_ReturnsCorrectEnum()
        {
            // Arrange
            var json = "\"value2\"";
            var converter = new DtoJsonStringEnumConverter<TestEnum>();
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            reader.Read(); // Advance to the string token

            // Act
            var result = converter.Read(ref reader, typeof(TestEnum), new JsonSerializerOptions());

            // Assert
            Assert.Equal(TestEnum.Value2, result);
        }

        // Tests that the Read method correctly converts a valid enum string (case-insensitive) to the corresponding enum value.
        [Fact]
        public void Read_ValidEnumValueCaseInsensitive_ReturnsCorrectEnum()
        {
            // Arrange
            var json = "\"VaLuE1\"";
            var converter = new DtoJsonStringEnumConverter<TestEnum>();
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            reader.Read();

            // Act
            var result = converter.Read(ref reader, typeof(TestEnum), new JsonSerializerOptions());

            // Assert
            Assert.Equal(TestEnum.Value1, result);
        }

        // Tests that the Read method throws a JsonException when an invalid enum string is provided.
        [Fact]
        public void Read_InvalidEnumValue_ThrowsJsonException()
        {
            // Arrange
            var json = "\"invalidValue\"";
            var converter = new DtoJsonStringEnumConverter<TestEnum>();

            // Act & Assert
            Assert.Throws<JsonException>(() =>
            {
                var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
                reader.Read();
                converter.Read(ref reader, typeof(TestEnum), new JsonSerializerOptions());
            });
        }

        // Tests that the Write method correctly converts an enum value to its corresponding string representation.
        [Fact]
        public void Write_ValidEnumValue_WritesCorrectString()
        {
            // Arrange
            var converter = new DtoJsonStringEnumConverter<TestEnum>();
            using var memoryStream = new MemoryStream();
            using var writer = new Utf8JsonWriter(memoryStream);
            var options = new JsonSerializerOptions();

            // Act
            converter.Write(writer, TestEnum.ValueThree, options);
            writer.Flush();

            // Assert
            var json = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            Assert.Equal("\"ValueThree\"", json);
        }

        // Tests that the Read method throws a JsonException when a null value is provided.
        [Fact]
        public void Read_NullValue_ThrowsJsonException()
        {
            // Arrange
            var json = "null";
            var converter = new DtoJsonStringEnumConverter<TestEnum>();

            // Act & Assert
            Assert.Throws<JsonException>(() =>
            {
                var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
                reader.Read();
                converter.Read(ref reader, typeof(TestEnum), new JsonSerializerOptions());
            });
        }
    }
}