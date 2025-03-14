using Application;
using Application.Security;
using DAL.SqlServer;
using FitnessProject.API.Infrastructure;
using FitnessProject.API.Security;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();