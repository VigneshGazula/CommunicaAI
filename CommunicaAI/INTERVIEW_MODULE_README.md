# Interview Session Management Module

## Overview
Complete implementation of the Interview Session Management module for Communica AI backend.

---

## Architecture

Follows the established pattern:
```
Controllers → Services → Repositories → EF Core → PostgreSQL
```

---

## Files Created/Updated

### 1. Entity (Updated)
**Path:** `Models/InterviewSession.cs`

**Properties:**
- `Guid Id` - Primary key
- `Guid UserId` - Foreign key to AppUser
- `string Role` - Job role/position
- `string Topic` - Interview topic
- `string Difficulty` - Difficulty level (easy/medium/hard)
- `int QuestionCount` - Number of questions (1-50)
- `int DurationMinutes` - Session duration (1-180)
- `DateTime StartedAt` - UTC start timestamp
- `DateTime? CompletedAt` - UTC completion timestamp (nullable)
- `string Status` - Session status (default: "InProgress")

**Change:** Updated `UserId` from `int` to `Guid` to match AppUser.Id type.

---

### 2. DTOs

#### **CreateInterviewRequest**
**Path:** `DTO/Interview/CreateInterviewRequest.cs`

**Properties:**
- `string Role` - Required
- `string Topic` - Required
- `string Difficulty` - Required
- `int QuestionCount` - Required, Range(1, 50)
- `int DurationMinutes` - Required, Range(1, 180)

**Validation:** DataAnnotations with error messages

---

#### **CreateInterviewResponse**
**Path:** `DTO/Interview/CreateInterviewResponse.cs`

**Properties:**
- `Guid SessionId`
- `string Status`
- `DateTime StartedAt`

---

#### **InterviewSessionResponse**
**Path:** `DTO/Interview/InterviewSessionResponse.cs`

**Properties:**
- `Guid SessionId`
- `string Role`
- `string Topic`
- `string Difficulty`
- `int QuestionCount`
- `int DurationMinutes`
- `string Status`
- `DateTime StartedAt`
- `DateTime? CompletedAt`

**Change:** Added `CompletedAt` property.

---

### 3. Repository Layer

#### **IInterviewRepository**
**Path:** `Repositories/Interfaces/IInterviewRepository.cs`

**Methods:**
```csharp
Task<InterviewSession> CreateAsync(InterviewSession session)
Task<InterviewSession?> GetByIdAsync(Guid sessionId)
Task<List<InterviewSession>> GetByUserIdAsync(Guid userId)
Task UpdateAsync(InterviewSession session)
```

---

#### **InterviewRepository**
**Path:** `Repositories/InterviewRepository.cs`

**Implementation:**
- `CreateAsync` - Adds new session and saves changes
- `GetByIdAsync` - Retrieves session by ID
- `GetByUserIdAsync` - Retrieves all user sessions ordered by StartedAt DESC
- `UpdateAsync` - Updates existing session and saves changes

**Dependencies:** `ApplicationDbContext`

---

### 4. Service Layer

#### **IInterviewService**
**Path:** `Services/Interfaces/IInterviewService.cs`

**Methods:**
```csharp
Task<CreateInterviewResponse> CreateInterviewAsync(Guid userId, CreateInterviewRequest request)
Task<InterviewSessionResponse?> GetInterviewAsync(Guid sessionId, Guid userId)
Task<List<InterviewSessionResponse>> GetUserInterviewsAsync(Guid userId)
Task<bool> CompleteInterviewAsync(Guid sessionId, Guid userId)
```

---

#### **InterviewService**
**Path:** `Services/InterviewService.cs`

**Business Logic:**

##### CreateInterviewAsync
1. Creates new InterviewSession entity
2. Generates new Guid
3. Sets StartedAt to UtcNow
4. Sets Status to "InProgress"
5. Saves via repository
6. Returns CreateInterviewResponse

##### GetInterviewAsync
1. Loads session by ID
2. Verifies ownership (userId match)
3. Returns DTO or null if not found/unauthorized

##### GetUserInterviewsAsync
1. Loads all sessions for user
2. Maps to DTO list
3. Returns ordered list (handled by repository)

##### CompleteInterviewAsync
1. Loads session by ID
2. Verifies ownership
3. Sets CompletedAt to UtcNow
4. Sets Status to "Completed"
5. Saves changes
6. Returns true on success, false if not found

**Dependencies:** `IInterviewRepository`

---

### 5. Controller

#### **InterviewController**
**Path:** `Controllers/InterviewController.cs`

**Attributes:**
- `[ApiController]`
- `[Route("api/interviews")]`
- `[Authorize]`

**Endpoints:**

##### POST /api/interviews
**Purpose:** Create new interview session

**Request Body:** `CreateInterviewRequest`

**Response:** `201 Created` with `CreateInterviewResponse`

**Authorization:** JWT (UserId from ClaimTypes.NameIdentifier)

**Validation:** ModelState validation

---

##### GET /api/interviews/{sessionId}
**Purpose:** Get interview session details

**Path Parameter:** `sessionId` (Guid)

**Response:** 
- `200 OK` with `InterviewSessionResponse`
- `404 Not Found` if session doesn't exist or unauthorized

**Authorization:** Verifies ownership

---

##### GET /api/interviews/my-history
**Purpose:** Get current user's interview history

**Response:** `200 OK` with `List<InterviewSessionResponse>`

**Authorization:** JWT

**Order:** Descending by StartedAt (most recent first)

---

##### POST /api/interviews/{sessionId}/complete
**Purpose:** Mark interview as completed

**Path Parameter:** `sessionId` (Guid)

**Response:** 
- `200 OK` with success message
- `404 Not Found` if session doesn't exist or unauthorized

