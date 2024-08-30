namespace EDI.Services.ConfigService
{
    public static class ConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddMyDependencyGroup(
             this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanCreateUser", policy =>
                    policy.RequireClaim("CanCreateUser","True"));

                options.AddPolicy("CanApproveContent", policy =>
                  policy.RequireClaim("CanApproveContent", "True"));
            });


            return services;
        }
    }
}
