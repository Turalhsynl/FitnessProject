using Application;
using Application.Security;
using DAL.SqlServer;
using DAL.SqlServer.Context;
using DAL.SqlServer.Infastructure;
using FitnessProject.API.Infrastructure;
using FitnessProject.API.Infrastructure.Middlewares;
using FitnessProject.API.Security;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://localhost:7298") // Frontend və Swagger
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
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
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();


app.MapControllers();

app.UseMiddleware<FitnessProject.API.Infrastructure.Middlewares.ExceptionHandlerMiddleware>();

app.Run();