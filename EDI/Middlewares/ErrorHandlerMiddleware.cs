using EDI.Controllers;
using System.Reflection;

namespace EDI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ErrorController _errorController;

        public ErrorHandlerMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _errorController = new ErrorController(configuration);
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                string controllerName = this.GetType().Name;
                string FuntionName = MethodBase.GetCurrentMethod().Name;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                await _next(httpContext);
            }
        } 
    }
}
