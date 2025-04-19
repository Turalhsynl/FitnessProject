//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//namespace FitnessProject.API.Security;

//public static class AuthenticationService
//{
//    public static IServiceCollection AddAuthenticationService(this IServiceCollection service, IConfiguration configuration)
//    {
//        service.AddAuthentication(opts =>
//        {
//            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        }).AddJwtBearer(cfg =>
//        {
//            cfg.RequireHttpsMetadata = false;
//            cfg.SaveToken = true;

//            cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
//            {
//                ValidIssuer = configuration["JWT:ValidIssuer"],
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
//                ValidAudience = configuration["JWT:ValidAudience"],
//            };
//        });

//        return service;
//    }
//}


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FitnessProject.API.Security;

public static class AuthenticationService
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddAuthentication(opts =>
        {
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;

            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
            };

            // SignalR üçün access_token query parametrlərindən token almaq
            cfg.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;

                    // yalnız /chathub üçün token query-dən oxunsun
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/chathub")))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        return service;
    }
}
