# Quick Test Guide - Interview Feature

## Prerequisites Check
✅ Backend running on http://localhost:5169  
✅ Question bank seeded (154 questions)  
✅ Frontend running (if not, start it)  

## Test Scenario 1: Complete Interview Flow

### Step 1: Register/Login
1. Navigate to `http://localhost:4200/auth/register`
2. Register new account:
   - Email: `test@example.com`
   - Password: `Password123!`
   - First Name: `Test`
   - Last Name: `User`
3. Or login if you already have an account

### Step 2: Start Interview
1. Click "Start Interview" from dashboard or navigate to `/interview/setup`
2. Configure interview:
   ```
   Role: Software Engineer
   Topic: Technical Interview (auto-filled)
   Difficulty: Medium
   Duration: 15 minutes
   Question Count: 5
   ```
3. Click "Start Interview" button

### Expected Result
✅ Navigate to `/interview/live/{sessionId}`  
✅ Question counter shows "Question 1 of 5"  
✅ First question displays  
✅ Record button is enabled  
✅ No 404 errors in console  

### Step 3: Answer Questions
1. Click "Start Recording" 🎤
2. Speak your answer (15-30 seconds)
3. Click "Stop Recording" ⏹️
4. Wait for AI transcription (~2-3 seconds)
5. Wait for AI evaluation (~2-3 seconds)
6. See transcript and score
7. Click "Next Question" ➡️
8. Repeat for remaining questions

### Expected Result
✅ Recording starts/stops correctly  
✅ Transcript appears after recording  
✅ Scores display (Technical, Clarity, Completeness)  
✅ Next button enabled after processing  
✅ Progress bar updates  

### Step 4: Complete Interview
1. After answering all 5 questions
2. Click "Finish Interview"
3. Navigate to results page

### Expected Result
✅ Navigate to `/interview/result/{sessionId}`  
✅ Overall scores display (Technical, Communication, Confidence)  
✅ Individual question scores shown  
✅ Strengths and improvements listed  
✅ AI-generated summary displayed  
✅ Recommendations provided  

### Step 5: View History
1. Navigate to `/interview/history`
2. See your completed interview

### Expected Result
✅ Interview listed with:
   - Role: "Software Engineer"
   - Difficulty badge: "Medium"
   - Status: "Completed"
   - Score: Your overall score
   - Date: Today's date
✅ Click card to view results again  

## Test Scenario 2: Different Roles

### Test Each Role
Try creating interviews with different roles to verify question variety:

1. **Product Manager** (Medium)
   - Should get product strategy questions
   
2. **UX Designer** (Easy)
   - Should get design process questions
   
3. **Data Scientist** (Hard)
   - Should get ML/statistics questions

4. **Marketing Manager** (Medium)
   - Should get marketing strategy questions

### Expected Result
✅ Each role gets role-specific questions  
✅ Questions match the selected difficulty  
✅ Mix of Technical (60%), Behavioral (20%), HR (20%)  

## Test Scenario 3: Different Difficulties

### Easy Interview
```
Role: Software Engineer
Difficulty: Easy
Questions: 3
```
Expected: Basic programming questions (e.g., "What is a variable?")

### Medium Interview
```
Role: Software Engineer
Difficulty: Medium
Questions: 5
```
Expected: Intermediate questions (e.g., "Explain OOP concepts")

### Hard Interview
```
Role: Software Engineer  
Difficulty: Hard
Questions: 7
```
Expected: Advanced questions (e.g., "Design a scalable system")

## Test Scenario 4: Edge Cases

### Minimum Questions
```
Question Count: 1
Duration: 5 minutes
```
Expected: ✅ Works with single question

### Maximum Questions
```
Question Count: 20
Duration: 60 minutes
```
Expected: ✅ Works with 20 questions (if enough questions exist in DB)

### Skip Recording (Audio Only App)
Expected: ⚠️ Cannot submit without recording (by design)

## Common Issues & Solutions

