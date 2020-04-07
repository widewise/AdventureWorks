using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace AdventureWorks.Web.Filters
{
    public class FileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId.ToLower() == "images_uploadsinglefile")
            {
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>(1);
                else
                    operation.parameters.Clear();
                operation.parameters.Add(new Parameter
                {
                    name = "File",
                    @in = "formData",
                    description = "Upload software package",
                    required = true,
                    type = "file"
                });
                operation.consumes.Add("application/form-data");
            }
        }
    }
}