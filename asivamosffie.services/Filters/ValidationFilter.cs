using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace asivamosffie.services.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // if (!context.ModelState.IsValid)
            // {
            //     //context.Result = new BadRequestObjectResult(context.ModelState);
            //     var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
            //       .SelectMany(v => v.Errors)
            //       .Select(v => v.ErrorMessage)
            //       .ToList();

            //     var responseObj = new Respuesta
            //     {
            //         Message = "Error validacion en campos",
            //         Code = "501",
            //         Data = context.ModelState.Values
            //     };

            //     context.Result = new JsonResult(responseObj)
            //     {
            //         StatusCode = 400
            //     };

            //     return;
            // }

            await next();
        }
    }
}
