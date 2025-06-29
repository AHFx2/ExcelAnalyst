using System.Text;
using ExcelAnalyst.Data.Repositores;
using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Common.IUnitOfWork;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Domain.JWTOptions;
using ExcelAnalyst.Repository.EntityFrameworkCore.Context;
using ExcelAnalyst.Repository.UnitOfWork;
using ExcelAnalyst.Service;
using ExcelAnalyst.Service.Objects.Auth;
using ExcelAnalyst.Service.Objects.JWT;
using ExcelAnalyst.Service.Objects.Users.Validator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static ExcelAnalyst.Data.Seeder.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();



#region EF Core Configurations
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


#endregion

#region Cors Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("ExcelAnalystFrontend", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader()
              .AllowAnyMethod();

    });
});

#endregion


#region Identity Configurations
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    // عدد المحاولات الفاشلة قبل القفل
    options.Lockout.MaxFailedAccessAttempts = 5;

    // مدة القفل
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

    // تفعيل القفل لحسابات المستخدمين الجدد
    options.Lockout.AllowedForNewUsers = true;

});
#endregion

#region Fluent Validation Configurations 
builder.Services.AddValidatorsFromAssemblyContaining<AddUserDTOValidator>();
builder.Services.AddFluentValidationAutoValidation();
#endregion



#region Security Configurations

var jWTOptions = builder.Configuration.GetSection("JWT").Get<JWToptions>();
builder.Services.AddSingleton(jWTOptions);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jWTOptions.Issuer,
        ValidAudience = jWTOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTOptions.SigningKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped<IJwtService, JwtService>();
#endregion

#region Dependency Injection Configurations
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();  
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("ExcelAnalystFrontend");

app.UseAuthorization();

app.MapControllers();

await IdentitySeeder.SeedAsync(app.Services); 


app.Run();
