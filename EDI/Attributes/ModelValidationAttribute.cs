using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EDI.Attributes
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            if (!context.ModelState.IsValid)
            {
                // Handle invalid model state, for example return BadRequest
                context.Result = new BadRequestObjectResult(context.ModelState);
                //var redirectResult = new RedirectToActionResult(action.ToString(), controller.ToString(), null);
                //context.Result = redirectResult;
            }
        }
    }
}
