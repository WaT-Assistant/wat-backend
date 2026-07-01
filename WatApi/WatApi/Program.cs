using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WatApi.Config;
using WatApi.Data;
using WatApi.Middleware;
using WatApi.Services;
using WatApi.Services.Interfaces;
using WatApi.Validators.JobOffer;

var builder = WebApplication.CreateBuilder(args);

// Bind JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// --- Rate limiting services: memory cache + options ---
builder.Services.AddMemoryCache();
builder.Services.Configure<RateLimitOptions>(builder.Configuration.GetSection("RateLimiting"));
// -----------------------------------------------------

// Data ASP.NET Core validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<JobOfferCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<JobOfferUpdateDtoValidator>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Enter JWT token. Format: Bearer {your_token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    if(builder.Environment.IsDevelopment())
        options.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJobOfferService, JobOfferService>();
builder.Services.AddScoped<IImportantInfoService, ImportantInfoService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Authentication setup (reads from configuration)
var key = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Use global exception middleware early so it catches everything
app.UseMiddleware<ExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularFrontend");

app.UseMiddleware<CsrfHeaderMiddleware>();

// Apply rate limiting early so abusive requests are rejected quickly
app.UseMiddleware<RateLimitingMiddleware>();

// Ensure authentication middleware runs before authorization
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();