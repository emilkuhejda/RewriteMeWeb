using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace RewriteMe.WebApi.Utils
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiVersion = controllerNamespace?.Split('.').Last().ToLower(CultureInfo.InvariantCulture);

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
