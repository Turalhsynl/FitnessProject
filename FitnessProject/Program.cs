using Application;
using Application.Abstractions;
using Application.Security;
using Application.Services;
using DAL.SqlServer;
using DAL.SqlServer.Context;
using DAL.SqlServer.Infastructure;
using FitnessProject.API.Hubs;
using FitnessProject.API.Infrastructure;
using FitnessProject.API.Infrastructure.Middlewares;
using FitnessProject.API.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("WithSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("https://localhost:7298", "http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

builder.Services.AddSingleton<IOpenAIService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var apiKey = configuration["OpenAI:ApiKey"];
    return new OpenAIService(apiKey);
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserContext, HttpUserContext>();

var conn = builder.Configuration.GetConnectionString("MyConn");
builder.Services.AddSqlServices(conn!);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerService();
builder.Services.AddAuthenticationService(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddScoped<IFitnessProgramRepository, SqlFitnessProgramRepository>();
builder.Services.AddScoped<ICategoryRepository, SqlCategoryRepository>();
builder.Services.AddScoped<IRecipeRepository, SqlRecipeRepository>();
builder.Services.AddScoped<IFileUploadRepository, SqlFileUploadRepository>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProgramService, ProgramImageService>();
builder.Services.AddScoped<ICategoryService, CategoryImageService>();
builder.Services.AddScoped<IRecipeService, RecipeImageService>();
builder.Services.AddScoped<StripeService>();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSignalR();
builder.Services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();





var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("WithSpecificOrigin");

app.UseAuthorization();
app.MapHub<ChatHub>("/chathub");

app.MapControllers();

app.UseMiddleware<FitnessProject.API.Infrastructure.Middlewares.ExceptionHandlerMiddleware>();

app.Run();