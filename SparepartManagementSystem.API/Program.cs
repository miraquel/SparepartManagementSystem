using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using SparepartManagementSystem.API.Swagger;
using SparepartManagementSystem.Repository;
using SparepartManagementSystem.Service;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SparepartManagementSystem.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var config = builder.Configuration;
        // Add services to the container.
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        builder.Services.AddAuthorization();

        var serilogConfiguration = new LoggerConfiguration();

        serilogConfiguration.ReadFrom.Configuration(config);

        if (!Enum.TryParse<DatabaseProvider>(config["DatabaseProvider"], out var databaseProvider))
        {
            throw new InvalidEnumArgumentException(nameof(databaseProvider), (int)databaseProvider, typeof(DatabaseProvider));
        }

        switch (databaseProvider)
        {
            case DatabaseProvider.MySql:
                serilogConfiguration.WriteTo.MySQL(config["ConnectionStrings:MySQL"]);
                break;
            case DatabaseProvider.SqlServer:
                // serilogConfiguration.WriteTo.MSSqlServer(config["ConnectionStrings:SqlServer"]);
                // break;
            case DatabaseProvider.PostgresSql:
            case DatabaseProvider.Oracle:
            case DatabaseProvider.SqLite:
            default:
                throw new InvalidOperationException("Database provider not supported");
        }

        if (builder.Environment.IsDevelopment())
        {
            serilogConfiguration.WriteTo.Console();
        }

        var serilog = serilogConfiguration.CreateLogger();

        Log.Logger = serilog;

        builder.Services.AddLogging(loggingBuilder => { loggingBuilder.AddSerilog(serilog); });

        builder.Services.AddRepository();
        builder.Services.AddService();

        builder.Services.AddHostedService<LifetimeEventsHostedService>();
        // Add services to the container.
        builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }).AddRazorRuntimeCompilation();
        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        builder.Services.AddHttpContextAccessor();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddDistributedMemoryCache();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();

                foreach (var description in descriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}