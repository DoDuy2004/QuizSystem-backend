﻿using System.Text.Json.Serialization;
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
using QuizSystem_backend.services.MailServices;
using Microsoft.AspNetCore.Identity.UI.Services;
using QuizSystem_backend.Hubs;
using QuizSystem_backend.services.NotificationServices;
using QuizSystem_backend.services.Notificationervices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IQuestionBankService, QuestionBankService>();
builder.Services.AddTransient<IQuestionBankRepository, QuestionBankRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<ICourseClassRepository, CourseClassRepository>();
builder.Services.AddTransient<ICourseClassService, CourseClassService>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IExamRepository, ExamRepository>();
builder.Services.AddTransient<IExamServices, ExamServices>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IChapterRepository, ChapterRepository>();
builder.Services.AddTransient<IRoomExamRepository, RoomExamRepository>();
builder.Services.AddTransient<IStudentExamRepository, StudentExamRepository>();

builder.Services.AddTransient<INotificationService,NotificationService>();

builder.Services.AddTransient<IRoomExamService, RoomExamService>();

builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();



builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IEmailSender, SendMailService>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();
builder.Services.AddTransient<TeacherService>();

builder.Services.AddSignalR();



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
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .SetIsOriginAllowed(_ => true);
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
            },
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/QuizHub"))
                {
                    // kích hoạt bearer token cho SignalR
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
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

app.MapHub<QuizHub>("/QuizHub");
SeedData.Initialize(app.Services);

//using var scope = app.Services.CreateScope();
//var db = scope.ServiceProvider.GetRequiredService<QuizSystemDbContext>();

var hasher = new PasswordHasher<User>();

//var users = await db.Users.ToListAsync();

//foreach (var user in users)
//{
//    if (!user.PasswordHash.StartsWith("$"))
//    {
//        user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);
//    }
//}

//await db.SaveChangesAsync();


app.Run();
