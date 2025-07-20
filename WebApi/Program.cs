using FileLoader;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using SqlServerLoader;
using WebApi.Middleware;
using WebApi.Services;
using WebApi.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);


#region Core Services
builder.Services.AddControllers();

builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    { 
        Title = "Vendor API", 
        Version = "v1",
        Description = "A vendor API using provided loaders"
    });
});

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));
#endregion

# region Application Services
switch (builder.Configuration["VendorLoader"])
{
    case "sql":
        builder.Services.AddScoped<DataLoader>(_ =>
            new DataLoader("server","userId","password"));
        builder.Services.AddScoped<IVendorLoader, SqlLoaderAdapter>();
        break;
    case "file":
        // register Loader as a singleton, passing in the file name
        builder.Services.AddSingleton<Loader>(sp =>
            new Loader("suppliers.txt"));
        builder.Services.AddSingleton<IVendorLoader, FileLoaderAdapter>();
        break;
    default:
        throw new ArgumentException("Not a valid vendor loader selected in configuration.");
}
#endregion

#region HttpLogging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add(HeaderNames.Accept);
    logging.RequestHeaders.Add(HeaderNames.ContentType);
    logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
    logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
    logging.RequestHeaders.Add(HeaderNames.ContentLength);
            
    logging.MediaTypeOptions.AddText("application/json");
    logging.MediaTypeOptions.AddText("multipart/form-data");
            
    logging.RequestBodyLogLimit = 1024;
    logging.ResponseBodyLogLimit = 1024;
});


#endregion

var app = builder.Build();

app.MapHealthChecks("/api/health");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpLogging();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.MapControllers();

app.UseHttpsRedirection();

await app.RunAsync();
