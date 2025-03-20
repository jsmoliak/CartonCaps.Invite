using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CartonCaps.Invite.API.Infrastructure
{
    /// <summary>
    /// A Swagger Schema Filter that converts enum properties to string representations in the OpenAPI documentation.
    /// This filter ensures that enums are displayed as strings with their names, rather than their underlying integer values.
    /// </summary>
    public class DtoJsonStringEnumSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies the schema filter to the given schema and context.
        /// If the context type is an enum, it modifies the schema to represent the enum as a string.
        /// </summary>
        /// <param name="schema">The schema to apply the filter to.</param>
        /// <param name="context">The schema filter context.</param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Type = "string";
                schema.Enum = Enum.GetNames(context.Type)
                    .Select(name => new OpenApiString(name))
                    .ToList<IOpenApiAny>();
            }
        }
    }
}