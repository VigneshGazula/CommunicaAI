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
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// RENDER PORT CONFIGURATION
// ======================================================

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


// ======================================================
// DATABASE CONNECTION
// ======================================================

static string? ResolveConnectionString(IConfiguration configuration)
{
    var environmentDatabaseUrl =
        Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrWhiteSpace(environmentDatabaseUrl))
    {
        if (Uri.TryCreate(
                environmentDatabaseUrl,
                UriKind.Absolute,
                out var databaseUri) &&
            (databaseUri.Scheme == "postgres" ||
             databaseUri.Scheme == "postgresql"))
        {
            var npgsqlBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port > 0
                    ? databaseUri.Port
                    : 5432,

                Database = databaseUri.AbsolutePath.TrimStart('/'),

                Username = databaseUri.UserInfo.Split(':', 2)[0],

                Password = databaseUri.UserInfo.Contains(':')
                    ? databaseUri.UserInfo.Split(':', 2)[1]
                    : string.Empty,

                SslMode = SslMode.Require
            };

            return npgsqlBuilder.ConnectionString;
        }

        return environmentDatabaseUrl;
    }

    return configuration.GetConnectionString("DefaultConnection");
}

var connectionString =
    ResolveConnectionString(builder.Configuration);

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection string is not configured.");
}


// ======================================================
// CONTROLLERS / OPENAPI
// ======================================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();


// ======================================================
// DATABASE
// ======================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


// ======================================================
// CONFIGURATION
// ======================================================

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("Gemini"));

builder.Services.Configure<PythonVerificationServiceOptions>(
    builder.Configuration.GetSection("PythonVerificationService"));


// ======================================================
// HTTP CLIENTS
// ======================================================

builder.Services.AddHttpClient();

builder.Services.AddHttpClient("PythonVerification", client =>
{
    var baseUrl =
        builder.Configuration["PythonVerificationService:BaseUrl"];

    if (!string.IsNullOrWhiteSpace(baseUrl))
    {
        client.BaseAddress = new Uri(baseUrl);
    }

    client.Timeout = TimeSpan.FromMinutes(2);
});

builder.Services.AddHttpClient<ResumeParserService>();


// ======================================================
// CORE SERVICES
// ======================================================

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.AddScoped<
    IPasswordHasher<AppUser>,
    PasswordHasher<AppUser>>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IGeminiService, GeminiService>();

builder.Services.AddScoped<
    ITranscriptionService,
    GeminiTranscriptionService>();


// ======================================================
// ANSWER EVALUATION
// ======================================================

builder.Services.AddScoped<
    IAnswerEvaluationRepository,
    AnswerEvaluationRepository>();


// ======================================================
// INTERVIEW MODULE
// ======================================================

builder.Services.AddScoped<
    IInterviewRepository,
    InterviewRepository>();

builder.Services.AddScoped<
    IInterviewService,
    InterviewService>();


// ======================================================
// QUESTION BANK MODULE
// ======================================================

builder.Services.AddScoped<
    IQuestionBankRepository,
    QuestionBankRepository>();

builder.Services.AddScoped<
    IQuestionBankService,
    QuestionBankService>();


// ======================================================
// INTERVIEW QUESTION MODULE
// ======================================================

builder.Services.AddScoped<
    IInterviewQuestionRepository,
    InterviewQuestionRepository>();

builder.Services.AddScoped<
    IInterviewQuestionService,
    InterviewQuestionService>();


// ======================================================
// INTERVIEW ANSWER MODULE
// ======================================================

builder.Services.AddScoped<
    IInterviewAnswerRepository,
    InterviewAnswerRepository>();

builder.Services.AddScoped<
    IInterviewAnswerService,
    InterviewAnswerService>();


// ======================================================
// INTERVIEW RESULT MODULE
// ======================================================

builder.Services.AddScoped<
    IInterviewResultRepository,
    InterviewResultRepository>();

builder.Services.AddScoped<
    IInterviewResultService,
    InterviewResultService>();


// ======================================================
// COMPANY PROFILE MODULE
// ======================================================

builder.Services.AddScoped<
    ICompanyProfileRepository,
    CompanyProfileRepository>();


// ======================================================
// RESUME PROFILE MODULE
// ======================================================

builder.Services.AddScoped<
    IResumeProfileRepository,
    ResumeProfileRepository>();


// ======================================================
// VIDEO ANALYSIS MODULE
// ======================================================

builder.Services.AddScoped<VideoAnalysisService>();


// ======================================================
// ANALYTICS MODULE
// ======================================================

builder.Services.AddScoped<
    IAnalyticsService,
    AnalyticsService>();


// ======================================================
// CORS
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// ======================================================
// JWT AUTHENTICATION
// ======================================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection =
            builder.Configuration.GetSection("Jwt");

        var jwtKey = jwtSection["Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT Key is not configured.");
        }

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateIssuerSigningKey = true,

                ValidateLifetime = true,

                ValidIssuer =
                    jwtSection["Issuer"],

                ValidAudience =
                    jwtSection["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();


// ======================================================
// FILE UPLOAD LIMIT
// ======================================================

builder.Services.Configure<
    Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit =
            200 * 1024 * 1024;
    });


// ======================================================
// BUILD APPLICATION
// ======================================================

var app = builder.Build();


// ======================================================
// SWAGGER
// ======================================================

// Enable Swagger on Render as well so API can be tested
app.UseSwagger();

app.UseSwaggerUI();


// ======================================================
// MIDDLEWARE
// ======================================================

// Render handles HTTPS externally.
// Don't force HTTPS redirection inside the container.
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();


// ======================================================
// ENDPOINTS
// ======================================================

app.MapControllers();


// Simple health endpoint
app.MapGet("/", () => Results.Ok(new
{
    status = "running",
    service = "CommunicaAI API"
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy"
}));


// ======================================================
// START APPLICATION
// ======================================================

app.Run();