**Authorization:** Verifies ownership

---

### 6. Database Configuration

#### **ApplicationDbContext** (Updated)
**Path:** `Data/ApplicationDbContext.cs`

**Added:**
- `DbSet<InterviewSession> InterviewSessions`

**Configuration:**
```csharp
modelBuilder.Entity<InterviewSession>(entity =>
{
    entity.HasKey(x => x.Id);
    entity.HasIndex(x => x.UserId);
    entity.Property(x => x.Role).IsRequired().HasMaxLength(100);
    entity.Property(x => x.Topic).IsRequired().HasMaxLength(200);
    entity.Property(x => x.Difficulty).IsRequired().HasMaxLength(50);
    entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
    entity.Property(x => x.StartedAt).IsRequired();
});
```

---

### 7. Dependency Injection

#### **Program.cs** (Updated)

**Added Registrations:**
```csharp
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
```

**Added Using Statements:**
```csharp
using CommunicaAI.Repositories;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;
```

---

## Database Migration

**Migration Name:** `UpdateInterviewSessionUserIdToGuid`

**Changes:** 
- Updated `InterviewSession.UserId` column type from `int` to `uuid` (Guid)

**Apply Migration:**
```bash
dotnet ef database update
```

---

## Authentication & Authorization

### JWT Token Handling
All endpoints require JWT authentication via `[Authorize]` attribute.

### User ID Extraction
```csharp
var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
if (!Guid.TryParse(userIdClaim, out var userId))
{
    return Unauthorized(new { message = "Invalid token." });
}
```

### Ownership Verification
Services verify that the interview session belongs to the authenticated user before allowing access or modifications.

---

## Validation Rules

### CreateInterviewRequest

| Field | Rule | Error Message |
|-------|------|---------------|
| Role | Required | "Role is required" |
| Topic | Required | "Topic is required" |
| Difficulty | Required | "Difficulty is required" |
| QuestionCount | Range(1, 50) | "Question count must be between 1 and 50" |
| DurationMinutes | Range(1, 180) | "Duration must be between 1 and 180 minutes" |

---

## API Usage Examples

### 1. Create Interview Session
```http
POST /api/interviews
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json

{
  "role": "Software Engineer",
  "topic": "Technical Interview",
  "difficulty": "medium",
  "questionCount": 5,
  "durationMinutes": 15
}
```

**Response (201 Created):**
```json
{
  "sessionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "InProgress",
  "startedAt": "2026-06-04T14:30:00Z"
}
```

---

### 2. Get Interview Session
```http
GET /api/interviews/3fa85f64-5717-4562-b3fc-2c963f66afa6
Authorization: Bearer <JWT_TOKEN>
```

**Response (200 OK):**
```json
{
  "sessionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "role": "Software Engineer",
  "topic": "Technical Interview",
  "difficulty": "medium",
  "questionCount": 5,
  "durationMinutes": 15,
  "status": "InProgress",
  "startedAt": "2026-06-04T14:30:00Z",
  "completedAt": null
}
```

---

### 3. Get My Interview History
```http
GET /api/interviews/my-history
Authorization: Bearer <JWT_TOKEN>
```

**Response (200 OK):**
```json
[
  {
    "sessionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "role": "Software Engineer",
    "topic": "Technical Interview",
    "difficulty": "medium",
    "questionCount": 5,
    "durationMinutes": 15,
    "status": "Completed",
    "startedAt": "2026-06-04T14:30:00Z",
    "completedAt": "2026-06-04T14:45:00Z"
  }
]
```

---

### 4. Complete Interview
```http
POST /api/interviews/3fa85f64-5717-4562-b3fc-2c963f66afa6/complete
Authorization: Bearer <JWT_TOKEN>
```

**Response (200 OK):**
```json
{
  "message": "Interview completed successfully."
}
```

---

## Error Responses

### 400 Bad Request (Validation)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "QuestionCount": ["Question count must be between 1 and 50"]
  }
}
```

### 401 Unauthorized
```json
{
  "message": "Invalid token."
}
```

### 404 Not Found
```json
{
  "message": "Interview session not found."
}
```

---

## Security Features

1. **JWT Authentication** - All endpoints protected
2. **Ownership Verification** - Users can only access their own sessions
3. **Input Validation** - DataAnnotations + ModelState checks
4. **SQL Injection Protection** - EF Core parameterized queries
5. **GUID IDs** - Non-sequential, hard to guess

---

## Testing Checklist

### Unit Tests (Recommended)
- [ ] Service layer business logic
- [ ] Repository CRUD operations
- [ ] DTO mapping logic

### Integration Tests (Recommended)
- [ ] Controller endpoints with auth
- [ ] Database operations
- [ ] Ownership verification

### Manual Testing
- [ ] Create interview with valid data
- [ ] Create interview with invalid data (validation)
- [ ] Get interview by ID (own session)
- [ ] Get interview by ID (other user's session - should fail)
- [ ] Get interview history
- [ ] Complete interview
- [ ] Complete already completed interview
- [ ] Complete non-existent session

---

## Future Enhancements (Not Implemented)

The following are deliberately NOT implemented per requirements:
- Questions management
- Answers storage
- AI scoring/evaluation
- Transcripts
- Communication analytics
- OpenAI/Whisper integration
- Video/Audio analysis

These can be added as separate modules when needed.

---

## Summary

✅ **Complete Interview Session Management module implemented**
- Entity with proper Guid types
- DTOs with validation
- Repository layer for data access
- Service layer for business logic
- Controller with 4 RESTful endpoints
- JWT authentication and ownership verification
- Database configuration and migration
- Dependency injection registered
- Production-quality code (no placeholders or TODOs)

**Ready for production use!**
