# Quick Test Guide - Backend Integration

**Purpose:** Verify the frontend is properly connected to the backend  
**Time Required:** 10 minutes  
**Prerequisites:** Backend running, question bank seeded

---

## ⚡ Quick Start

### 1. Start Backend (Terminal 1)
```bash
cd c:\Users\gazul\OneDrive\Desktop\Projects\CommunicaAI\CommunicaAI
dotnet run
```
**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5169
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### 2. Start Frontend (Terminal 2)
```bash
cd c:\Users\gazul\OneDrive\Desktop\Projects\CommunicaAI\Frontend
npm start
```
**Expected Output:**
```
  ➜  Local:   http://localhost:4200/
  ➜  press h + enter to show help
```

### 3. Seed Question Bank (One-Time Setup)
Open browser console and run:
```javascript
fetch('http://localhost:5169/api/question-bank/seed', {
  method: 'POST',
  headers: {
    'Authorization': 'Bearer YOUR_JWT_TOKEN', // Get from localStorage after login
    'Content-Type': 'application/json'
  }
})
.then(r => r.json())
.then(console.log);
```

Or use Postman/Thunder Client:
- Method: POST
- URL: `http://localhost:5169/api/question-bank/seed`
- Headers: `Authorization: Bearer {token}`

---

## 🧪 Test Scenarios

### Test 1: User Registration (2 minutes)

**Steps:**
1. Navigate to `http://localhost:4200/register`
2. Fill form:
   - Full Name: `Test User`
   - Email: `test@example.com`
   - Password: `Test123!`
   - Audio File: Record 3 seconds "Hello, this is test user"
   - Video File: Record 3 seconds of your face
3. Click "Register"

**Expected Results:**
✅ Redirect to dashboard  
✅ Console shows: "Registration successful"  
✅ localStorage has `token` key  
✅ Backend console shows: "User registered successfully"

**Backend Verification:**
```bash
# Check database
psql -U postgres -d CommunicaAIDB
SELECT * FROM "AppUsers" WHERE "Email" = 'test@example.com';
SELECT * FROM "UserVerificationProfiles";
```

---

### Test 2: Login (1 minute)

**Steps:**
1. Logout if logged in
2. Navigate to `http://localhost:4200/login`
3. Enter:
   - Email: `test@example.com`
   - Password: `Test123!`
4. Click "Login"

**Expected Results:**
✅ Redirect to dashboard  
✅ Console shows: "Login successful"  
✅ localStorage has new `token`

**Verify Token:**
```javascript
// In browser console
const token = localStorage.getItem('token');
console.log('Token:', token);

// Decode JWT (without verification)
const payload = JSON.parse(atob(token.split('.')[1]));
console.log('Payload:', payload);
// Should show: email, sub (userId), exp (expiration)
```

---

### Test 3: Create Interview (2 minutes)

**Steps:**
1. Click "Start Interview" from dashboard
2. Fill form:
   - Role: `Software Engineer`
   - Topic: `Technical Interview`
   - Difficulty: `Medium`
   - Duration: `15` minutes
   - Questions: `5`
3. Click "Start Interview"

**Expected Results:**
✅ Redirect to `/interview/live/{sessionId}`  
✅ Console shows: "Interview session created: {sessionId}"  
✅ Questions loaded from backend  
✅ Timer shows: `15:00`  
✅ First question displayed  
✅ AI starts speaking question (if TTS enabled)

**Backend Verification:**
```sql
-- In psql
SELECT * FROM "InterviewSessions" ORDER BY "StartedAt" DESC LIMIT 1;
SELECT * FROM "InterviewQuestions" WHERE "InterviewSessionId" = 'session-id-here';
```

**Debug in Browser Console:**
```javascript
// Check current session
const session = JSON.parse(sessionStorage.getItem('currentSession'));
console.log('Session:', session);
console.log('Questions count:', session?.questions?.length);
```

---

### Test 4: Record Audio Answer (3 minutes)

**Steps:**
1. On live interview page
2. Wait for AI to finish speaking (or click stop if TTS disabled)
3. Click "Start Answer" 🎤
4. Grant microphone permission
5. Speak your answer for 10-20 seconds:
   ```
   "My approach to this problem would be to first analyze the requirements,
   then design a scalable solution using appropriate design patterns,
   implement the core functionality, write comprehensive tests,
   and finally deploy with proper monitoring."
   ```
6. Click "Stop Answer" 🛑
7. **Wait 5-8 seconds** - This is important!

