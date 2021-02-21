using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers.Swagger
{
    public class ApiVersionOperationFilter : IOperationFilter
    {
      

        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            ApiVersionModel actionApiVersionModel = context.ApiDescription.ActionDescriptor?.GetApiVersion();
            if(actionApiVersionModel == null)
            {
                return;
            }

            if (actionApiVersionModel.DeclaredApiVersions.Any())
            {
                operation.Produces = operation.Produces
                    .SelectMany(p => actionApiVersionModel.DeclaredApiVersions
                    .Select(version => $"{p};v={version.ToString()}")).ToList();
            }
            else
            {
                operation.Produces = operation.Produces
                    .SelectMany(p => actionApiVersionModel.ImplementedApiVersions.OrderByDescending(v => v)
                    .Select(version => $"{p}; v={version.ToString()}")).ToList();
            }
        }
    }
}
