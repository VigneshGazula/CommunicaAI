# Communica AI - Quick Start Guide

## 🚀 Getting Started

### Prerequisites
- PostgreSQL database running
- .NET 10.0 SDK installed
- Connection string configured in `appsettings.json`

---

## 📦 First-Time Setup

### 1. Apply Migrations (Already Done)
```bash
cd CommunicaAI
dotnet ef database update
```

### 2. Run the Application
```bash
dotnet run
```

### 3. Seed Question Bank (IMPORTANT - Do This First!)
```http
POST https://localhost:5001/api/question-bank/seed
Authorization: Bearer YOUR_JWT_TOKEN
```

This will seed 100+ interview questions across 9 roles.

---

## 🔑 Authentication

All interview endpoints require JWT authentication.

**Login/Register first:**
```http
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "password"
}
```

**Copy the JWT token** from the response and use it in all subsequent requests:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 📝 Typical Interview Flow

### Step 1: Seed Questions (Once)
```http
POST /api/question-bank/seed
Authorization: Bearer {token}
```

### Step 2: Create Interview Session
```http
POST /api/interviews
Authorization: Bearer {token}
Content-Type: application/json

{
  "role": "Software Engineer",
  "topic": "Technical Interview Practice",
  "difficulty": "Medium",
  "questionCount": 5,
  "durationMinutes": 15
}
```

**Response:**
```json
{
  "sessionId": "123e4567-e89b-12d3-a456-426614174000",
  "status": "InProgress",
  "startedAt": "2026-06-05T10:00:00Z"
}
```

**⚠️ Save the `sessionId` - you'll need it for all other requests!**

### Step 3: Get Questions
```http
GET /api/interviews/{sessionId}/questions
Authorization: Bearer {token}
```

**Response:**
```json
[
  {
    "id": "question-guid-1",
    "orderNumber": 1,
    "category": "Technical",
    "questionText": "Explain object-oriented programming principles.",
    "isAnswered": false
  },
  {
    "id": "question-guid-2",
    "orderNumber": 2,
    "category": "Technical",
    "questionText": "What is dependency injection?",
    "isAnswered": false
  }
  // ... 3 more questions
]
```

### Step 4: Submit Answers (One at a Time)
```http
POST /api/interviews/{sessionId}/answers
Authorization: Bearer {token}
Content-Type: application/json

{
  "questionId": "question-guid-1",
  "transcript": "Object-oriented programming is a paradigm based on objects containing data and methods. The main principles are encapsulation, inheritance, polymorphism, and abstraction..."
}
```

**Response:**
```json
{
  "id": "answer-guid",
  "questionId": "question-guid-1",
  "transcript": "Object-oriented programming is...",
  "answeredAt": "2026-06-05T10:05:00Z"
}
```

**Repeat for each question** (or as many as you want to answer).

### Step 5: Complete Interview
```http
POST /api/interviews/{sessionId}/complete
Authorization: Bearer {token}
```

**Response:**
```json
{
  "message": "Interview completed successfully."
}
```

**This triggers:**
- Sets `CompletedAt` timestamp
- Changes status to "Completed"
- Generates `InterviewResult` with completion percentage

### Step 6: View Results
```http
GET /api/interviews/{sessionId}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "sessionId": "123e4567-e89b-12d3-a456-426614174000",
  "role": "Software Engineer",
  "topic": "Technical Interview Practice",
  "difficulty": "Medium",
  "questionCount": 5,
  "durationMinutes": 15,
  "status": "Completed",
  "startedAt": "2026-06-05T10:00:00Z",
  "completedAt": "2026-06-05T10:15:00Z",
  "questions": [
    {
      "id": "question-guid-1",
      "orderNumber": 1,
      "category": "Technical",
      "questionText": "Explain object-oriented programming principles.",
      "isAnswered": true,
      "answer": {
        "id": "answer-guid",
        "transcript": "Object-oriented programming is...",
        "answeredAt": "2026-06-05T10:05:00Z"
      }
    }
    // ... other questions with/without answers
  ],
  "result": {
    "totalQuestions": 5,
    "answeredQuestions": 5,
    "completionPercentage": 100.0,
    "generatedAt": "2026-06-05T10:15:00Z"
  }
}
```

### Step 7: View History
```http
GET /api/interviews/my-history
Authorization: Bearer {token}
```

**Response:**
```json
[
  {
    "sessionId": "123e4567-e89b-12d3-a456-426614174000",
    "role": "Software Engineer",
    "difficulty": "Medium",
    "startedAt": "2026-06-05T10:00:00Z",
    "completedAt": "2026-06-05T10:15:00Z",
    "status": "Completed",
    "completionPercentage": 100.0
  },
  {
    "sessionId": "another-session-guid",
    "role": "Frontend Developer",
    "difficulty": "Easy",
    "startedAt": "2026-06-04T14:00:00Z",
    "completedAt": "2026-06-04T14:10:00Z",
    "status": "Completed",
    "completionPercentage": 80.0
  }
  // ... ordered by most recent first
]
```

