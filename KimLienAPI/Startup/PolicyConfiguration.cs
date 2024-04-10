namespace KimLienAPI.Startup
{
    public static class PolicyConfiguration
    {
        public static string LocalPolicy = "LocalPolicy";
        public static void ConfigPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(option =>
            {
                option.AddPolicy(LocalPolicy, policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader();
                    policy.WithOrigins("http://localhost:3000");
                });
            });
        }
    }
}
