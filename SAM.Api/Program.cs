using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SAM.Api.Token;
using SAM.Entities;
using SAM.Entities.Interfaces;
using SAM.Repositories.Database.Context;
using SAM.Repositories.Database.Extensions;
using SAM.Service.Extensions;
using System.Text;

namespace SAM.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(cors => cors.AddPolicy("AllowOriginAndMethod", policy =>
            {
                policy.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
            }));

            builder.Services.AddControllers(config =>
            {
                config.Filters.Add<AuthorizationFilter>();
            });

            builder.Services.AddLogging();

            builder.Services.AddScoped<ICurrentUser, UserReturn>();

            builder.Services
                .AddEndpointsApiExplorer()
                .AddDatabaseRepository()
                .AddServices();

            //configura a autenticação do swagger  
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Informe o token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                           {
                               new OpenApiSecurityScheme
                               {
                                   Reference = new OpenApiReference
                                   {
                                       Type=ReferenceType.SecurityScheme,
                                       Id="Authorization"
                                   }
                               },
                               Array.Empty<string>()
                           }

                });
            });

            #region Injeção de dependência do JWT Token  
            var tokenConfiguration = new TokenConfiguration();
            var authenticate = new Authenticate();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfiguration")).Configure(tokenConfiguration);
            builder.Services.AddSingleton(tokenConfiguration);
            builder.Services.AddScoped<IGenerateToken, GenerateToken>();
            #endregion

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidIssuer = tokenConfiguration.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret!))
                };
            });

            var app = builder.Build();

            //executa as migrações  
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<MySqlContext>();
                dbContext?.Database.Migrate();
            }
            // Configure the HTTP request pipeline.  
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowOriginAndMethod");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}