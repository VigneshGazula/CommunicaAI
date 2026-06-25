# Communica AI - Complete Technical Architecture Reference

**Version:** 1.0  
**Last Updated:** 2025  
**Project Type:** Full-Stack Web Application  
**Tech Stack:** ASP.NET Core 9.0 (Backend) + Angular 18+ (Frontend)

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [System Architecture Overview](#system-architecture-overview)
3. [Database Schema](#database-schema)
4. [Backend API Reference](#backend-api-reference)
5. [Backend Services Reference](#backend-services-reference)
6. [Backend Repositories Reference](#backend-repositories-reference)
7. [Frontend Architecture](#frontend-architecture)
8. [Authentication & Authorization](#authentication--authorization)
9. [External Integrations](#external-integrations)
10. [Dependency Injection Map](#dependency-injection-map)
11. [Configuration Settings](#configuration-settings)
12. [What's Implemented vs What's Missing](#whats-implemented-vs-whats-missing)
13. [Technical Debt Analysis](#technical-debt-analysis)

---

## Executive Summary

Communica AI is an AI-powered interview practice platform with biometric authentication. The system allows users to:
- Register with audio/video biometric enrollment
- Login using password, audio, or video verification
- Create and conduct mock interview sessions
- Get AI-powered evaluation of interview answers
- Track interview history and performance

### Key Technologies
- **Backend:** ASP.NET Core 9.0, Entity Framework Core, PostgreSQL
- **Frontend:** Angular 18+ (Standalone Components), TypeScript
- **AI Services:** Google Gemini API (transcription & evaluation)
- **Media Storage:** Cloudinary
- **Biometric Verification:** Python microservice (audio), Mock implementation (video)
- **Authentication:** JWT Bearer Tokens

---

## System Architecture Overview

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      Angular Frontend                        │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐            │
│  │ Components │  │  Services  │  │   Guards   │            │
│  │            │──│            │  │ Interceptors│            │
│  └────────────┘  └────────────┘  └────────────┘            │
└─────────────────────────────────────────────────────────────┘
                           │ HTTP/REST
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                ASP.NET Core Backend API                      │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐            │
│  │Controllers │──│  Services  │──│Repositories│            │
│  └────────────┘  └────────────┘  └────────────┘            │
│                           │                                  │
│                           ▼                                  │
│                  ┌────────────────┐                         │
│                  │  EF Core DbContext │                      │
│                  └────────────────┘                         │
└─────────────────────────────────────────────────────────────┘
           │                │                │
           ▼                ▼                ▼
    ┌──────────┐    ┌──────────┐    ┌──────────────┐
    │PostgreSQL│    │Cloudinary│    │ Gemini API   │
    │ Database │    │  Media   │    │(Transcription│
    │          │    │ Storage  │    │ & Evaluation)│
    └──────────┘    └──────────┘    └──────────────┘
           │
           ▼
    ┌──────────────┐
    │   Python     │
    │ Verification │
    │   Service    │
    └──────────────┘
```

### Backend Folder Structure

```
CommunicaAI/
├── Controllers/               # API Endpoints (6 files)
│   ├── AuthController.cs
│   ├── InterviewController.cs
│   ├── InterviewAnswerController.cs
│   ├── QuestionBankController.cs
│   ├── TestController.cs
│   └── WeatherForecastController.cs
├── Services/                  # Business Logic (14 files)
│   ├── Interfaces/
│   │   ├── ICloudinaryService.cs
│   │   ├── IGeminiService.cs
│   │   ├── IInterviewAnswerService.cs
│   │   ├── IInterviewQuestionService.cs
│   │   ├── IInterviewResultService.cs
│   │   ├── IInterviewService.cs
│   │   ├── IQuestionBankService.cs
│   │   └── ITranscriptionService.cs
│   ├── BiometricVerificationService.cs
│   ├── CloudinaryService.cs
│   ├── GeminiService.cs
│   ├── GeminiTranscriptionService.cs
│   ├── IBiometricVerificationService.cs
│   ├── InterviewAnswerService.cs
│   ├── InterviewQuestionService.cs
│   ├── InterviewResultService.cs
│   ├── InterviewService.cs
│   ├── IPythonVerificationService.cs
│   ├── ITokenService.cs
│   ├── PythonVerificationService.cs
│   ├── QuestionBankService.cs
│   └── TokenService.cs
├── Repositories/              # Data Access (6 files)
│   ├── Interfaces/
│   │   ├── IAnswerEvaluationRepository.cs
│   │   ├── IInterviewAnswerRepository.cs
│   │   ├── IInterviewQuestionRepository.cs
│   │   ├── IInterviewRepository.cs
│   │   ├── IInterviewResultRepository.cs
│   │   └── IQuestionBankRepository.cs
│   ├── AnswerEvaluationRepository.cs
│   ├── InterviewAnswerRepository.cs
│   ├── InterviewQuestionRepository.cs
│   ├── InterviewRepository.cs
│   ├── InterviewResultRepository.cs
│   └── QuestionBankRepository.cs
├── Models/                    # Entity Models (12 files)
│   ├── Configurations/
│   │   ├── CloudinarySettings.cs
│   │   └── GeminiSettings.cs
│   ├── AnswerEvaluation.cs
│   ├── AppUser.cs
│   ├── InterviewAnswer.cs
│   ├── InterviewQuestion.cs
│   ├── InterviewResult.cs
│   ├── InterviewSession.cs
│   ├── MediaUploadResult.cs
│   ├── PythonVerificationResult.cs
│   ├── PythonVerificationServiceOptions.cs
│   ├── QuestionBank.cs
│   ├── UserMediaProfile.cs
│   └── UserVerificationProfile.cs
├── DTO/                       # Data Transfer Objects (18 files)
│   ├── Auth/
│   │   ├── AudioLoginRequest.cs
│   │   ├── AuthResponse.cs
│   │   ├── PasswordLoginRequest.cs
│   │   ├── RegisterRequest.cs
│   │   └── VideoLoginRequest.cs
│   ├── Interview/
│   │   ├── AnswerResponse.cs
│   │   ├── AnswerSubmitRequest.cs
│   │   ├── CreateInterviewRequest.cs
│   │   ├── CreateInterviewResponse.cs
│   │   ├── InterviewDetailResponse.cs
│   │   ├── InterviewHistoryResponse.cs
│   │   ├── InterviewSessionResponse.cs
│   │   └── QuestionResponse.cs
│   ├── QuestionBank/
│   │   ├── CreateQuestionRequest.cs
│   │   └── QuestionBankResponse.cs
│   ├── Media/
│   │   ├── MediaOnboardingResponse.cs
│   │   └── MediaOnboardingUploadRequest.cs
│   └── Evaluation/
│       └── SubmitAudioAnswerResponse.cs
├── Data/                      # Database Context
│   └── ApplicationDbContext.cs
├── Migrations/                # EF Core Migrations
└── Program.cs                 # Application Entry Point & DI Config
```

---

## Database Schema

### Complete Entity Relationship Diagram

```
┌─────────────────────────────────────────┐
│              AppUser                     │
├─────────────────────────────────────────┤
│ PK  Id: Guid                            │
│     FullName: string(100)               │
│ UQ  Email: string(150)                  │
│     PasswordHash: string                │
│     CreatedAtUtc: DateTime              │
└─────────────────────────────────────────┘
        │                   │
        │ 1:1               │ 1:1
        ▼                   ▼
┌───────────────────┐  ┌───────────────────────┐
│UserVerification   │  │ UserMediaProfile      │
│Profile            │  │ (Not Actively Used)   │
├───────────────────┤  ├───────────────────────┤
│PK Id: Guid        │  │PK Id: Guid            │
│FK UserId: Guid    │  │FK UserId: Guid        │
│  EnrollmentAudioUrl│  │  AudioUrl            │
│  EnrollmentAudio  │  │  AudioPublicId       │
│  PublicId         │  │  AudioContentType    │
│  EnrollmentVideo  │  │  AudioSizeBytes      │
│  Url              │  │  AudioUploadedAtUtc  │
│  EnrollmentVideo  │  │  VideoUrl            │
│  PublicId         │  │  VideoPublicId       │
│  EnrolledAtUtc    │  │  VideoContentType    │
│  UpdatedAtUtc     │  │  VideoSizeBytes      │
└───────────────────┘  │  VideoUploadedAtUtc  │
                        │  CreatedAtUtc        │
                        │  UpdatedAtUtc        │
                        └───────────────────────┘

┌─────────────────────────────────────────┐
│         InterviewSession                 │
├─────────────────────────────────────────┤
│ PK  Id: Guid                            │
│ FK  UserId: Guid (indexed)              │
│     Role: string(100)                   │
│     Topic: string(200)                  │
│     Difficulty: string(50)              │
│     QuestionCount: int                  │
│     DurationMinutes: int                │
│     StartedAt: DateTime                 │
│     CompletedAt: DateTime?              │
│     Status: string(50) = "InProgress"   │
└─────────────────────────────────────────┘
        │
        │ 1:N
        ▼
┌─────────────────────────────────────────┐
│       InterviewQuestion                  │
├─────────────────────────────────────────┤
│ PK  Id: Guid                            │
│ FK  InterviewSessionId: Guid (indexed)  │
│     OrderNumber: int                    │
│     Category: string(50)                │
│     QuestionText: string(1000)          │
│     IsAnswered: bool = false            │
│     CreatedAt: DateTime                 │
└─────────────────────────────────────────┘
        │
        │ 1:1
        ▼
┌─────────────────────────────────────────┐
│        InterviewAnswer                   │
├─────────────────────────────────────────┤
│ PK  Id: Guid                            │
│ FK  InterviewQuestionId: Guid (UQ idx)  │
│ FK  InterviewSessionId: Guid (indexed)  │
│     Transcript: string (text)           │
│     AudioUrl: string?                   │
│     DurationSeconds: int                │
│     AnsweredAt: DateTime                │
└─────────────────────────────────────────┘
        │
        │ 1:1
        ▼
┌─────────────────────────────────────────┐
│       AnswerEvaluation                   │
├─────────────────────────────────────────┤
│ PK  Id: Guid                            │
│ FK  InterviewAnswerId: Guid             │
│     TechnicalScore: int                 │
│     ClarityScore: int                   │
│     CompletenessScore: int              │
│     OverallScore: int                   │
│     Strengths: string                   │
│     Improvements: string                │
│     Feedback: string                    │
│     EvaluatedAt: DateTime               │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│         InterviewResult                  │
├─────────────────────────────────────────┤
│ PK  Id: Guid                            │
│ FK  InterviewSessionId: Guid (UQ idx)   │
│     OverallScore: int                   │
│     TechnicalScore: int                 │
│     CommunicationScore: int             │
│     ConfidenceScore: int                │
│     Strengths: string                   │
│     Weaknesses: string                  │
│     Recommendations: string             │
│     Summary: string                     │
│     GeneratedAt: DateTime               │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│          QuestionBank                    │
├─────────────────────────────────────────┤
│ PK  Id: Guid                            │
│ IDX Role: string(100)                   │
│ IDX Category: string(50)                │
│ IDX Difficulty: string(50)              │
│     QuestionText: string(1000)          │
│     CreatedAt: DateTime                 │
│                                         │
│ Composite Index:                        │
│   (Role, Category, Difficulty)          │
└─────────────────────────────────────────┘
```

### Database Configuration Details

**Connection String:**
```
Host=localhost;Port=5432;Database=CommunicaAIDB;Username=postgres;Password=Vignesh@123
```

**Provider:** Npgsql (PostgreSQL)

**Key Constraints:**
- AppUser.Email: Unique index
- UserVerificationProfile.UserId: Unique index
- InterviewAnswer.InterviewQuestionId: Unique index
- InterviewResult.InterviewSessionId: Unique index

**Cascade Delete Rules:**
- UserVerificationProfile → AppUser (CASCADE)
- InterviewQuestion → InterviewSession (CASCADE)
- InterviewAnswer → InterviewQuestion (CASCADE)
- InterviewAnswer → InterviewSession (CASCADE)
- InterviewResult → InterviewSession (CASCADE)

---

## Backend API Reference

### API Base URL
**Development:** `http://localhost:5169/api`

### Authentication Endpoints

#### POST /api/auth/register
**Purpose:** Register new user with biometric enrollment

**Content-Type:** `multipart/form-data`

**Request Body:**
```
FullName: string (required, max 100)
Email: string (required, email format, max 150)
Password: string (required, min 6)
AudioFile: IFormFile (required, audio file for voice enrollment)
VideoFile: IFormFile (required, video file for face enrollment)
```

**Response (200 OK):**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "fullName": "John Doe",
  "email": "john@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAtUtc": "2025-01-15T12:00:00Z"
}
```

**Error Responses:**
- `409 Conflict` - Email already registered

**Implementation Notes:**
- Uploads audio/video to Cloudinary
- Hashes password using ASP.NET Core Identity PasswordHasher
- Creates UserVerificationProfile with media URLs
- Generates JWT token (2-hour expiration)

---

#### POST /api/auth/login/password
**Purpose:** Login with email and password

**Content-Type:** `application/json`

**Request Body:**
```json
{
  "email": "john@example.com",
  "password": "SecurePassword123"
}
```

**Response (200 OK):**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "fullName": "John Doe",
  "email": "john@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAtUtc": "2025-01-15T12:00:00Z"
}
```

**Error Responses:**
- `401 Unauthorized` - Invalid email or password

---

#### POST /api/auth/login/audio
**Purpose:** Login with email and voice verification

**Content-Type:** `multipart/form-data`

**Request Body:**
```
Email: string (required)
AudioFile: IFormFile (required, audio file to verify)
```

**Response (200 OK):** Same as password login

**Error Responses:**
- `401 Unauthorized` - Invalid email or audio verification failed
- `503 Service Unavailable` - Python verification service unavailable

**Implementation:**
- Fetches enrolled audio URL from UserVerificationProfile
- Calls Python verification service at `http://127.0.0.1:8000/verify-audio`
- Verification service compares audio features
- Returns JWT if verification score passes threshold

---

#### POST /api/auth/login/video
**Purpose:** Login with email and video verification

**Content-Type:** `multipart/form-data`

**Request Body:**
```
Email: string (required)
VideoFile: IFormFile (required, video file to verify)
```

**Response (200 OK):** Same as password login

**Error Responses:**
- `401 Unauthorized` - Invalid email or video verification failed

**Implementation Notes:**
- Currently uses BiometricVerificationService.VerifyVideoAsync
- **STUB IMPLEMENTATION** - always returns true for development
- Needs real facial recognition integration in production

---

#### GET /api/auth/me
**Purpose:** Get current authenticated user profile

**Authorization:** Bearer Token Required

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "fullName": "John Doe",
  "email": "john@example.com"
}
```

**Error Responses:**
- `401 Unauthorized` - Invalid or missing token
- `404 Not Found` - User not found

---

### Interview Session Endpoints

#### POST /api/interviews
**Purpose:** Create new interview session

**Authorization:** Bearer Token Required

**Request Body:**
```json
{
  "role": "Software Engineer",
  "topic": "Technical Interview",
  "difficulty": "medium",
  "questionCount": 5,
  "durationMinutes": 15
}
```

**Validation Rules:**
- `role`: Required
- `topic`: Required
- `difficulty`: Required
- `questionCount`: Required, Range(1, 50)
- `durationMinutes`: Required, Range(1, 180)

**Response (201 Created):**
```json
{
  "sessionId": "7fa85f64-5717-4562-b3fc-2c963f66afa7",
  "status": "InProgress",
  "startedAt": "2025-01-15T10:00:00Z"
}
```

**Implementation:**
- Creates InterviewSession with UserId from JWT
- Generates questions via InterviewQuestionService
- Questions sourced from QuestionBank based on role/difficulty
- Questions distributed: 60% Technical, 20% Behavioral, 20% HR

---

#### GET /api/interviews/{sessionId}
**Purpose:** Get interview session details

**Authorization:** Bearer Token Required

**Path Parameter:**
- `sessionId`: Guid

**Response (200 OK):**
```json
{
  "sessionId": "7fa85f64-5717-4562-b3fc-2c963f66afa7",
  "role": "Software Engineer",
  "topic": "Technical Interview",
  "difficulty": "medium",
  "questionCount": 5,
  "durationMinutes": 15,
  "status": "InProgress",
  "startedAt": "2025-01-15T10:00:00Z",
  "completedAt": null,
  "questions": [...],
  "result": null
}
```

**Error Responses:**
- `404 Not Found` - Session doesn't exist or not authorized

---

#### GET /api/interviews/my-history
**Purpose:** Get user's interview history

**Authorization:** Bearer Token Required

**Response (200 OK):**
```json
[
  {
    "sessionId": "7fa85f64-5717-4562-b3fc-2c963f66afa7",
    "role": "Software Engineer",
    "difficulty": "medium",
    "startedAt": "2025-01-15T10:00:00Z",
    "completedAt": "2025-01-15T10:15:00Z",
    "status": "Completed",
    "completionPercentage": 100.0
  }
]
```

**Notes:**
- Ordered by StartedAt descending (most recent first)
- Includes completion percentage from InterviewResult if available

---

#### GET /api/interviews/{sessionId}/questions
**Purpose:** Get questions for an interview session

**Authorization:** Bearer Token Required

**Path Parameter:**
- `sessionId`: Guid

**Response (200 OK):**
```json
[
  {
    "id": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
    "orderNumber": 1,
    "category": "Technical",
    "questionText": "What is the difference between == and === in JavaScript?",
    "isAnswered": false
  }
]
```

**Error Responses:**
- `404 Not Found` - No questions found or not authorized

---

#### POST /api/interviews/{sessionId}/answers
**Purpose:** Submit text answer to a question

**Authorization:** Bearer Token Required

**Path Parameter:**
- `sessionId`: Guid

**Request Body:**
```json
{
  "questionId": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
  "transcript": "The answer is..."
}
```

**Validation:**
- `questionId`: Required
- `transcript`: Required, MinLength(1), MaxLength(5000)

**Response (200 OK):**
```json
{
  "id": "9fa85f64-5717-4562-b3fc-2c963f66afa9",
  "questionId": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
  "transcript": "The answer is...",
  "answeredAt": "2025-01-15T10:05:00Z"
}
```

**Error Responses:**
- `400 Bad Request` - Invalid request or question already answered
- `404 Not Found` - Session not found or not authorized

---

#### POST /api/interviews/{sessionId}/answers/audio
**Purpose:** Submit audio answer with transcription and AI evaluation

**Authorization:** Bearer Token Required

**Content-Type:** `multipart/form-data`

**Path Parameter:**
- `sessionId`: Guid

**Request Body:**
```
questionId: Guid (required)
audioFile: IFormFile (required)
durationSeconds: int (required)
```

**Response (200 OK):**
```json
{
  "answerId": "9fa85f64-5717-4562-b3fc-2c963f66afa9",
  "transcript": "My approach to this problem would be...",
  "audioUrl": "https://res.cloudinary.com/...",
  "technicalScore": 85,
  "clarityScore": 90,
  "completenessScore": 88,
  "overallScore": 87,
  "feedback": "Strong technical knowledge demonstrated..."
}
```

**Implementation Flow:**
1. Validates session and question ownership
2. Uploads audio to Cloudinary
3. Transcribes audio using Gemini API
4. Evaluates answer using Gemini API
5. Creates InterviewAnswer record
6. Creates AnswerEvaluation record
7. Marks question as answered

**Services Used:**
- CloudinaryService.UploadAudioAsync
- GeminiTranscriptionService.TranscribeAsync
- GeminiService.EvaluateAnswerAsync

---

#### POST /api/interviews/{sessionId}/complete
**Purpose:** Mark interview as completed and generate results

**Authorization:** Bearer Token Required

**Path Parameter:**
- `sessionId`: Guid

**Response (200 OK):**
```json
{
  "message": "Interview completed successfully."
}
```

**Implementation:**
- Sets CompletedAt timestamp
- Sets Status to "Completed"
- Generates InterviewResult via InterviewResultService

**Error Responses:**
- `404 Not Found` - Session not found or not authorized

---

### Question Bank Endpoints

#### POST /api/question-bank
**Purpose:** Create new question in question bank

**Authorization:** Bearer Token Required

**Request Body:**
```json
{
  "role": "Software Engineer",
  "category": "Technical",
  "difficulty": "medium",
  "questionText": "Explain the concept of async/await in JavaScript."
}
```

**Validation:**
- All fields required
- `role`: MaxLength(100)
- `category`: MaxLength(50)
- `difficulty`: MaxLength(50)
- `questionText`: MaxLength(1000)

**Response (201 Created):**
```json
{
  "id": "afa85f64-5717-4562-b3fc-2c963f66afaa",
  "role": "Software Engineer",
  "category": "Technical",
  "difficulty": "medium",
  "questionText": "Explain the concept of async/await in JavaScript.",
  "createdAt": "2025-01-15T10:00:00Z"
}
```

---

#### GET /api/question-bank/{id}
**Purpose:** Get question by ID

**Authorization:** Bearer Token Required

**Response (200 OK):** Same as create response

**Error Responses:**
- `404 Not Found` - Question not found

---

#### GET /api/question-bank
**Purpose:** Get all questions in bank

**Authorization:** Bearer Token Required

**Response (200 OK):** Array of question objects

**Notes:** Ordered by Role, then Category

---

#### DELETE /api/question-bank/{id}
**Purpose:** Delete question from bank

**Authorization:** Bearer Token Required

**Response (200 OK):**
```json
{
  "message": "Question deleted successfully"
}
```

---

#### POST /api/question-bank/seed
**Purpose:** Seed question bank with default questions

**Authorization:** Bearer Token Required

**Response (200 OK):**
```json
{
  "message": "Questions seeded successfully"
}
```

**Notes:**
- Only seeds if question bank is empty
- Includes 100+ pre-defined questions across multiple roles
- Covers Technical, Behavioral, and HR categories
- Difficulty levels: Easy, Medium, Hard
- Roles: Software Engineer, Backend Developer, Frontend Developer, 
  Data Scientist, DevOps Engineer, Cloud Engineer, Full Stack Developer, 
  Data Analyst, Machine Learning Engineer

---

### Test Endpoints

#### POST /api/test/transcribe
**Purpose:** Test audio transcription

**Content-Type:** `multipart/form-data`

**Request Body:**
```
audioFile: IFormFile (required)
```

**Response (200 OK):**
```json
{
  "transcript": "The transcribed text..."
}
```

**Notes:** Uses GeminiTranscriptionService

---

## Backend Services Reference

### Authentication Services

#### TokenService
**Interface:** `ITokenService`  
**Implementation:** `TokenService`  
**Lifetime:** Scoped

**Purpose:** Creates JWT tokens for authentication

**Method:**
```csharp
(string Token, DateTime ExpiresAtUtc) CreateToken(AppUser user)
```

**Implementation Details:**
- Uses HS256 algorithm
- 2-hour token expiration
- Claims included:
  - Subject (NameIdentifier): User.Id
  - Email: User.Email
  - UniqueName: User.FullName
  - Jti: Random Guid
- Key, Issuer, Audience from configuration

**Configuration:**
```json
{
  "Jwt": {
    "Issuer": "CommunicaAI",
    "Audience": "CommunicaAIUsers",
    "Key": "THIS_IS_A_DEMO_SECRET_KEY_CHANGE_IT_TO_A_LONG_RANDOM_SECRET"
  }
}
```

---

#### BiometricVerificationService
**Interface:** `IBiometricVerificationService`  
**Implementation:** `BiometricVerificationService`  
**Lifetime:** Scoped

**Purpose:** Verifies user identity via audio/video biometrics

**Methods:**
```csharp
Task<bool> VerifyAudioAsync(UserVerificationProfile profile, IFormFile sampleAudio)
Task<bool> VerifyVideoAsync(UserVerificationProfile profile, IFormFile sampleVideo)
```

**Current Status:**
- VerifyAudioAsync: Uses PythonVerificationService (real implementation)
- VerifyVideoAsync: **STUB** - returns true (needs implementation)

**TODO:** Implement facial recognition for video verification

---

#### PythonVerificationService
**Interface:** `IPythonVerificationService`  
**Implementation:** `PythonVerificationService`  
**Lifetime:** Scoped

**Purpose:** Calls external Python service for speaker verification

**Method:**
```csharp
Task<PythonVerificationResult> VerifyAudioAsync(
    string enrolledAudioUrl,
    IFormFile sampleAudio,
    CancellationToken cancellationToken = default)
```

**Implementation:**
1. Downloads enrolled audio from Cloudinary URL
2. Uploads both enrolled and sample audio to Python service
3. Calls POST http://127.0.0.1:8000/verify-audio
4. Parses response JSON
5. Returns verification result with score

**Configuration:**
```json
{
  "PythonVerificationService": {
    "BaseUrl": "http://127.0.0.1:8000",
    "VerifyAudioPath": "/verify-audio"
  }
}
```

**Response Format:**
```json
{
  "verified": true,
  "score": 0.95
}
```

**Dependencies:** HttpClient named "PythonVerification"

---

### Media Services

#### CloudinaryService
**Interface:** `ICloudinaryService`  
**Implementation:** `CloudinaryService`  
**Lifetime:** Scoped

**Purpose:** Uploads and manages media files in Cloudinary

**Methods:**
```csharp
Task<MediaUploadResult> UploadAudioAsync(IFormFile file, Guid userId)
Task<MediaUploadResult> UploadVideoAsync(IFormFile file, Guid userId)
Task DeleteAsync(string publicId, ResourceType resourceType)
```

**Implementation:**
- Uses CloudinaryDotNet library
- Uploads to folders: `communica-ai/users/{userId}/audio` or `/video`
- Generates unique filenames
- Returns secure URLs

**Configuration:**
```json
{
  "CloudinarySettings": {
    "CloudName": "cloudname",
    "ApiKey": "apikey",
    "ApiSecret": "apisecret"
  }
}
```

**Dependencies:** `IOptions<CloudinarySettings>`

---

### AI Services

#### GeminiService
**Interface:** `IGeminiService`  
**Implementation:** `GeminiService`  
**Lifetime:** Scoped

**Purpose:** Evaluates interview answers using Google Gemini API

**Method:**
```csharp
Task<SubmitAudioAnswerResponse> EvaluateAnswerAsync(string question, string answer)
```

**Implementation:**
1. Constructs evaluation prompt
2. Calls Gemini API: `https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent`
3. Parses JSON response
4. Returns evaluation scores and feedback

**Prompt Structure:**
```
You are a senior technical interviewer.

Question: {question}
Candidate Answer: {answer}

Evaluate the answer.
Return ONLY a JSON object with these fields:
technicalScore, clarityScore, completenessScore, overallScore, 
strengths, improvements, feedback

Do not include markdown or explanations.
```

**Response Fields:**
- technicalScore (int)
- clarityScore (int)
- completenessScore (int)
- overallScore (int)
- strengths (string)
- improvements (string)
- feedback (string)

**Configuration:**
```json
{
  "Gemini": {
    "ApiKey": "YOUR_API_KEY",
    "Model": "gemini-2.5-flash"
  }
}
```

**Dependencies:** HttpClient, `IOptions<GeminiSettings>`

---

#### GeminiTranscriptionService
**Interface:** `ITranscriptionService`  
**Implementation:** `GeminiTranscriptionService`  
**Lifetime:** Scoped

**Purpose:** Transcribes audio to text using Gemini API

**Method:**
```csharp
Task<string> TranscribeAsync(Stream audioStream, string contentType)
```

**Implementation:**
1. Reads audio stream to byte array
2. Converts to base64
3. Sends to Gemini API with transcription prompt
4. Extracts and returns transcript text

**Prompt:**
```
Transcribe the following interview answer. 
Return only the transcript text. 
Do not add explanations.
```

**API Call:**
- Endpoint: `https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent`
- Content type passed as mime_type
- Audio data as base64 inline_data

**Dependencies:** HttpClient, `IOptions<GeminiSettings>`

---

### Interview Services

#### InterviewService
**Interface:** `IInterviewService`  
**Implementation:** `InterviewService`  
**Lifetime:** Scoped

**Purpose:** Manages interview session lifecycle

**Methods:**
```csharp
Task<CreateInterviewResponse> CreateInterviewAsync(Guid userId, CreateInterviewRequest request)
Task<InterviewSessionResponse?> GetInterviewAsync(Guid sessionId, Guid userId)
Task<List<InterviewSessionResponse>> GetUserInterviewsAsync(Guid userId)
Task<bool> CompleteInterviewAsync(Guid sessionId, Guid userId)
Task<List<InterviewHistoryResponse>> GetUserHistoryAsync(Guid userId)
Task<InterviewDetailResponse?> GetInterviewDetailsAsync(Guid sessionId, Guid userId)
```

**Key Business Logic:**

**CreateInterviewAsync:**
- Creates session with UserId, Status="InProgress", StartedAt=UtcNow
- Delegates question generation to InterviewQuestionService
- Returns session ID and metadata

**CompleteInterviewAsync:**
- Sets CompletedAt, Status="Completed"
- Generates result via InterviewResultService

**GetInterviewDetailsAsync:**
- Loads session, questions, answers, and result
- Joins data for complete interview view
- Maps to detailed DTO with nested collections

**Dependencies:**
- IInterviewRepository
- IInterviewQuestionService
- IInterviewResultService
- IInterviewQuestionRepository
- IInterviewAnswerRepository
- IInterviewResultRepository

---

#### InterviewQuestionService
**Interface:** `IInterviewQuestionService`  
**Implementation:** `InterviewQuestionService`  
**Lifetime:** Scoped

**Purpose:** Generates and retrieves interview questions

**Methods:**
```csharp
Task<List<QuestionResponse>> GetSessionQuestionsAsync(Guid sessionId, Guid userId)
Task GenerateQuestionsForSessionAsync(Guid sessionId, string role, string difficulty, int questionCount)
```

**Key Business Logic - GenerateQuestionsForSessionAsync:**

**Question Distribution:**
- 60% Technical questions
- 20% Behavioral questions
- 20% HR questions

**Algorithm:**
1. Calculate count per category (Math.Ceiling)
2. Fetch random questions from QuestionBank by role/difficulty/category
3. Create InterviewQuestion entities with OrderNumber
4. If insufficient questions, fetch fallback questions (any category)
5. Bulk insert via CreateRangeAsync

**Random Selection:**
- Uses Random().Next() for shuffling
- Takes requested count via LINQ .Take()

**Dependencies:**
- IInterviewQuestionRepository
- IInterviewRepository
- IQuestionBankRepository

---

#### InterviewAnswerService
**Interface:** `IInterviewAnswerService`  
**Implementation:** `InterviewAnswerService`  
**Lifetime:** Scoped

**Purpose:** Handles answer submission and evaluation

**Methods:**
```csharp
Task<AnswerResponse> SubmitAnswerAsync(Guid sessionId, Guid userId, AnswerSubmitRequest request)
Task<SubmitAudioAnswerResponse> SubmitAudioAnswerAsync(
    Guid sessionId, Guid questionId, IFormFile audioFile, int durationSeconds, Guid userId)
```

**Key Flow - SubmitAudioAnswerAsync:**
1. Validate session ownership
2. Validate question belongs to session
3. Check for duplicate answer
4. Upload audio to Cloudinary
5. Transcribe audio via ITranscriptionService
6. Evaluate answer via IGeminiService
7. Create InterviewAnswer record
8. Create AnswerEvaluation record
9. Mark question as answered

**Validation:**
- Session must exist and belong to user
- Question must belong to session
- Question must not already be answered

**Dependencies:**
- IInterviewAnswerRepository
- IInterviewQuestionRepository
- IInterviewRepository
- ITranscriptionService (Gemini)
- IGeminiService
- ICloudinaryService
- IAnswerEvaluationRepository

---

#### InterviewResultService
**Interface:** `IInterviewResultService`  
**Implementation:** `InterviewResultService`  
**Lifetime:** Scoped

**Purpose:** Generates interview completion results

**Method:**
```csharp
Task<InterviewResultResponse> GenerateResultAsync(Guid sessionId)
```

**Implementation:**
- Checks for existing result (returns if found)
- Counts total questions and answered questions
- Calculates completion percentage
- Creates InterviewResult record
- Maps to response DTO

**Note:** Current implementation tracks basic completion metrics. 
Detailed scoring (TechnicalScore, CommunicationScore, ConfidenceScore, etc.) 
stored but not calculated from AnswerEvaluations yet.

**Dependencies:**
- IInterviewResultRepository
- IInterviewQuestionRepository

---

#### QuestionBankService
**Interface:** `IQuestionBankService`  
**Implementation:** `QuestionBankService`  
**Lifetime:** Scoped

**Purpose:** Manages question bank CRUD operations and seeding

**Methods:**
```csharp
Task<QuestionBankResponse> CreateQuestionAsync(CreateQuestionRequest request)
Task<QuestionBankResponse?> GetQuestionByIdAsync(Guid id)
Task<List<QuestionBankResponse>> GetAllQuestionsAsync()
Task<bool> DeleteQuestionAsync(Guid id)
Task SeedQuestionsAsync()
```

**SeedQuestionsAsync Details:**
- Seeds 100+ pre-defined questions
- Only runs if question bank is empty
- Covers 8+ roles:
  - Software Engineer
  - Backend Developer
  - Frontend Developer
  - Data Scientist
  - DevOps Engineer
  - Cloud Engineer
  - Full Stack Developer
  - Data Analyst
  - Machine Learning Engineer
- 3 categories: Technical, Behavioral, HR
- 3 difficulty levels: Easy, Medium, Hard

**Dependencies:** IQuestionBankRepository

---

## Backend Repositories Reference

All repositories follow Entity Framework Core pattern with ApplicationDbContext.

### InterviewRepository
**Interface:** `IInterviewRepository`  
**Methods:**
```csharp
Task<InterviewSession> CreateAsync(InterviewSession session)
Task<InterviewSession?> GetByIdAsync(Guid sessionId)
Task<List<InterviewSession>> GetByUserIdAsync(Guid userId)
Task UpdateAsync(InterviewSession session)
```

**Query Optimizations:**
- GetByUserIdAsync: Ordered by StartedAt DESC

---

### InterviewQuestionRepository
**Interface:** `IInterviewQuestionRepository`  
**Methods:**
```csharp
Task<InterviewQuestion> CreateAsync(InterviewQuestion question)
Task<List<InterviewQuestion>> CreateRangeAsync(List<InterviewQuestion> questions)
Task<InterviewQuestion?> GetByIdAsync(Guid id)
Task<List<InterviewQuestion>> GetBySessionIdAsync(Guid sessionId)
Task<InterviewQuestion?> GetBySessionAndQuestionIdAsync(Guid sessionId, Guid questionId)
Task UpdateAsync(InterviewQuestion question)
Task<int> GetAnsweredCountAsync(Guid sessionId)
```

**Query Optimizations:**
- GetBySessionIdAsync: Ordered by OrderNumber
- GetAnsweredCountAsync: Filters by IsAnswered flag

---

### InterviewAnswerRepository
**Interface:** `IInterviewAnswerRepository`  
**Methods:**
```csharp
Task<InterviewAnswer> CreateAsync(InterviewAnswer answer)
Task<InterviewAnswer?> GetByQuestionIdAsync(Guid questionId)
Task<List<InterviewAnswer>> GetBySessionIdAsync(Guid sessionId)
```

**Query Optimizations:**
- GetBySessionIdAsync: Ordered by AnsweredAt

---

### InterviewResultRepository
**Interface:** `IInterviewResultRepository`  
**Methods:**
```csharp
Task<InterviewResult> CreateAsync(InterviewResult result)
Task<InterviewResult?> GetBySessionIdAsync(Guid sessionId)
```

---

### QuestionBankRepository
**Interface:** `IQuestionBankRepository`  
**Methods:**
```csharp
Task<QuestionBank> CreateAsync(QuestionBank question)
Task<QuestionBank?> GetByIdAsync(Guid id)
Task<List<QuestionBank>> GetAllAsync()
Task<List<QuestionBank>> GetByRoleAndDifficultyAsync(string role, string difficulty)
Task<List<QuestionBank>> GetByRoleDifficultyAndCategoryAsync(string role, string difficulty, string category)
Task UpdateAsync(QuestionBank question)
Task DeleteAsync(Guid id)
```

**Query Optimizations:**
- GetAllAsync: Ordered by Role, then Category
- Composite index on (Role, Category, Difficulty) for fast filtering

---

### AnswerEvaluationRepository
**Interface:** `IAnswerEvaluationRepository`  
**Methods:**
```csharp
Task<AnswerEvaluation?> GetByAnswerIdAsync(Guid answerId)
Task<AnswerEvaluation> CreateAsync(AnswerEvaluation evaluation)
```

---

## Frontend Architecture

### Technology Stack
- **Framework:** Angular 18+ (Standalone Components)
- **Language:** TypeScript 5.x
- **Build Tool:** Vite
- **Styling:** SCSS
- **State Management:** Signals (Angular 18 feature)
- **HTTP Client:** Angular HttpClient with Interceptors

### Frontend Folder Structure

```
Frontend/src/app/
├── core/                      # Core Module
│   ├── guards/
│   │   └── auth.guard.ts     # Route protection
│   ├── interceptors/
│   │   └── auth.interceptor.ts # JWT injection
│   ├── models/
│   │   ├── auth.models.ts    # Auth types
│   │   └── interview.models.ts # Interview types
│   └── services/
│       ├── auth.service.ts
│       ├── interview.service.ts
│       ├── interview-history.service.ts
│       └── speech-transcription.service.ts
├── features/                  # Feature Modules
│   ├── auth/
│   │   ├── login/
│   │   │   ├── login.component.ts
│   │   │   ├── login.component.html
│   │   │   └── login.component.scss
│   │   └── register/
│   │       ├── register.component.ts
│   │       ├── register.component.html
│   │       └── register.component.scss
│   ├── dashboard/
│   │   ├── dashboard.component.ts
│   │   ├── dashboard.component.html
│   │   └── dashboard.component.scss
│   └── interview/
│       ├── setup/
│       ├── live/
│       ├── result/
│       └── history/
├── app.config.ts             # App configuration
├── app.routes.ts             # Route definitions
└── app.ts                    # Root component
```

### Routing Configuration

**Routes:** (from app.routes.ts)

```typescript
'' → redirect to 'login'
'/login' → LoginComponent (public)
'/register' → RegisterComponent (public)
'/dashboard' → DashboardComponent (protected)
'/interview/setup' → SetupComponent (protected)
'/interview/live/:sessionId' → LiveComponent (protected)
'/interview/result/:sessionId' → ResultComponent (protected)
'/history' → HistoryComponent (protected)
'**' → redirect to 'login'
```

**Protected Routes:** Use `authGuard` (canActivate)

**Lazy Loading:** All feature components use dynamic imports

---

### Frontend Core Services

#### AuthService
**Path:** `core/services/auth.service.ts`  
**Injectable:** Root

**Purpose:** Manages authentication state and API calls

**Methods:**
```typescript
register(formData: FormData): Observable<AuthResponse>
loginPassword(payload: {email, password}): Observable<AuthResponse>
loginAudio(formData: FormData): Observable<AuthResponse>
loginVideo(formData: FormData): Observable<AuthResponse>
me(): Observable<UserProfile>
saveTokenSync(token: string): void
getToken(): string | null
isLoggedIn(): boolean
logout(): void
```

**Storage:** Uses localStorage for JWT token (browser only, SSR-safe)

**API Endpoints:**
- POST /api/auth/register
- POST /api/auth/login/password
- POST /api/auth/login/audio
- POST /api/auth/login/video
- GET /api/auth/me

---

#### InterviewService
**Path:** `core/services/interview.service.ts`  
**Injectable:** Root

**Purpose:** Manages interview session lifecycle (MOCK IMPLEMENTATION)

**Important:** This service is currently a **MOCK** implementation using localStorage. 
It does NOT call the backend API. It generates fake questions and stores session data locally.

**Methods:**
```typescript
createSession(setup: InterviewSetup): Observable<InterviewSession>
getCurrentSession(): InterviewSession | null
saveAnswer(sessionId: string, answer: InterviewAnswer): Observable<void>
saveTranscript(sessionId: string, questionId: string, transcript: string): Observable<void>
updateQuestionIndex(sessionId: string, index: number): Observable<void>
finishSession(sessionId: string): Observable<InterviewResult>
```

**Mock Question Bank:**
- Software Engineer: 5 questions
- Product Manager: 5 questions
- Data Scientist: 5 questions
- Marketing Manager: 5 questions

**Mock Result Generation:**
- Calculates completion rate
- Generates scores based on answer length
- Creates mock strengths and improvements

**Storage Key:** `communica_current_session`

**TODO:** Replace with real backend API calls

---

#### InterviewHistoryService
**Path:** `core/services/interview-history.service.ts`  
**Injectable:** Root

**Purpose:** Stores and retrieves interview history (MOCK IMPLEMENTATION)

**Important:** Uses localStorage, NOT backend API

**Methods:**
```typescript
listSessions(): Observable<InterviewResult[]>
getSessionById(id: string): Observable<InterviewResult | null>
saveSession(result: InterviewResult): Observable<void>
getStats(): Observable<InterviewStats>
clearHistory(): Observable<void>
```

**Storage Key:** `communica_interview_history`

**Stats Calculation:**
- totalInterviews: count of sessions
- averageScore: mean of all overallScores
- currentStreak: consecutive days with interviews

**TODO:** Replace with backend API integration

---

#### SpeechTranscriptionService
**Path:** `core/services/speech-transcription.service.ts`  
**Injectable:** Root

**Purpose:** Transcribes audio to text (MOCK IMPLEMENTATION)

**Method:**
```typescript
transcribe(audioBlob: Blob): Observable<TranscriptionResult>
```

**Current Behavior:**
- Returns random template text
- Simulates 800ms network delay
- Does NOT call backend API

**Mock Templates:**
- 5 pre-defined response templates
- Randomly selected based on blob size

**TODO:** Implement real backend API call to /api/test/transcribe

---

### Guards and Interceptors

#### AuthGuard
**Path:** `core/guards/auth.guard.ts`  
**Type:** CanActivateFn

**Purpose:** Protects routes requiring authentication

**Logic:**
1. During SSR, allows request (no localStorage)
2. In browser, checks AuthService.isLoggedIn()
3. Redirects to /login if not authenticated

**Used On:**
- /dashboard
- /interview/setup
- /interview/live/:sessionId
- /interview/result/:sessionId
- /history

---

#### AuthInterceptor
**Path:** `core/interceptors/auth.interceptor.ts`  
**Type:** HttpInterceptorFn

**Purpose:** Injects JWT token into HTTP requests

**Logic:**
1. Gets token from AuthService
2. If token exists, clones request with Authorization header
3. Format: `Bearer {token}`

**Registered In:** app.config.ts via `provideHttpClient(withInterceptors([authInterceptor]))`

---

### Frontend Components

#### LoginComponent
**Path:** `features/auth/login/login.component.ts`

**Features:**
- 3 login modes: password, audio, video
- Password form: email + password
- Audio mode: email + audio recording
- Video mode: email + video recording
- MediaRecorder API for capture
- Supports audio/video mime types: webm with codecs
- Real-time preview for video
- Recording state management
- Error handling

**State Signals:**
- mode: 'password' | 'audio' | 'video'
- recordingState: 'idle' | 'recording' | 'stopped'
- loading, error

**Recording Flow:**
1. Request camera/microphone permissions
2. Start MediaRecorder
3. Collect chunks
4. Stop recorder, create Blob
5. Submit FormData to backend

**Forms:**
- ReactiveFormsModule
- Email + password validation
- Email-only validation for biometric modes

---

#### RegisterComponent
**Path:** `features/auth/register/register.component.ts`

**Multi-Step Registration:**
1. **Step 1 (form):** Name, Email, Password
2. **Step 2 (video):** 5-second face capture
3. **Step 3 (audio):** 5-second voice recording (reads funny quote)
4. **Step 4 (review):** Confirm and submit

**Features:**
- Auto-stop recording at 5 seconds
- Countdown timers
- Retake functionality
- Funny programming quotes for audio enrollment
- Video preview during recording
- FormData submission with all 3 parts

**State Signals:**
- step: 'form' | 'video' | 'audio' | 'review'
- videoReady, videoRecording, videoTimeLeft
- audioRecording, audioTimeLeft
- funnyQuote

**Validation:**
- Name: required
- Email: required, email format
- Password: required, min 6 characters

---

#### DashboardComponent
**Path:** `features/dashboard/dashboard.component.ts`

**Purpose:** Main landing page after login

**Features:**
- Displays user profile
- Shows interview statistics:
  - Total interviews
  - Average score
  - Current streak
- Lists 3 most recent interview sessions
- Navigation to:
  - Start new interview
  - View history
- Logout functionality

**Data Loading:**
- Uses forkJoin to load multiple observables
- Loads user profile, stats, and recent sessions in parallel
- Redirects to login on auth failure

**State Signals:**
- user, stats, recentSessions
- loading, error

---

#### SetupComponent
**Path:** `features/interview/setup/setup.component.ts`

**Purpose:** Configure new interview session

**Form Fields:**
- Role: Dropdown (8 predefined roles)
- Topic: Text input
- Difficulty: Radio (easy/medium/hard)
- Duration: Number input (5-60 minutes)
- Question Count: Number input (1-20)

**Default Values:**
- Role: Software Engineer
- Topic: Technical Interview
- Difficulty: medium
- Duration: 15 minutes
- Question Count: 5

**Available Roles:**
- Software Engineer
- Product Manager
- Data Scientist
- Marketing Manager
- UX Designer
- Business Analyst
- Sales Executive
- Customer Success Manager

**Flow:**
1. User fills form
2. Submit → InterviewService.createSession()
3. Navigate to /interview/live/:sessionId

---

#### LiveComponent
**Path:** `features/interview/live/live.component.ts`

**Purpose:** Conduct live interview session

**Major Features:**
1. **Timer:** Countdown from duration
2. **TTS:** Text-to-speech for questions (Web Speech API)
3. **Recording:** Audio recording via MediaRecorder
4. **Transcription:** Auto-transcribe recorded audio
5. **Navigation:** Next/Previous question
6. **State Management:** Tracks current question index

**Speech States:**
- 'idle': Ready
- 'ai-speaking': Question being spoken
- 'user-turn': Waiting for user
- 'user-recording': Recording answer

**Features:**
- Auto-speak questions when loaded
- Show/hide captions toggle
- Real-time transcript display
- Clear transcript button
- Record/Stop recording
- Previous/Next navigation
- Finish interview early

**Implementation Notes:**
- Uses Web Speech Synthesis API (speechSynthesis)
- MediaRecorder for audio capture
- SpeechTranscriptionService for mock transcription
- localStorage for state persistence
- Timer auto-finishes interview when time expires

**Cleanup:**
- Stops timers on destroy
- Stops speech synthesis
- Releases MediaRecorder and streams

---

#### ResultComponent
**Path:** `features/interview/result/result.component.ts`

**Purpose:** Display interview results

**Data Displayed:**
- Overall score with colored badge
- Communication score
- Confidence score
- Strengths (list)
- Improvements (list)
- Full transcript (collapsible)
- Interview metadata (role, difficulty, date)

**Features:**
- Copy transcript to clipboard
- Navigate back to dashboard
- Visual score indicators (colors based on score ranges)

**Score Colors:**
- 80+: Green (#10b981)
- 60-79: Yellow (#f59e0b)
- <60: Red (#ef4444)

---

#### HistoryComponent
**Path:** `features/interview/history/history.component.ts`

**Purpose:** List all past interview sessions

**Data Displayed:**
- Session date
- Role
- Difficulty
- Overall score (with colored badge)
- Status

**Features:**
- Click session to view results
- Badge colors based on score
- Sorted by date (newest first)

**Badge Classes:**
- badge-success: 80+
- badge-warning: 60-79
- badge-danger: <60

---

## Authentication & Authorization

### JWT Token Structure

**Issuer:** CommunicaAI  
**Audience:** CommunicaAIUsers  
**Algorithm:** HS256  
**Expiration:** 2 hours from issue

**Claims:**
```json
{
  "sub": "user-guid-here",           // ClaimTypes.NameIdentifier
  "email": "user@example.com",       // ClaimTypes.Email
  "unique_name": "John Doe",         // ClaimTypes.UniqueName
  "jti": "random-guid-here"          // JwtRegisteredClaimNames.Jti
}
```

**Header:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

### Backend JWT Configuration

**Program.cs:**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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
```

**Controller Usage:**
```csharp
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InterviewController : ControllerBase
{
    // Extract user ID from claims
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!Guid.TryParse(userIdClaim, out var userId))
    {
        return Unauthorized(new { message = "Invalid token." });
    }
}
```

---

### Frontend JWT Handling

**Storage:**
```typescript
// Save token
localStorage.setItem('token', token);

// Get token
const token = localStorage.getItem('token');

// Remove token
localStorage.removeItem('token');
```

**Automatic Injection:**
```typescript
// auth.interceptor.ts
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (token) {
    const cloned = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
    return next(cloned);
  }

  return next(req);
};
```

**Route Protection:**
```typescript
// auth.guard.ts
export const authGuard: CanActivateFn = () => {
  if (!isPlatformBrowser(inject(PLATFORM_ID))) {
    return true; // SSR bypass
  }

  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.isLoggedIn()) {
    return true;
  }

  return router.createUrlTree(['/login']);
};
```

---

## External Integrations

### Cloudinary Integration

**Purpose:** Media storage for audio/video files

**Configuration:**
```json
{
  "CloudinarySettings": {
    "CloudName": "cloudname",
    "ApiKey": "apikey",
    "ApiSecret": "apisecret"
  }
}
```

**Folder Structure:**
```
communica-ai/
└── users/
    └── {userId}/
        ├── audio/
        │   └── {unique-filename}.webm
        └── video/
            └── {unique-filename}.webm
```

**Upload Process:**
1. Receive IFormFile from controller
2. Open stream
3. Create VideoUploadParams (used for both audio/video)
4. Set folder path with user ID
5. Enable UniqueFilename
6. Upload via CloudinaryDotNet client
7. Return secure URL and public ID

**Delete Process:**
- Uses public ID
- Specifies ResourceType (Audio/Video)
- Calls DestroyAsync

**NuGet Package:** CloudinaryDotNet

---

### Google Gemini AI Integration

**Purpose:** Audio transcription and answer evaluation

**API Base URL:**
```
https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent
```

**Model:** gemini-2.5-flash

**Configuration:**
```json
{
  "Gemini": {
    "ApiKey": "YOUR_API_KEY",
    "Model": "gemini-2.5-flash"
  }
}
```

**Use Cases:**

1. **Transcription:**
   - Endpoint: POST :generateContent
   - Input: Base64-encoded audio + mime type
   - Prompt: "Transcribe the following interview answer..."
   - Output: Plain text transcript

2. **Answer Evaluation:**
   - Endpoint: POST :generateContent
   - Input: Question text + answer text
   - Prompt: "You are a senior technical interviewer. Evaluate..."
   - Output: JSON with scores and feedback

**Request Format (Transcription):**
```json
{
  "contents": [
    {
      "parts": [
        {
          "text": "Transcribe the following interview answer..."
        },
        {
          "inline_data": {
            "mime_type": "audio/webm",
            "data": "<base64-encoded-audio>"
          }
        }
      ]
    }
  ]
}
```

**Response Format:**
```json
{
  "candidates": [
    {
      "content": {
        "parts": [
          {
            "text": "The transcribed text here..."
          }
        ]
      }
    }
  ]
}
```

**Evaluation Response:**
```json
{
  "technicalScore": 85,
  "clarityScore": 90,
  "completenessScore": 88,
  "overallScore": 87,
  "strengths": "Strong technical understanding...",
  "improvements": "Could elaborate more on...",
  "feedback": "Overall excellent response..."
}
```

---

### Python Verification Service

**Purpose:** Speaker verification for audio login

**Technology:** External Python microservice (not part of this codebase)

**Endpoint:** POST http://127.0.0.1:8000/verify-audio

**Request:**
```
Content-Type: multipart/form-data

enrolled_audio: file (enrolled audio from Cloudinary)
sample_audio: file (login attempt audio)
```

**Response:**
```json
{
  "verified": true,
  "score": 0.95
}
```

**Integration Flow:**
1. Backend receives audio login request
2. Fetches enrolled_audio URL from UserVerificationProfile
3. Downloads enrolled audio from Cloudinary
4. Creates multipart form with both audio files
5. Posts to Python service
6. Parses verification result
7. Returns success/failure to client

**Error Handling:**
- Catches HttpRequestException
- Returns 503 Service Unavailable if Python service is down
- Returns descriptive error message

**Configuration:**
```json
{
  "PythonVerificationService": {
    "BaseUrl": "http://127.0.0.1:8000",
    "VerifyAudioPath": "/verify-audio"
  }
}
```

**HttpClient Configuration:**
```csharp
builder.Services.AddHttpClient("PythonVerification", client =>
{
    var baseUrl = builder.Configuration["PythonVerificationService:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
    client.Timeout = TimeSpan.FromMinutes(2);
});
```

---

## Dependency Injection Map

### Complete DI Registration (Program.cs)

**Database:**
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

**Configuration Options:**
```csharp
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("Gemini"));

builder.Services.Configure<PythonVerificationServiceOptions>(
    builder.Configuration.GetSection("PythonVerificationService"));
```

**HttpClients:**
```csharp
builder.Services.AddHttpClient("PythonVerification", client => { ... });
builder.Services.AddHttpClient(); // Default client
```

**Services (Scoped):**
```csharp
// Python & Biometric Services
builder.Services.AddScoped<IPythonVerificationService, PythonVerificationService>();
builder.Services.AddScoped<IBiometricVerificationService, BiometricVerificationService>();

// Media Services
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// Auth Services
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
builder.Services.AddScoped<ITokenService, TokenService>();

// AI Services
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<ITranscriptionService, GeminiTranscriptionService>();

// Interview Services
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IInterviewQuestionRepository, InterviewQuestionRepository>();
builder.Services.AddScoped<IInterviewQuestionService, InterviewQuestionService>();
builder.Services.AddScoped<IInterviewAnswerRepository, InterviewAnswerRepository>();
builder.Services.AddScoped<IInterviewAnswerService, InterviewAnswerService>();
builder.Services.AddScoped<IInterviewResultRepository, InterviewResultRepository>();
builder.Services.AddScoped<IInterviewResultService, InterviewResultService>();

// Question Bank Services
builder.Services.AddScoped<IQuestionBankRepository, QuestionBankRepository>();
builder.Services.AddScoped<IQuestionBankService, QuestionBankService>();

// Answer Evaluation
builder.Services.AddScoped<IAnswerEvaluationRepository, AnswerEvaluationRepository>();
```

**Authentication & Authorization:**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { ... });

builder.Services.AddAuthorization();
```

**CORS:**
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                      "http://localhost:4200",
                      "https://localhost:4200",
                      "http://localhost:4000",
                      "https://localhost:4000"
                  )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
```

**Form Options:**
```csharp
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200 MB
});
```

**Middleware Pipeline:**
```csharp
app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```

---

## Configuration Settings

### appsettings.json Complete Reference

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=CommunicaAIDB;Username=postgres;Password=Vignesh@123"
  },
  "Jwt": {
    "Issuer": "CommunicaAI",
    "Audience": "CommunicaAIUsers",
    "Key": "THIS_IS_A_DEMO_SECRET_KEY_CHANGE_IT_TO_A_LONG_RANDOM_SECRET"
  },
  "CloudinarySettings": {
    "CloudName": "cloudname",
    "ApiKey": "apikey",
    "ApiSecret": "apisecret"
  },
  "PythonVerificationService": {
    "BaseUrl": "http://127.0.0.1:8000",
    "VerifyAudioPath": "/verify-audio"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Gemini": {
    "ApiKey": "YOUR_API_KEY",
    "Model": "gemini-2.5-flash"
  }
}
```

**Security Notes:**
- JWT Key should be changed to a strong secret in production
- Cloudinary credentials should be environment-specific
- Database password should use secrets management
- Gemini API key should be secured

---

### Frontend Environment Configuration

**Path:** `Frontend/src/environments/environment.ts`

```typescript
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5169'
};
```

**Production Environment:**
- Create `environment.prod.ts` with production API URL
- Set `production: true`
- Configure in angular.json build configurations

---

## What's Implemented vs What's Missing

### ✅ Fully Implemented Features

#### Backend:
1. **User Registration & Authentication**
   - ✅ Email/password registration with biometric enrollment
   - ✅ Password-based login
   - ✅ Audio-based login (with Python service)
   - ✅ Video-based login (stub implementation)
   - ✅ JWT token generation and validation
   - ✅ User profile endpoint

2. **Interview Session Management**
   - ✅ Create interview session
   - ✅ Get session details
   - ✅ Get user's interview history
   - ✅ Complete interview
   - ✅ Session ownership validation

3. **Question Management**
   - ✅ Question bank CRUD operations
   - ✅ Seed 100+ pre-defined questions
   - ✅ Random question generation by role/difficulty/category
   - ✅ Question distribution (60% Technical, 20% Behavioral, 20% HR)

4. **Answer Management**
   - ✅ Submit text answers
   - ✅ Submit audio answers with auto-transcription
   - ✅ Audio upload to Cloudinary
   - ✅ Gemini API transcription
   - ✅ Gemini API answer evaluation
   - ✅ AnswerEvaluation storage

5. **Result Generation**
   - ✅ Basic completion percentage
   - ✅ InterviewResult entity and storage

6. **External Integrations**
   - ✅ Cloudinary for media storage
   - ✅ Gemini AI for transcription
   - ✅ Gemini AI for evaluation
   - ✅ Python service for speaker verification

#### Frontend:
1. **Authentication UI**
   - ✅ Multi-step registration with video/audio capture
   - ✅ Login with 3 modes (password/audio/video)
   - ✅ MediaRecorder API integration
   - ✅ Auto-countdown timers
   - ✅ JWT token storage and management

2. **Dashboard**
   - ✅ User profile display
   - ✅ Interview statistics
   - ✅ Recent sessions list

3. **Interview Setup**
   - ✅ Form with role/topic/difficulty selection
   - ✅ Duration and question count configuration

4. **Live Interview** (Mock Implementation)
   - ✅ Question display with TTS
   - ✅ Audio recording
   - ✅ Mock transcription
   - ✅ Timer countdown
   - ✅ Navigation between questions

5. **Results Display**
   - ✅ Score display with colored badges
   - ✅ Strengths and improvements
   - ✅ Full transcript view

6. **History**
   - ✅ List past interviews
   - ✅ View previous results

---

### ⚠️ Partially Implemented / Mock Features

#### Backend:
1. **Video Biometric Verification**
   - ⚠️ BiometricVerificationService.VerifyVideoAsync always returns true
   - ⚠️ No facial recognition implementation
   - ❌ TODO: Integrate facial recognition library or service

2. **Result Scoring**
   - ⚠️ InterviewResult stores scores but doesn't calculate from AnswerEvaluations
   - ⚠️ Only completion percentage is calculated
   - ❌ TODO: Aggregate AnswerEvaluation scores into InterviewResult

#### Frontend:
1. **Interview Service**
   - ⚠️ InterviewService uses MOCK implementation with localStorage
   - ⚠️ Doesn't call backend /api/interviews endpoints
   - ❌ TODO: Replace with real backend API integration

2. **Interview History Service**
   - ⚠️ Uses localStorage, not backend API
   - ❌ TODO: Call GET /api/interviews/my-history

3. **Speech Transcription Service**
   - ⚠️ Returns random mock text
   - ⚠️ Doesn't call backend /api/test/transcribe
   - ❌ TODO: Implement real API call

4. **Live Interview Audio Submission**
   - ⚠️ Records audio but doesn't submit to backend
   - ⚠️ Uses mock transcription locally
   - ❌ TODO: Call POST /api/interviews/{sessionId}/answers/audio

---

### ❌ Not Implemented Features

1. **UserMediaProfile Entity**
   - ❌ Entity exists in schema but not used
   - ❌ No API endpoints for media profile management
   - ❌ Not referenced in AuthController or services

2. **Advanced Result Analytics**
   - ❌ No detailed scoring algorithm
   - ❌ No trend analysis across interviews
   - ❌ No performance recommendations

3. **Question Feedback System**
   - ❌ Users can't report bad questions
   - ❌ No quality metrics for questions

4. **Interview Pause/Resume**
   - ❌ Can't pause interview and continue later
   - ❌ Session state not persisted between page refreshes

5. **Multi-Language Support**
   - ❌ Only English interface
   - ❌ No i18n/l10n infrastructure

6. **Admin Panel**
   - ❌ No admin user roles
   - ❌ No question bank management UI
   - ❌ No user management

7. **Email Notifications**
   - ❌ No email confirmation
   - ❌ No password reset
   - ❌ No interview completion emails

8. **Analytics & Reporting**
   - ❌ No usage analytics
   - ❌ No performance dashboards
   - ❌ No export functionality

9. **Social Features**
   - ❌ No sharing results
   - ❌ No leaderboards
   - ❌ No community questions

10. **Payment Integration**
    - ❌ No subscription plans
    - ❌ No payment processing

---

## Technical Debt Analysis

### Critical Issues

#### 1. Frontend-Backend Integration Gap
**Severity:** HIGH  
**Location:** Frontend services (InterviewService, InterviewHistoryService, SpeechTranscriptionService)

**Problem:**
- Frontend uses mock localStorage implementations
- Backend APIs exist but aren't called
- User data not synchronized

**Impact:**
- Features appear to work but data is lost on browser clear
- No real interview evaluation
- No persistent history

**Recommendation:**
- **Priority 1:** Integrate InterviewService with backend API
- Replace localStorage with HTTP calls
- Implement proper error handling
- Add loading states

**Estimated Effort:** 2-3 days

---

#### 2. Video Biometric Verification Stub
**Severity:** HIGH  
**Location:** BiometricVerificationService.VerifyVideoAsync

**Problem:**
- Always returns true (security vulnerability)
- No actual facial recognition

**Impact:**
- Anyone can login with any video if they know an email
- Biometric security not functional

**Recommendation:**
- Integrate facial recognition library (e.g., Face++ API, Azure Face API)
- Or expand Python service to handle video verification
- Implement proper verification threshold

**Estimated Effort:** 5-7 days

---
#### 3. Hardcoded Configuration Values
**Severity:** MEDIUM  
**Location:** appsettings.json, environment.ts

**Problem:**
- Database password in source control
- Demo JWT secret key
- Cloudinary credentials as placeholders

**Impact:**
- Security risk in production
- Credentials could be exposed

**Recommendation:**
- Use Azure Key Vault / AWS Secrets Manager
- Environment variables for sensitive data
- .gitignore appsettings.Production.json
- User Secrets in development

**Estimated Effort:** 1 day

---

#### 4. No Result Score Aggregation
**Severity:** MEDIUM  
**Location:** InterviewResultService

**Problem:**
- AnswerEvaluations created but not used for InterviewResult
- InterviewResult stores scores but they're not calculated

**Impact:**
- Users don't get accurate overall assessment
- Evaluation data not utilized

**Recommendation:**
- Implement aggregation logic in InterviewResultService.GenerateResultAsync
- Calculate:
  - TechnicalScore = avg(AnswerEvaluation.TechnicalScore)
  - ClarityScore = avg(AnswerEvaluation.ClarityScore)
  - OverallScore = weighted average
- Generate Strengths/Weaknesses from evaluations

**Estimated Effort:** 1-2 days

---

### Medium Issues

#### 5. No Input Validation on File Uploads
**Severity:** MEDIUM  
**Location:** AuthController, InterviewAnswerController

**Problem:**
- File size not validated
- File type not strictly validated
- Could accept malicious files

**Impact:**
- Storage abuse
- Security risk

**Recommendation:**
- Add file size limits (e.g., 50MB for video, 10MB for audio)
- Validate mime types
- Scan for malware (optional)
- Return 413 Payload Too Large for oversized files

**Estimated Effort:** 1 day

---

#### 6. No Rate Limiting
**Severity:** MEDIUM  
**Location:** All API endpoints

**Problem:**
- No protection against brute force attacks
- No throttling on expensive operations (transcription, evaluation)

**Impact:**
- API abuse
- DDoS vulnerability
- High Gemini API costs

**Recommendation:**
- Implement rate limiting middleware
- Use AspNetCoreRateLimit NuGet package
- Configure per-endpoint limits:
  - Login: 5 requests/minute
  - Transcription: 10 requests/minute
  - Question generation: 3 requests/minute

**Estimated Effort:** 2 days

---

#### 7. Error Handling Inconsistency
**Severity:** MEDIUM  
**Location:** Various controllers and services

**Problem:**
- Some errors return plain text, some return JSON
- No global exception handler
- Stack traces could leak in production

**Impact:**
- Poor client error handling
- Inconsistent UX
- Security information disclosure

**Recommendation:**
- Implement global exception middleware
- Standardize error response format:
  ```json
  {
    "error": "ErrorCode",
    "message": "User-friendly message",
    "details": {} // optional
  }
  ```
- Log exceptions to monitoring service
- Hide stack traces in production

**Estimated Effort:** 2-3 days

---

#### 8. No Database Indexes on Foreign Keys
**Severity:** MEDIUM  
**Location:** ApplicationDbContext

**Problem:**
- Some foreign keys lack indexes
- Could cause slow queries as data grows

**Impact:**
- Performance degradation
- Slow dashboard loads

**Recommendation:**
- Add index to InterviewAnswer.InterviewSessionId
- Add index to InterviewResult.InterviewSessionId
- Review query patterns and add composite indexes

**Estimated Effort:** 0.5 day

---

### Low Issues

#### 9. No Unit Tests
**Severity:** LOW  
**Location:** Entire solution

**Problem:**
- No test coverage
- Difficult to refactor with confidence

**Impact:**
- Regression risk
- Slower development

**Recommendation:**
- Add xUnit test project
- Test services with mocked dependencies
- Test repositories with in-memory database
- Target 70%+ coverage

**Estimated Effort:** 5-7 days

---

#### 10. No Logging Infrastructure
**Severity:** LOW  
**Location:** All services

**Problem:**
- No structured logging
- Difficult to debug production issues

**Impact:**
- Poor observability
- Hard to troubleshoot

**Recommendation:**
- Add Serilog or NLog
- Log to file/console/cloud
- Add correlation IDs
- Log key events (login, interview start/complete, errors)

**Estimated Effort:** 1-2 days

---

#### 11. UserMediaProfile Entity Unused
**Severity:** LOW  
**Location:** Models, Data

**Problem:**
- Entity defined but never used
- No API endpoints
- Dead code

**Impact:**
- Confusion
- Maintenance overhead

**Recommendation:**
- Either implement media profile management OR remove entity
- If keeping, add endpoints:
  - GET /api/media/profile
  - POST /api/media/upload-audio
  - POST /api/media/upload-video

**Estimated Effort:** 0.5 day (removal) or 2 days (implementation)

---

#### 12. No API Documentation
**Severity:** LOW  
**Location:** Controllers

**Problem:**
- No Swagger annotations
- Limited XML comments

**Impact:**
- Harder for frontend developers
- Poor API discoverability

**Recommendation:**
- Add Swashbuckle XML comments
- Document all endpoints with:
  - Summary
  - Parameters
  - Response codes
  - Example requests/responses

**Estimated Effort:** 1 day

---

#### 13. Frontend Code Duplication
**Severity:** LOW  
**Location:** Login and Register components

**Problem:**
- Similar MediaRecorder logic duplicated
- Video/audio capture code repeated

**Impact:**
- Maintenance burden
- Bug fix requires multiple changes

**Recommendation:**
- Extract shared logic to service or utility
- Create reusable MediaCaptureComponent
- Share recording state management

**Estimated Effort:** 2 days

---

### Recommended Priorities

**Phase 1 (Critical - 2 weeks):**
1. Frontend-Backend Integration (InterviewService)
2. Video Biometric Verification
3. Hardcoded Configuration → Secrets Management
4. Result Score Aggregation

**Phase 2 (Important - 1 week):**
5. File Upload Validation
6. Rate Limiting
7. Error Handling Standardization
8. Database Index Optimization

**Phase 3 (Enhancement - 2 weeks):**
9. Unit Test Coverage
10. Logging Infrastructure
11. API Documentation
12. Code Refactoring

---

## Performance Considerations

### Backend Optimizations

1. **Database Query Optimization:**
   - Use `.AsNoTracking()` for read-only queries
   - Implement pagination for list endpoints
   - Add select projections to reduce data transfer

2. **Caching:**
   - Cache QuestionBank queries (rarely change)
   - Cache user profiles
   - Use Redis for distributed caching

3. **Async Operations:**
   - All I/O operations are async ✅
   - Consider background jobs for long-running tasks (evaluation)

4. **API Response Time Targets:**
   - Authentication: <500ms
   - Question retrieval: <200ms
   - Answer submission: <2s (including transcription)
   - Result generation: <1s

---

### Frontend Optimizations

1. **Lazy Loading:**
   - All feature modules use lazy loading ✅

2. **Change Detection:**
   - Uses OnPush strategy where possible
   - Signals minimize unnecessary re-renders ✅

3. **Bundle Size:**
   - Tree-shaking enabled
   - Production build optimization
   - Consider code splitting for large dependencies

4. **Media Optimization:**
   - Compress audio/video before upload
   - Use WebM with appropriate bitrates
   - Show upload progress

---

## Deployment Guide

### Backend Deployment

**Prerequisites:**
- .NET 9.0 Runtime
- PostgreSQL 14+
- Cloudinary account
- Google Gemini API key
- Python verification service (separate deployment)

**Environment Variables:**
```
ConnectionStrings__DefaultConnection=<postgres-connection-string>
Jwt__Key=<strong-secret-key>
CloudinarySettings__CloudName=<cloudinary-cloud-name>
CloudinarySettings__ApiKey=<cloudinary-api-key>
CloudinarySettings__ApiSecret=<cloudinary-api-secret>
Gemini__ApiKey=<gemini-api-key>
PythonVerificationService__BaseUrl=<python-service-url>
```

**Build Commands:**
```bash
dotnet restore
dotnet build --configuration Release
dotnet publish --configuration Release --output ./publish
```

**Database Migration:**
```bash
dotnet ef database update --project CommunicaAI
```

**Run:**
```bash
cd publish
dotnet CommunicaAI.dll
```

---

### Frontend Deployment

**Prerequisites:**
- Node.js 18+
- Angular CLI 18+

**Environment Configuration:**
Create `src/environments/environment.prod.ts`:
```typescript
export const environment = {
  production: true,
  apiBaseUrl: 'https://api.communicaai.com'
};
```

**Build Commands:**
```bash
npm install
npm run build -- --configuration production
```

**Output:**
- Builds to `dist/frontend/browser`
- Static files ready for hosting

**Hosting Options:**
- Azure Static Web Apps
- AWS S3 + CloudFront
- Netlify
- Vercel

**nginx Configuration Example:**
```nginx
server {
    listen 80;
    server_name communicaai.com;
    root /var/www/communicaai/browser;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api {
        proxy_pass http://localhost:5169;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

---

## Security Checklist

### Backend Security

- ✅ JWT authentication implemented
- ✅ Password hashing with bcrypt (via PasswordHasher)
- ✅ HTTPS enforced (UseHttpsRedirection)
- ✅ CORS configured with specific origins
- ✅ SQL injection protected (EF Core parameterized queries)
- ❌ Rate limiting (TODO)
- ❌ Input validation on file uploads (TODO)
- ❌ API key rotation strategy (TODO)
- ❌ Security headers (CSP, HSTS, X-Frame-Options) (TODO)

### Frontend Security

- ✅ JWT stored in localStorage (acceptable for this use case)
- ✅ Route guards for protected pages
- ✅ HTTP interceptor for token injection
- ❌ XSS protection in dynamic content (TODO: sanitize if rendering user HTML)
- ❌ CSRF protection (not needed for stateless JWT API)

### Production Recommendations

1. **Enable HTTPS Only**
   - Enforce HSTS
   - Redirect HTTP to HTTPS

2. **Secure JWT**
   - Use strong secret (256+ bits)
   - Rotate secrets periodically
   - Consider refresh tokens for long sessions

3. **API Security Headers**
   ```csharp
   app.Use(async (context, next) =>
   {
       context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
       context.Response.Headers.Add("X-Frame-Options", "DENY");
       context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
       context.Response.Headers.Add("Referrer-Policy", "no-referrer");
       await next();
   });
   ```

4. **Monitor & Alert**
   - Failed login attempts
   - Unusual API usage patterns
   - Error rate spikes

---

## Monitoring & Observability

### Recommended Tools

**Application Monitoring:**
- Application Insights (Azure)
- New Relic
- Datadog

**Logging:**
- Serilog → Azure Log Analytics
- ELK Stack (Elasticsearch, Logstash, Kibana)

**Metrics to Track:**
- API response times
- Authentication success/failure rates
- Interview completion rates
- Error rates by endpoint
- Gemini API usage and costs
- Database query performance

**Alerts:**
- API error rate > 5%
- Response time > 2s
- Database connection failures
- Python service unavailability
- Cloudinary upload failures

---

## Appendix A: API Quick Reference

### Authentication Endpoints

| Method | Endpoint | Auth | Purpose |
|--------|----------|------|---------|
| POST | /api/auth/register | No | Register user |
| POST | /api/auth/login/password | No | Password login |
| POST | /api/auth/login/audio | No | Audio login |
| POST | /api/auth/login/video | No | Video login |
| GET | /api/auth/me | Yes | Get profile |

### Interview Endpoints

| Method | Endpoint | Auth | Purpose |
|--------|----------|------|---------|
| POST | /api/interviews | Yes | Create session |
| GET | /api/interviews/{id} | Yes | Get session |
| GET | /api/interviews/my-history | Yes | Get history |
| GET | /api/interviews/{id}/questions | Yes | Get questions |
| POST | /api/interviews/{id}/answers | Yes | Submit answer |
| POST | /api/interviews/{id}/answers/audio | Yes | Submit audio |
| POST | /api/interviews/{id}/complete | Yes | Complete |

### Question Bank Endpoints

| Method | Endpoint | Auth | Purpose |
|--------|----------|------|---------|
| POST | /api/question-bank | Yes | Create question |
| GET | /api/question-bank/{id} | Yes | Get question |
| GET | /api/question-bank | Yes | List all |
| DELETE | /api/question-bank/{id} | Yes | Delete question |
| POST | /api/question-bank/seed | Yes | Seed questions |

---

## Appendix B: Database Migration History

1. **20260516153607_InitialCreate**
   - Created Users, UserVerificationProfiles tables

2. **20260518081821_MediaTableAdded**
   - Created UserMediaProfile table (unused)

3. **20260524150108_AddedMigrationForNewAuth**
   - Updated authentication schema

4. **20260604173813_AddInterviewSession**
   - Created InterviewSession table

5. **20260604175540_UpdateInterviewSessionUserIdToGuid**
   - Changed UserId from int to Guid

6. **20260604192342_AddInterviewManagementTables**
   - Created InterviewQuestion, InterviewAnswer, InterviewResult tables

7. **20260605171615_AddAnswerEvaluation**
   - Created AnswerEvaluation table

8. **20260605180410_ChangedInterviewAnswer**
   - Updated InterviewAnswer schema

---

## Appendix C: Frontend Component Hierarchy

```
App (Root)
├── LoginComponent
│   ├── Password Mode
│   ├── Audio Mode (MediaRecorder)
│   └── Video Mode (MediaRecorder)
├── RegisterComponent
│   ├── Form Step
│   ├── Video Capture Step (MediaRecorder)
│   ├── Audio Capture Step (MediaRecorder)
│   └── Review Step
├── DashboardComponent
│   ├── User Profile Section
│   ├── Stats Cards
│   └── Recent Sessions List
├── SetupComponent
│   └── Interview Configuration Form
├── LiveComponent
│   ├── Timer
│   ├── Question Display (TTS)
│   ├── Audio Recorder (MediaRecorder)
│   ├── Transcript Display
│   └── Navigation Controls
├── ResultComponent
│   ├── Score Cards
│   ├── Strengths/Improvements
│   └── Transcript View
└── HistoryComponent
    └── Session List
```

---

## Document Version History

- **v1.0** - 2025-01-15: Initial comprehensive documentation
  - Documented all 6 controllers
  - Documented all 14 services
  - Documented all 6 repositories
  - Documented all 12 models
  - Documented all 18 DTOs
  - Documented complete frontend architecture
  - Analyzed technical debt
  - Identified implemented vs missing features

---

## Conclusion

This document serves as the complete technical reference for Communica AI. It provides:

1. **Complete API Reference** - All endpoints with request/response formats
2. **Service Documentation** - All business logic and dependencies
3. **Database Schema** - All entities, relationships, and indexes
4. **Frontend Architecture** - All components, services, and routing
5. **Integration Details** - Cloudinary, Gemini, Python service
6. **Configuration Guide** - All settings and environment variables
7. **Technical Debt Analysis** - Issues prioritized by severity
8. **Deployment Guide** - Steps for production deployment

**Key Takeaways:**
- Backend is largely complete with production-ready code
- Frontend uses mock implementations that need backend integration
- Video biometric verification is a stub requiring implementation
- Security hardening needed before production (secrets management, rate limiting)
- Result scoring needs aggregation logic
- Comprehensive testing infrastructure needed

**Next Steps:**
1. Integrate frontend with backend APIs (Priority 1)
2. Implement video facial recognition
3. Secure configuration management
4. Add comprehensive error handling and logging
5. Implement unit and integration tests

---

**Document Maintained By:** Development Team  
**Last Review:** 2025-01-15  
**Next Review Due:** 2025-02-15
