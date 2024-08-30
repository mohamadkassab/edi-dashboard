namespace EDI.Middlewares
{
    public class StartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<ErrorHandlerMiddleware>();
                next(builder);
            };
        }
    }
}
