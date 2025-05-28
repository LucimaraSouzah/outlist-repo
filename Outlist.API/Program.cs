using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Outlist.Application.Interface;
using Outlist.Application.Services;
using Outlist.Application.UseCases;
using Outlist.Infrastructure.Repositories;
using Outlist.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Logging;
using System.Text;
using Microsoft.AspNetCore.Authentication;

public class Program
{
    public static void Main(string[] args)
    {
        // Habilita logs detalhados de erros JWT para debug
        IdentityModelEventSource.ShowPII = true;

        var builder = WebApplication.CreateBuilder(args);

        // MongoDB
        builder.Services.Configure<MongoDbSettings>(
            builder.Configuration.GetSection("MongoDbSettings"));

        // Dependências
        builder.Services.AddSingleton<MongoContext>();
        builder.Services.AddScoped<IOutlistRepository, OutlistRepository>();
        builder.Services.AddScoped<AddProductUseCase>();
        builder.Services.AddScoped<RemoveProductUseCase>();
        builder.Services.AddScoped<UpdateValidityUseCase>();
        builder.Services.AddScoped<GetAllProductsUseCase>();
        builder.Services.AddScoped<CheckProductBlockedUseCase>();
        builder.Services.AddScoped<IOutlistService, OutlistService>();

        builder.Services.AddControllers();

        // Versionamento de API
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Outlist API",
                Version = "v1",
                Description = "API for managing product blocking periods",
            });

            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando o esquema Bearer. \r\n\r\n" +
                              "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
                              "Exemplo: \"Bearer abcdef12345\"",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        // Autenthentication
        builder.Services.AddAuthentication("ApiKeyScheme")
                         .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKeyScheme", null);

        var app = builder.Build();

        // Middlewares
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
