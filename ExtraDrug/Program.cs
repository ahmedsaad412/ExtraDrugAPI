
using ExtraDrug.Core.Models;
using ExtraDrug.Helpers;
using ExtraDrug.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Persistence.Services;
using ExtraDrug.Controllers.Attributes;
using Microsoft.AspNetCore.Mvc;
using ExtraDrug.Persistence.Repositories;

namespace ExtraDrug;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // add DbContext
        builder.Services.AddDbContext<AppDbContext>(
            optionsAction: options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );

        // Add Identity Service
        builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        // get JWT  config from JSON FILE 
        builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
        builder.Services.Configure<PhotoSettings>(builder.Configuration.GetSection("PhotosSetting"));

        // Defined services 
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IDrugCategoryRepo, DrugCategoryRepo>();
        builder.Services.AddScoped<IDrugTypeRepo,DrugTypeRepo>();
        builder.Services.AddScoped<IDrugCompanyRepo,DrugCompanyRepo>();
        builder.Services.AddScoped<IDrugRepo,DrugRepo>();
        builder.Services.AddScoped<IEffectiveMatrialRepo, EffectiveMatrialRepo>();
        builder.Services.AddScoped<IUserRepo, UserRepo>();
        builder.Services.AddScoped<IFileService,FileService>();


        builder.Services.AddScoped<ResponceBuilder>();
        builder.Services.AddScoped(typeof(RepoResultBuilder<>));
       


        //defined filters 
        builder.Services.AddScoped<ValidateModelAttribute>();
        builder.Services.AddScoped<ExceptionHandlerAttribute>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme =JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           })
            .AddJwtBearer( o =>
            {
                o.RequireHttpsMetadata = false; 
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey= true,
                    ValidateIssuer  =true,
                    ValidateAudience= true,
                    ValidateLifetime =true,
                    ValidIssuer= builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "dedault key"))
                };
            });


        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
