using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Microsoft.AspNetCore.Mvc.Versioning.ApiVersionMapping;

namespace Sentinel.Api.Billing3.Helpers
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            var apiVersion = apiDescription.GetApiVersion();
            var model = apiDescription.ActionDescriptor.GetApiVersionModel(Explicit | Implicit);

            operation.Deprecated = model.DeprecatedApiVersions.Contains(apiVersion);

            if (operation.Parameters == null)
            {
                return;
            }

            // foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            // {
            //     var description = apiDescription.ParameterDescriptions
            //                                     .First(p => p.Name == parameter.Name);

            //     if (parameter.Description == null)
            //     {
            //         parameter.Description = description.ModelMetadata?.Description;
            //     }

            //     if (parameter.Default == null)
            //     {
            //         parameter.Default = description.DefaultValue;
            //     }

            //     parameter.Required |= description.IsRequired;
            // }
        }
    }
}