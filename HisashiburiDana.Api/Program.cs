using HisashiburiDana.Api.Filters;
using HisashiburiDana.Application;
using HisashiburiDana.Application.MiddleWares;
using HisashiburiDana.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

string xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
string xmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HisashiburiDana.Api", Version = "v1" });
    //
#if DEBUG

#else
    c.DocumentFilter<SwaggerFilter>(); 
#endif

    c.IncludeXmlComments(xmlCommentFullPath, true);
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement();
    securityRequirement.Add(securitySchema, new[] { "Bearer" });
    c.AddSecurityRequirement(securityRequirement);
});


var logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File(new JsonFormatter(), "important-Logs.json", restrictedToMinimumLevel: LogEventLevel.Warning)
                    .WriteTo.File("./Dailylogs.txt", rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Information()
                    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(logger, dispose: true);
});
//builder.Services.AddSingleton<Serilog.ILogger>(logger);
logger.Information($"Starting Application at ==> {DateTime.UtcNow.AddHours(1)}");
builder.Services.AddApplication()
    .AddAInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(
    c => {
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "HisashiburiDana.Api");
        c.DocExpansion(DocExpansion.List);
    });

app.UseMiddleware<ExceptionMiddleWare>();
app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
