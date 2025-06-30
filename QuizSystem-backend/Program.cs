using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;
using Mono.TextTemplating;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using QuizSystem_backend.Helper;
using System.Text.Json.Serialization;
//using QuizSystem_backend.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionBankService, QuestionBankService>();
builder.Services.AddScoped<IQuestionBankRepository, QuestionBankRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ICourseClassRepository, CourseClassRepository>();
builder.Services.AddScoped<ICourseClassService, CourseClassService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IExamRepository, EXamRepository>();
builder.Services.AddScoped<IExamServices, ExamServices>();
builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Bảo vệ Session khỏi XSS
    options.Cookie.IsEssential = true; // Yêu cầu cookie luôn được lưu
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull; // Hide field nulll
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
            RoleClaimType = ClaimTypes.Role

        };

        // 👇 Thêm đoạn này để xử lý lỗi trả về 401 thay vì 500
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.NoResult();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\": \"Authentication failed\"}");
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\": \"Unauthorized\"}");
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\": \"Forbidden\"}");
            }
        };
    });



builder.Services.AddDbContext<QuizSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuizSystemConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

SeedData.Initialize(app.Services);

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<QuizSystemDbContext>();

var hasher = new PasswordHasher<User>();

var users = await db.Users.ToListAsync();

foreach (var user in users)
{
    if (!user.PasswordHash.StartsWith("$"))
    {
        user.PasswordHash = hasher.HashPassword(user, "123456789");
    }
}

await db.SaveChangesAsync();


app.Run();
