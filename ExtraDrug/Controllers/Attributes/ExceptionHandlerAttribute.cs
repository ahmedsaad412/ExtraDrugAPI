using ExtraDrug.Controllers.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExtraDrug.Controllers.Attributes;

public class ExceptionHandlerAttribute:ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var Exeption = context.Exception;
        while(Exeption.InnerException != null)
        {
            Exeption = Exeption.InnerException;
        }


        context.Result = new BadRequestObjectResult(new ErrorResponce() { 
            Message = Exeption.Message,
            Errors = null
        });
    }

}
