using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WatApi.Config
{
    public class SwaggerCsrfHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpper();

            if (method == "POST" || method == "PUT" || method == "DELETE" || method == "PATCH")
            {
                operation.Parameters ??= [];

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-CSRF",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Default = new Microsoft.OpenApi.Any.OpenApiString("1")
                    },
                    Description = "CSRF Protection Header"
                });
            }
        }
    }
}