**Expected Results:**
✅ Loading spinner appears  
✅ Console shows: "Submitting audio to backend..."  
✅ After 5-8 seconds, transcript appears in text area  
✅ Console shows evaluation scores:
```
Answer Evaluation: Overall: 87% | Technical: 85% | Clarity: 90%
```
✅ Question marked as answered (checkmark or color change)

**Backend Verification:**
```sql
-- Check answer was saved
SELECT * FROM "InterviewAnswers" ORDER BY "AnsweredAt" DESC LIMIT 1;

-- Check evaluation was created
SELECT * FROM "AnswerEvaluations" ORDER BY "EvaluatedAt" DESC LIMIT 1;
```

**Backend Console Should Show:**
```
[INFO] Uploading audio to Cloudinary...
[INFO] Audio uploaded: https://res.cloudinary.com/...
[INFO] Transcribing audio with Gemini...
[INFO] Transcript: My approach to this problem...
[INFO] Evaluating answer with Gemini...
[INFO] Evaluation complete: Overall score 87%
```

**Debug:**
```javascript
// In browser console
console.log('Current transcript:', document.querySelector('textarea')?.value);

// Check network tab
// Find request: POST /api/interviews/{sessionId}/answers/audio
// Response should have:
// - transcript: "My approach..."
// - technicalScore: 85
// - clarityScore: 90
// - overallScore: 87
// - feedback: "..."
```

---

### Test 5: Navigate Questions (1 minute)

**Steps:**
1. Click "Next Question" →
2. Record another answer
3. Click "Previous Question" ←
4. Verify previous transcript is still there

**Expected Results:**
✅ Question changes  
✅ Transcript persists when navigating back  
✅ Question counter updates (e.g., "2 of 5")  
✅ AI speaks new question

---

### Test 6: Complete Interview (2 minutes)

**Steps:**
1. Answer at least 2-3 questions
2. Click "Finish Interview"
3. Wait for redirect

**Expected Results:**
✅ Redirect to `/interview/result/{sessionId}`  
✅ Results page displays:
   - Session metadata (role, topic, difficulty)
   - All questions with numbers
   - All transcripts for answered questions
   - Completion percentage
   - Timestamps

**Backend Verification:**
```sql
SELECT * FROM "InterviewSessions" WHERE "Id" = 'session-id' \gx
-- Status should be 'Completed'
-- CompletedAt should have timestamp

SELECT * FROM "InterviewResults" WHERE "InterviewSessionId" = 'session-id' \gx
```

---

## 🔍 Verification Checklist

### Frontend Health Check
- [ ] Application loads without console errors
- [ ] Can navigate between pages
- [ ] Forms are responsive
- [ ] Buttons work
- [ ] Loading spinners appear during async operations

### Backend Health Check
- [ ] Backend responds to `http://localhost:5169/api/test`
- [ ] Database connection successful
- [ ] Cloudinary uploads work
- [ ] Gemini API responds (check API key)
- [ ] JWT tokens are generated

### Integration Health Check
- [ ] Auth interceptor attaches Bearer token
- [ ] Network tab shows 200 OK responses
- [ ] No 401 Unauthorized errors
- [ ] No CORS errors
- [ ] Sessions persist across page refreshes

---

## 🐛 Common Issues & Solutions

### Issue 1: 401 Unauthorized
**Symptom:** All API calls return 401  
**Cause:** JWT token missing or expired  
**Solution:**
```javascript
// Check if token exists
console.log('Token:', localStorage.getItem('token'));

// If missing, login again
// If exists but expired (2 hours), login again
```

### Issue 2: CORS Error
**Symptom:** Browser console shows CORS policy error  
**Cause:** Backend CORS not configured for frontend URL  
**Solution:** Check `Program.cs` has:
```csharp
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### Issue 3: No Questions Loaded
**Symptom:** "No questions found" message  
**Cause:** Question bank not seeded  
**Solution:**
```bash
# Seed question bank
POST http://localhost:5169/api/question-bank/seed
Authorization: Bearer {your_token}
```

### Issue 4: Audio Upload Fails
**Symptom:** "Failed to process audio" error  
**Cause:** Cloudinary not configured  
**Solution:** Check `appsettings.json`:
```json
{
  "CloudinarySettings": {
    "CloudName": "your_cloud_name",
    "ApiKey": "your_api_key",
    "ApiSecret": "your_api_secret"
  }
}
```

### Issue 5: Transcription Fails
**Symptom:** Transcript shows "Error" or empty  
**Cause:** Gemini API key invalid or quota exceeded  
**Solution:** Check `appsettings.json`:
```json
{
  "Gemini": {
    "ApiKey": "YOUR_VALID_API_KEY",
    "Model": "gemini-2.5-flash"
  }
}
```
Verify API key at: https://aistudio.google.com/app/apikey

### Issue 6: Microphone Not Working
**Symptom:** "Could not access microphone" error  
**Cause:** Browser permission denied  
**Solution:**
1. Click 🔒 in address bar
2. Allow microphone permission
3. Refresh page

### Issue 7: Backend Not Starting
**Symptom:** Connection refused on port 5169  
**Cause:** PostgreSQL not running  
**Solution:**
```bash
# Check PostgreSQL status
pg_ctl status

