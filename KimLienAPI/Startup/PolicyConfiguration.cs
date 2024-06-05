namespace KimLienAPI.Startup
{
    public static class PolicyConfiguration
    {
        public const string LocalPolicy = "LocalPolicy";
        public const string AnonymousPolicy = "AnonymousPolicy";
        public static void ConfigPolicy(this WebApplicationBuilder builder, string policy)
        {
          switch (policy)
            {
                case LocalPolicy:
                    builder.Services.AddCors(option =>
                    {
                        option.AddPolicy(policy, policy =>
                        {
                            policy.AllowAnyMethod().AllowAnyHeader();
                            policy.WithOrigins("http://localhost:3000");
                        });
                    });
                    break;
                case AnonymousPolicy:
                    builder.Services.AddCors(option =>
                    {
                        option.AddPolicy(policy, policy =>
                        {
                            policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                        });
                    });
                    break;
            }
        }
    }
}