### Issue: 404 on Interview Creation
**Symptom**: Blank page, console shows 404 errors  
**Cause**: Question bank empty  
**Solution**: Run seed command:
```powershell
curl -Method POST http://localhost:5169/api/question-bank/seed
```

### Issue: "Failed to create interview session"
**Symptom**: Error message on setup page  
**Possible Causes**:
1. Backend not running → Start backend with `dotnet run`
2. Not logged in → Login first
3. Database connection issue → Check connection string

### Issue: Audio Recording Not Working
**Symptom**: Microphone permission denied  
**Solution**: 
1. Check browser microphone permissions
2. Use HTTPS or localhost (required for getUserMedia)
3. Allow microphone access when prompted

### Issue: Slow AI Processing
**Symptom**: "Processing..." takes >10 seconds  
**Expected**: 
- Transcription: 2-3 seconds
- Evaluation: 2-3 seconds  
- Total: 4-6 seconds

**If slower**:
1. Check Gemini API key in `appsettings.json`
2. Check internet connection
3. Check backend logs for errors

### Issue: No Questions Display
**Symptom**: Interview page loads but no question text  
**Possible Causes**:
1. No questions for selected role/difficulty
2. Database query issue

**Check**:
```powershell
# View backend logs
# Look for: "No questions found for this session"
```

## Backend Status Check

### Quick Health Check
```powershell
# Test backend is responding
curl -UseBasicParsing http://localhost:5169/api/auth/login

# Should return: 400 Bad Request (means API is accessible)
# Should NOT return: Connection refused (means backend is down)
```

### View Backend Logs
Check the terminal where backend is running for:
- ✅ `Now listening on: http://localhost:5169`
- ❌ Exception messages
- ❌ Database connection errors

## Frontend Status Check

### Check Angular Dev Server
```powershell
# Should be running on port 4200
curl -UseBasicParsing http://localhost:4200

# Should return HTML (Angular app)
```

### Browser Console
Open browser DevTools (F12) and check:
- **Console tab**: No red errors
- **Network tab**: API calls returning 200/201 (not 404/401)
- **Application tab**: JWT token stored in localStorage

## Success Criteria

### ✅ Feature Working When:
1. Can register/login successfully
2. Can configure and start interview
3. Questions load without 404 errors
4. Can record and submit audio answers
5. AI transcription works (2-3 seconds)
6. AI evaluation returns scores (2-3 seconds)
7. Can complete interview
8. Results page displays all scores
9. History page shows completed interviews
10. Can click history item to view results again

### 🎯 Performance Targets:
- Interview creation: < 1 second
- Question loading: < 1 second
- Audio transcription: 2-3 seconds
- Answer evaluation: 2-3 seconds
- Results generation: < 2 seconds

### 🔒 Security Check:
- ✅ Cannot access interview without login
- ✅ Cannot access other user's interviews
- ✅ JWT token required for all interview APIs
- ✅ Token expires after configured time

## Next Steps After Testing

### If Everything Works:
1. ✅ **Mark task as complete**
2. Consider additional features:
   - Admin panel for managing questions
   - Interview sharing/comparison
   - Practice mode (skip AI evaluation)
   - Export results as PDF

### If Issues Found:
1. Check browser console for errors
2. Check backend terminal for exceptions
3. Verify JWT token is being sent
4. Check API responses in Network tab
5. Report specific error messages

## Test Data Cleanup

### Reset Test Data (Optional)
```sql
-- Connect to PostgreSQL database
-- Delete test interviews
DELETE FROM "InterviewSessions" WHERE "UserId" = 'test-user-guid';
DELETE FROM "InterviewQuestions" WHERE "InterviewSessionId" IN (...);
DELETE FROM "InterviewAnswers" WHERE "InterviewQuestionId" IN (...);
```

### Keep Question Bank
Do NOT delete QuestionBank data - it's reference data needed for all interviews.

---

**Happy Testing!** 🚀

Report any issues with:
- Specific steps to reproduce
- Error messages from console
- Network request/response details
- Backend log output
