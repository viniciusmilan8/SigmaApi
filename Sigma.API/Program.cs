using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sigma.Infra.CrossCutting.IoC;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var configuration = builder.Configuration;

        // Segredos não ficam versionados: em desenvolvimento vêm do User Secrets,
        // em produção de variáveis de ambiente. Ver README.
        var connectionString = configuration.GetConnectionString("Database");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "ConnectionStrings:Database não configurada. Rode: dotnet user-secrets set \"ConnectionStrings:Database\" \"<connection string>\"");

        var secretKey = configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException(
                "JwtSettings:SecretKey não configurada. Rode: dotnet user-secrets set \"JwtSettings:SecretKey\" \"<chave de 32+ bytes>\"");

        builder.Services.AddApplicationContext(connectionString);

        MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.AddMaps(new[] { "Sigma.Application" });
        });

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "SigmaAuth",
                    ValidAudience = "SigmaAPI",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"erro\": \"Não autorizado. Token ausente ou inválido.\"}");
                    },

                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"erro\": \"Acesso negado. Você não tem permissão para esta operação.\"}");
                    }
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddSingleton(mapperConfiguration.CreateMapper());

        builder.Services.AddApplicationServicesCollentions();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}