# Start PostgreSQL if needed
pg_ctl start
```

---

## 🎯 Success Criteria

You have successfully verified the integration when:

✅ **Authentication Works**
- Can register new users
- Can login with password
- JWT tokens are stored and used
- Protected endpoints work

✅ **Interview Flow Works**
- Can create interview sessions
- Questions load from backend
- Can record audio answers
- Transcript appears after 5-8 seconds
- Evaluation scores appear in console

✅ **Data Persistence Works**
- Sessions saved in database
- Questions saved in database
- Answers saved in database
- Evaluations saved in database
- Can reload page and continue interview

✅ **AI Integration Works**
- Gemini transcribes audio correctly
- Gemini evaluates answers with scores
- Scores are realistic (not all 0 or 100)
- Feedback text is meaningful

---

## 📊 Quick Database Queries

### View Recent Activity
```sql
-- Recent users
SELECT "Id", "FullName", "Email", "CreatedAtUtc" 
FROM "AppUsers" 
ORDER BY "CreatedAtUtc" DESC 
LIMIT 5;

-- Recent interviews
SELECT "Id", "Role", "Topic", "Difficulty", "Status", "StartedAt" 
FROM "InterviewSessions" 
ORDER BY "StartedAt" DESC 
LIMIT 5;

-- Recent answers
SELECT "Id", "Transcript"::text as "Transcript", "AnsweredAt" 
FROM "InterviewAnswers" 
ORDER BY "AnsweredAt" DESC 
LIMIT 5;

-- Recent evaluations
SELECT "Id", "TechnicalScore", "ClarityScore", "OverallScore", "EvaluatedAt" 
FROM "AnswerEvaluations" 
ORDER BY "EvaluatedAt" DESC 
LIMIT 5;
```

### Check Question Bank
```sql
-- Count questions by role and difficulty
SELECT "Role", "Difficulty", COUNT(*) 
FROM "QuestionBanks" 
GROUP BY "Role", "Difficulty" 
ORDER BY "Role", "Difficulty";

-- Sample questions
SELECT "Role", "Category", "QuestionText"::text 
FROM "QuestionBanks" 
LIMIT 5;
```

---

## 🚀 Performance Benchmarks

**Expected Response Times:**
- Login: < 1 second
- Create interview: < 2 seconds
- Load questions: < 1 second
- **Audio submission: 5-8 seconds** (includes AI processing)
- Load results: < 1 second

**Audio Processing Breakdown:**
- Upload to Cloudinary: 1-2 seconds
- Gemini transcription: 2-3 seconds
- Gemini evaluation: 2-3 seconds
- Save to database: < 0.5 seconds

---

## 📝 Testing Notes Template

Use this template to document your test results:

```
Date: ___________
Tester: ___________

✅ Backend running: Yes/No
✅ Frontend running: Yes/No
✅ Database accessible: Yes/No
✅ Question bank seeded: Yes/No

Test 1 - Registration: Pass/Fail
Notes: _______________________

Test 2 - Login: Pass/Fail
Notes: _______________________

Test 3 - Create Interview: Pass/Fail
Session ID: _______________________
Notes: _______________________

Test 4 - Audio Answer: Pass/Fail
Transcript received: Yes/No
Scores received: Yes/No
Overall Score: _____%
Notes: _______________________

Test 5 - Navigate Questions: Pass/Fail
Notes: _______________________

Test 6 - Complete Interview: Pass/Fail
Notes: _______________________

Overall Status: Pass/Fail
Issues Found: _______________________
```

---

**Good luck testing! 🎉**

If all tests pass, your integration is working perfectly and ready for production use.
