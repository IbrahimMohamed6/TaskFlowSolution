
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskFlow.InfraStructure.Data.DbContexts;
using TaskFlow.InfraStructure.Repository;
using TaskFlowDomain.Repository.Contract;
using TaskFlowDomain.Service.Contract;
using TaskFlow.Application.Service;
using TaskFlow.API.Middleware;
using TaskFlow.Application.Service.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskFlowDomain.Entites;

namespace TaskFlow.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();
            // Add services to the container.

            builder.Services.AddControllers()
                              .AddJsonOptions(options =>
                              {
                                  options.JsonSerializerOptions.Converters
                                      .Add(new JsonStringEnumConverter());
                              });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddDbContext<ApplicationDbContext>(
                option=>option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                
                ));

            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<User>>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWTSetting:issuer"],
                        ValidAudience = builder.Configuration["JWTSetting:audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:Key"]!))
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<ITaskItemsService, TaskItemsService>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
