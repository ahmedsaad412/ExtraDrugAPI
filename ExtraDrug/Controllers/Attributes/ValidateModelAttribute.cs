using ExtraDrug.Controllers.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace ExtraDrug.Controllers.Attributes
{
    public class ValidateModelAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponce()
                {
                    Errors = context.ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)).ToList(),
                    Message = "Invalid User Data."
                });
            }
        }
    }
}