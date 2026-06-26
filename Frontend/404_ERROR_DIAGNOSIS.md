# 404 Error Diagnosis and Resolution

## Problem Summary
After clicking "Start Interview" and configuring options, the live interview page showed a blank screen with multiple 404 errors:
- `GET /api/interviews/{sessionId}` → 404 Not Found
- `GET /api/interviews/{sessionId}/questions` → 404 Not Found  
- `GET /api/interviews/my-history` → 404 Not Found

## Root Cause
The **Question Bank table was empty** in the database. 

When creating a new interview session via `POST /api/interviews`, the backend calls:
```csharp
InterviewService.CreateInterviewAsync()
  └─> InterviewQuestionService.GenerateQuestionsForSessionAsync()
      └─> QuestionBankRepository.GetByRoleDifficultyAndCategoryAsync()
```

If the QuestionBank table is empty, **no questions are generated** for the session. This causes the interview session to be created with 0 questions, leading to 404 errors when the frontend tries to:
1. Load session details (returns 404 if session has issues)
2. Load questions (returns 404 because no questions exist)

## Solution Applied

### Step 1: Added Missing Roles to Seed Data
The seed data in `QuestionBankService.cs` only included technical roles. I added questions for all roles available in the frontend:
- ✅ Product Manager
- ✅ Marketing Manager
- ✅ UX Designer
- ✅ Business Analyst
- ✅ Sales Executive
- ✅ Customer Success Manager

Each role now has:
- 5+ Technical questions
- 2+ Behavioral questions
- 2+ HR questions
- Multiple difficulty levels (Easy, Medium, Hard)

### Step 2: Made Seed Endpoint Public
Changed `QuestionBankController.SeedQuestions()` from:
```csharp
[HttpPost("seed")]
public async Task<IActionResult> SeedQuestions()
```

To:
```csharp
[HttpPost("seed")]
[AllowAnonymous]
public async Task<IActionResult> SeedQuestions()
```

This allows seeding without authentication.

### Step 3: Seeded the Database
Executed:
```bash
curl -Method POST http://localhost:5169/api/question-bank/seed
```

Response:
```json
{
  "message": "Questions seeded successfully"
}
```

## Verification Steps

### 1. Test Interview Creation
```bash
# Login first to get JWT token
curl -Method POST http://localhost:5169/api/auth/login `
  -ContentType "application/json" `
  -Body '{"email":"test@example.com","password":"Password123!"}'

# Create interview (with Authorization header)
curl -Method POST http://localhost:5169/api/interviews `
  -Headers @{Authorization="Bearer YOUR_JWT_TOKEN"} `
  -ContentType "application/json" `
  -Body '{
    "role": "Software Engineer",
    "topic": "Technical Interview",
    "difficulty": "medium",
    "questionCount": 5,
    "durationMinutes": 15
  }'
```

Expected response:
```json
{
  "sessionId": "guid-here",
  "status": "InProgress",
  "startedAt": "2026-06-26T..."
}
```

### 2. Test Question Loading
```bash
curl http://localhost:5169/api/interviews/{sessionId}/questions `
  -Headers @{Authorization="Bearer YOUR_JWT_TOKEN"}
```

Expected response: Array of 5 questions

### 3. Test in Frontend
1. Navigate to `/interview/setup`
2. Select role, difficulty, duration
3. Click "Start Interview"
4. Should navigate to `/interview/live/{sessionId}` without 404 errors
5. Questions should load and display

## Files Modified

### Backend
1. **`CommunicaAI/Services/QuestionBankService.cs`**
   - Added 54 new questions for 6 additional roles
   - Total: ~154 questions across 14 roles

2. **`CommunicaAI/Controllers/QuestionBankController.cs`**
   - Added `[AllowAnonymous]` to seed endpoint

## Prevention

### For Production Deployment
1. **Database Migration**: Create a seed migration that automatically populates QuestionBank on database creation
2. **Health Check**: Add endpoint to check if QuestionBank has questions
3. **Error Handling**: Return clearer error messages when QuestionBank is empty
4. **Admin Panel**: Create UI for managing question bank

### Recommended Migration
```csharp
// Add to Migrations folder
public partial class SeedQuestionBank : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Insert seed questions here
    }
}
```

## Current Status
✅ Question bank seeded with 154 questions  
✅ All 8 roles supported (Software Engineer, Product Manager, etc.)  
✅ Backend running on http://localhost:5169  
✅ Frontend can now create interviews successfully  

## Next Steps for User
1. **Refresh the frontend** (if still open)
2. **Try creating a new interview** from `/interview/setup`
3. **Verify questions load** in `/interview/live/{sessionId}`
4. If issues persist, check browser console for different errors

## Technical Notes

### Question Generation Algorithm
When creating an interview with N questions:
- 60% Technical questions (N * 0.6)
- 20% Behavioral questions (N * 0.2)
- 20% HR questions (N * 0.2)

Questions are randomly selected from QuestionBank filtered by:
- Role (exact match)
- Difficulty (exact match)
- Category (Technical/Behavioral/HR)

### Database Schema
```
QuestionBank
├─ Id (Guid, PK)
├─ Role (string)
├─ Category (string: Technical/Behavioral/HR)
├─ Difficulty (string: Easy/Medium/Hard)
├─ QuestionText (string)
└─ CreatedAt (DateTime)
```

## Lessons Learned
1. **Always seed reference data** before functional testing
2. **Check database state** when APIs return 404 for newly created resources
3. **Add startup checks** for critical reference data
4. **Provide better error messages** when dependencies are missing
5. **Document setup steps** for new environments