---

## 🎯 Supported Roles

- Software Engineer
- Backend Developer
- Frontend Developer
- Full Stack Developer
- Data Analyst
- Data Scientist
- Machine Learning Engineer
- DevOps Engineer
- Cloud Engineer

---

## 📊 Question Categories

Questions are auto-generated with this distribution:
- **60% Technical** - Role-specific technical questions
- **20% Behavioral** - Soft skills and past experience
- **20% HR** - General professional questions

---

## 🎚️ Difficulty Levels

- **Easy** - Entry-level questions
- **Medium** - Mid-level questions
- **Hard** - Senior-level questions

---

## ⚙️ Configuration Limits

**Question Count**: 1-50 questions per session  
**Duration**: 1-180 minutes  
**Transcript Length**: 1-5000 characters per answer

---

## ❌ Common Errors

### 400 Bad Request: "Question already answered"
- You cannot submit multiple answers for the same question
- Each question can only be answered once

### 401 Unauthorized
- JWT token is missing or expired
- Login again and get a new token

### 404 Not Found: "Session not found"
- You're trying to access another user's session
- Sessions are private to each user
- Verify the `sessionId` is correct

### 404 Not Found: "Question not found"
- The `questionId` doesn't belong to this session
- Get questions from `/api/interviews/{sessionId}/questions`

---

## 🧪 Testing with Postman/Swagger

### 1. Create a Collection
1. Import these endpoints into Postman
2. Set up an environment variable `{{jwt_token}}`
3. Set up an environment variable `{{sessionId}}`

### 2. Workflow
1. Login → Save JWT token
2. Seed questions (once)
3. Create interview → Save sessionId
4. Get questions → Save question IDs
5. Submit answers (multiple times)
6. Complete interview
7. View results
8. View history

### 3. Authorization Header Template
```
Authorization: Bearer {{jwt_token}}
```

---

## 🔍 Debugging Tips

### Check Migrations Applied
```bash
dotnet ef migrations list
```

All 6 migrations should show without asterisks (applied).

### Check Database Tables
```sql
SELECT * FROM "QuestionBanks" LIMIT 10;
SELECT * FROM "InterviewSessions" WHERE "UserId" = 'your-user-id';
SELECT * FROM "InterviewQuestions" WHERE "InterviewSessionId" = 'session-id';
SELECT * FROM "InterviewAnswers" WHERE "InterviewSessionId" = 'session-id';
SELECT * FROM "InterviewResults" WHERE "InterviewSessionId" = 'session-id';
```

### Check Logs
Look for console output when running `dotnet run`:
- JWT validation errors
- Database connection issues
- Entity Framework queries

### Verify Seeded Questions
```sql
SELECT "Role", "Category", COUNT(*) 
FROM "QuestionBanks" 
GROUP BY "Role", "Category";
```

Should show questions across all 9 roles and 3 categories.

---

## 🎯 Quick Reference: All Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/question-bank/seed` | Seed 100 questions (once) |
| POST | `/api/interviews` | Create interview session |
| GET | `/api/interviews/{id}` | Get session details |
| GET | `/api/interviews/{id}/questions` | Get session questions |
| POST | `/api/interviews/{id}/answers` | Submit answer |
| POST | `/api/interviews/{id}/complete` | Complete interview |
| GET | `/api/interviews/my-history` | Get user history |
| POST | `/api/question-bank` | Create custom question (admin) |
| GET | `/api/question-bank` | List all questions (admin) |
| DELETE | `/api/question-bank/{id}` | Delete question (admin) |

**All require JWT Bearer token!**

---

## 💡 Tips

1. **Always seed questions first** before creating interviews
2. **Save the sessionId** from the create response
3. **Answer questions in any order** - orderNumber is just for display
4. **You don't have to answer all questions** - partial completion is tracked
5. **Complete the interview** to generate results
6. **Results are cached** - calling complete multiple times won't duplicate results
7. **Sessions are private** - you can only access your own interviews
8. **Questions are randomized** - each session has different questions

---

## 🚀 Next: Frontend Integration

See `Frontend/INTERVIEW_UPGRADE_README.md` for:
- Connecting Angular frontend to these APIs
- Voice recording with Whisper integration
- AI avatar and speech synthesis
- Real-time transcript management

---

**Happy Interviewing!** 🎤

For detailed API documentation, see `INTERVIEW_MANAGEMENT_COMPLETE.md`
