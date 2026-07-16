using CommunicaAI.Configurations;
using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Models.Configurations;
using CommunicaAI.Repositories;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services;
using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("Gemini"));

builder.Services.Configure<PythonVerificationServiceOptions>(
    builder.Configuration.GetSection("PythonVerificationService"));

builder.Services.AddHttpClient("PythonVerification", client =>
{
    var baseUrl = builder.Configuration["PythonVerificationService:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
    client.Timeout = TimeSpan.FromMinutes(2);
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGeminiService,GeminiService>();
builder.Services.AddScoped<IAnswerEvaluationRepository,AnswerEvaluationRepository>();
builder.Services.AddScoped<IInterviewAnswerRepository, InterviewAnswerRepository>();

// Interview Module
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<IInterviewService, InterviewService>();

builder.Services.AddScoped<ITranscriptionService,GeminiTranscriptionService>();

// Question Bank Module
builder.Services.AddScoped<IQuestionBankRepository, QuestionBankRepository>();
builder.Services.AddScoped<IQuestionBankService, QuestionBankService>();

// Interview Question Module
builder.Services.AddScoped<IInterviewQuestionRepository, InterviewQuestionRepository>();
builder.Services.AddScoped<IInterviewQuestionService, InterviewQuestionService>();

// Interview Answer Module
builder.Services.AddScoped<IInterviewAnswerRepository, InterviewAnswerRepository>();
builder.Services.AddScoped<IInterviewAnswerService, InterviewAnswerService>();

// Interview Result Module
builder.Services.AddScoped<IInterviewResultRepository, InterviewResultRepository>();
builder.Services.AddScoped<IInterviewResultService, InterviewResultService>();

// Company Profile Module (Module 6)
builder.Services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();

// Resume Profile Module (Module 7)
builder.Services.AddScoped<IResumeProfileRepository, ResumeProfileRepository>();
builder.Services.AddHttpClient<ResumeParserService>();

// Video Analysis Module (Module 4)
builder.Services.AddScoped<VideoAnalysisService>();

// Analytics Module (Module 8)
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                      "http://localhost:4200",
                      "https://localhost:4200",
                      "http://localhost:4000",
                      "https://localhost:4000",
                      "https://communicaai-frontend.onrender.com"
                  )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();