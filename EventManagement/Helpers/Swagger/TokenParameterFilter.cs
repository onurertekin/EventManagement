using Host.Filter;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Host.Helpers.Swagger
{
    public class TokenParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            var authorizableAttribute = actionDescriptor.EndpointMetadata.OfType<AuthorizableAttribute>().FirstOrDefault();
            if (authorizableAttribute != null)
            {
                var p = new OpenApiParameter();
                p.Name = "Token";
                p.Required = true;
                p.Schema = new OpenApiSchema() { Type = "string" };
                p.In = ParameterLocation.Header;
                p.Description = "";
                operation.Parameters.Add(p);
            }
        }
    }
}
