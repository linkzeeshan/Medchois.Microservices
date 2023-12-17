using Medchois.UserManagementService.Infrastructure.Data;
using Medchois.UserManagementService.Services.EmailService;
using Medchois.UserManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Medchois.UserManagementService.Services;
using Medchois.UserManagementService.Domain.IRepository;
using Medchois.UserManagementService.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Medchois.UserManagementService.Domain.Interfaces.IRepository;
using Medchois.UserManagementService.Domain.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Load the connection string from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//Add config required Email
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});
//Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

//configure email setting
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

//Add DI
builder.Services.AddScoped<IUserEmailService, UserEmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserManagementRepo, UserManagementRepo>();
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
//Jwt configuration ends here

//Add Authorization Button
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "User Management API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/Register", [AllowAnonymous] async ([FromBody] UserCreateDto model, IUserEmailService _emailService, IUserManagementRepo _repositoiry) =>
{
    return await _repositoiry.CreateAsync(model);
})
.WithName("Register")
.WithOpenApi();

app.MapPost("/Login", [AllowAnonymous] async ([FromBody] LoginDto model, IUserEmailService _emailService, IUserManagementRepo _repositoiry) =>
{
    var response = await _repositoiry.LoginAsync(model);
    return response;
})
.WithName("Login")
.WithOpenApi();

app.MapPost("/login-2FA", [AllowAnonymous] async (string code, string username, IUserEmailService _emailService, IUserManagementRepo _repositoiry) =>
{
    return await _repositoiry.LoginWithOTPAsync(code, username); ;
})
.WithName("Login-2FA")
.WithOpenApi();

app.MapGet("/ForgotPassword", [AllowAnonymous] async ([Required] string email, IUserManagementRepo _repositoiry) =>
{
    return await _repositoiry.ForgotPasswordAsync(email);
})
.WithName("ForgotPassword")
.WithOpenApi();

app.MapGet("/ResetPassword", [AllowAnonymous] async (string token, string email, IUserManagementRepo _repositoiry) =>
{
    return await _repositoiry.ResetPasswordAsync(token, email);
})
.WithName("GetResetPassword")
.WithOpenApi();

app.MapPost("/ResetPassword", [AllowAnonymous] async (ResetPasswordDto model, IUserManagementRepo _repositoiry) =>
{
    return await _repositoiry.ResetPasswordAsync(model);
})
.WithName("ResetPassword")
.WithOpenApi();

app.MapGet("/ConfirmEmail", [AllowAnonymous] async (string email, string token, IUserManagementRepo _repositoiry) =>
{
    return await _repositoiry.ConfirnmEmailAsync(email, token);
})
.WithName("ConfirmEmail")
.WithOpenApi();

app.MapPost("/TestAuthorization", async (string token, HttpContext httpContext) =>
{
    var user = httpContext.User;
    return user;
}).RequireAuthorization()
.WithName("TestAuthorization")
.WithOpenApi();
app.Run();

