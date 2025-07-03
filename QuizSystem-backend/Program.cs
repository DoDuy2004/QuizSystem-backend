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
using QuizSystem_backend.Controllers;
//using QuizSystem_backend.Controllers;

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
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<QuizSystemDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 4; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 100; // Thất bại 5 lần thì khóa
    options.Lockout.AllowedForNewUsers = false;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

        //// 👇 Thêm đoạn này để xử lý lỗi trả về 401 thay vì 500
        //options.Events = new JwtBearerEvents
        //{
        //    OnAuthenticationFailed = context =>
        //    {
        //        context.NoResult();
        //        context.Response.StatusCode = 401;
        //        context.Response.ContentType = "application/json";
        //        return context.Response.WriteAsync("{\"message\": \"Authentication failed\"}");
        //    },
        //    OnChallenge = context =>
        //    {
        //        context.HandleResponse();
        //        context.Response.StatusCode = 401;
        //        context.Response.ContentType = "application/json";
        //        return context.Response.WriteAsync("{\"message\": \"Unauthorized\"}");
        //    },
        //    OnForbidden = context =>
        //    {
        //        context.Response.StatusCode = 403;
        //        context.Response.ContentType = "application/json";
        //        return context.Response.WriteAsync("{\"message\": \"Forbidden\"}");
        //    }
        //};
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AddQuestion", policy => policy.RequireClaim("AddQuestion", "true"));
    options.AddPolicy("EditQuestion", policy => policy.RequireClaim("EditQuestion", "true"));
    options.AddPolicy("DeleteQuestion", policy => policy.RequireClaim("DeleteQuestion", "true"));
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



using (var scope = app.Services.CreateScope())
{
    //var db = scope.ServiceProvider.GetRequiredService<QuizSystemDbContext>();

    //var hasher = new PasswordHasher<AppUser>();

    //var users = await db.Users.ToListAsync();

    //foreach (var user in users)
    //{
    //    if (!user.PasswordHash!.StartsWith("$"))
    //    {
    //        user.PasswordHash = hasher.HashPassword(user, "123456789");
    //    }
    //}

    //await db.SaveChangesAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string[] roles = new[] { "ADMIN", "TEACHER", "STUDENT" };

    foreach (var roleName in roles)
    {
        var exists = await roleManager.RoleExistsAsync(roleName);
        if (!exists)
        {
            var role = new IdentityRole<Guid>(roleName);
        
            await roleManager.CreateAsync(role);
        }
    }

    var adminEmail = "ndduy@gmail.com";
    var studentEmail = "student@caothang.edu.vn";
    var teacherEmail = "teacher@caothang.edu.vn";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(adminUser, "123456789");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "ADMIN");
        }
    }

    var studentUser = await userManager.FindByEmailAsync(studentEmail);
    if (studentUser == null)
    {
        studentUser = new Student
        {
            UserName = studentEmail,
            Email = studentEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(studentUser, "12345678");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(studentUser, "STUDENT");
        }
    }

    var teacherUser = await userManager.FindByEmailAsync(teacherEmail);
    if (teacherUser == null)
    {
        teacherUser = new Teacher
        {
            UserName = teacherEmail,
            Email = teacherEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(teacherUser, "12345678");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(teacherUser, "TEACHER");
        }
    }


}

    app.Run();