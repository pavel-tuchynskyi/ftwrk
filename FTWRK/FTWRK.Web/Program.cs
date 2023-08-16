using FTWRK.Application.Configuration;
using FTWRK.Application.Hubs;
using FTWRK.Infrastructure.Configuration;
using FTWRK.Persistance.Configuration;
using FTWRK.Web.Configuration;
using FTWRK.Web.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Debug()
        .WriteTo.Console();
});

var configuration = builder.Configuration;

builder.Services.AddHttpClient();
builder.Services.ConfigureWebServices(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistace(configuration);
builder.Services.AddApplication();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FtwrkCors");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapHub<NotificationHub>("/api/notify");

app.MapControllers();

app.Run();
