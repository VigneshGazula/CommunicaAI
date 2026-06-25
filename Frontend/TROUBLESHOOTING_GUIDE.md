# Troubleshooting Guide - Backend Integration

**Purpose:** Diagnose and fix common integration issues  
**Audience:** Developers  
**Last Updated:** June 25, 2026

---

## 🔧 Diagnostic Tools

### 1. Browser Developer Tools
**Access:** Press F12 or Right-click → Inspect

**Console Tab:**
- View JavaScript errors
- Check service method logs
- Inspect object values
- Test API calls manually

**Network Tab:**
- Monitor HTTP requests
- Check request headers (Authorization)
- Inspect response status codes
- View request/response payloads

**Application Tab:**
- View localStorage (JWT token)
- Clear storage if needed
- Check cookies

### 2. Backend Console
**Terminal showing backend output**

Look for:
- HTTP request logs
- SQL queries (if logging enabled)
- Exception stack traces
- Service method calls

### 3. Database Client
**pgAdmin or psql**

```bash
# Connect via psql
psql -U postgres -d CommunicaAIDB

# View tables
\dt

# View specific table
\d "InterviewSessions"

# Query data
SELECT * FROM "AppUsers";
```

---

## 🚨 Common Errors & Solutions

## Error Category: Authentication

### Error 1: "401 Unauthorized" on All Requests

**Symptoms:**
- All API calls return 401
- Console shows "Unauthorized" errors
- User redirected to login repeatedly

**Diagnosis:**
```javascript
// Check if token exists
const token = localStorage.getItem('token');
console.log('Token exists:', !!token);

if (token) {
  // Decode token (without verification)
  const parts = token.split('.');
  const payload = JSON.parse(atob(parts[1]));
  console.log('Token payload:', payload);
  console.log('Token expired:', Date.now() >= payload.exp * 1000);
}
```

**Possible Causes:**
1. Token expired (2-hour lifetime)
2. Token not stored correctly
3. Interceptor not attaching token
4. Backend JWT validation failed

**Solutions:**

**Solution 1: Token Expired → Re-login**
```javascript
// Token expired, user must login again
localStorage.removeItem('token');
window.location.href = '/login';
```

**Solution 2: Verify Interceptor**
Check `app.config.ts`:
```typescript
export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([authInterceptor])) // ✅ Must be here
  ]
};
```

**Solution 3: Check Token Format**
```javascript
// Token should look like: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
const token = localStorage.getItem('token');
console.log('Token format valid:', /^[\w-]+\.[\w-]+\.[\w-]+$/.test(token));
```

**Solution 4: Backend JWT Settings**
Check `appsettings.json`:
```json
{
  "Jwt": {
    "Issuer": "CommunicaAI",
    "Audience": "CommunicaAIUsers",
    "Key": "THIS_IS_A_DEMO_SECRET_KEY_CHANGE_IT_TO_A_LONG_RANDOM_SECRET"
  }
}
```

And `Program.cs`:
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });
```

---

### Error 2: "Login Failed: Invalid Credentials"

**Symptoms:**
- Password login returns 401
- Console shows "Invalid email or password"

**Diagnosis:**
```javascript
// Check request payload
const loginData = {
  email: 'test@example.com',
  password: 'Test123!'
};
console.log('Login data:', loginData);

// Check response
fetch('http://localhost:5169/api/auth/login/password', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify(loginData)
})
.then(r => r.json())
.then(console.log)
.catch(console.error);
```

**Possible Causes:**
1. Email not registered
2. Wrong password
3. Email case mismatch
4. Password not meeting requirements

**Solutions:**

**Solution 1: Verify User Exists**
```sql
-- In psql
SELECT "Id", "Email", "FullName" FROM "AppUsers" WHERE "Email" = 'test@example.com';
```

**Solution 2: Reset Password**
```sql
-- You'll need to hash the new password using ASP.NET Core Identity
-- Or register a new user
```

**Solution 3: Check Email Format**
```javascript
// Email should be lowercase
const email = 'Test@Example.com'.toLowerCase(); // ✅ 'test@example.com'
```

---

### Error 3: "Registration Failed: Email Already Exists"

**Symptoms:**
- Registration returns 409 Conflict
- Console shows "Email already registered"

**Diagnosis:**
```sql
-- Check if email exists
SELECT * FROM "AppUsers" WHERE "Email" = 'test@example.com';
```

**Solutions:**

**Solution 1: Use Different Email**
```typescript
const email = 'test2@example.com'; // ✅ New email
```

**Solution 2: Delete Existing User (Development Only)**
```sql
-- WARNING: Development only!
DELETE FROM "AppUsers" WHERE "Email" = 'test@example.com';
```

---

## Error Category: Interview Sessions

### Error 4: "Session Not Found" (404)

**Symptoms:**
- Live interview page shows 404
- Console shows "Session not found or unauthorized"

**Diagnosis:**
```javascript
// Check session ID in URL
const sessionId = window.location.pathname.split('/').pop();
console.log('Session ID:', sessionId);
console.log('Is valid GUID:', /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(sessionId));
```

**Possible Causes:**
1. Invalid session ID in URL
2. Session belongs to different user
3. Session deleted from database

**Solutions:**

**Solution 1: Verify Session Exists**
```sql
SELECT * FROM "InterviewSessions" WHERE "Id" = 'session-guid-here';
```

**Solution 2: Verify Ownership**
```sql
SELECT "Id", "UserId" FROM "InterviewSessions" WHERE "Id" = 'session-guid-here';
-- Compare UserId with your JWT token's sub claim
```

**Solution 3: Create New Session**
```typescript
// Navigate to setup page and create new interview
this.router.navigate(['/interview/setup']);
```

---

### Error 5: "No Questions Found"

**Symptoms:**
- Live interview page loads but no questions
- Console shows "No questions found"

**Diagnosis:**
```sql
-- Check if questions exist for session
SELECT * FROM "InterviewQuestions" WHERE "InterviewSessionId" = 'session-guid-here';

-- Check question bank has questions
SELECT COUNT(*) FROM "QuestionBanks";
```

**Possible Causes:**
1. Question bank not seeded
2. No questions for role/difficulty combination
3. Question generation failed

**Solutions:**

**Solution 1: Seed Question Bank**
```bash
# Via API (requires auth token)
POST http://localhost:5169/api/question-bank/seed
Authorization: Bearer {your_token}
```

**Solution 2: Verify Question Bank**
```sql
-- Check available questions
SELECT "Role", "Difficulty", COUNT(*) 
FROM "QuestionBanks" 
GROUP BY "Role", "Difficulty";
```

**Solution 3: Add Questions Manually**
```sql
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt")
VALUES 
(gen_random_uuid(), 'Software Engineer', 'Technical', 'medium', 'What is a closure in JavaScript?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'medium', 'Explain the difference between SQL and NoSQL.', NOW());
```

---

## Error Category: Audio Recording

### Error 6: "Could Not Access Microphone"

**Symptoms:**
- Recording button disabled
- Console shows "Could not access microphone"
- Browser shows "Permission denied"

**Diagnosis:**
```javascript
// Check if getUserMedia is supported
console.log('getUserMedia supported:', !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia));

// Try to get microphone access
navigator.mediaDevices.getUserMedia({ audio: true })
  .then(() => console.log('✅ Microphone access granted'))
  .catch(err => console.error('❌ Microphone error:', err));
```

**Possible Causes:**
1. Browser permission denied
2. No microphone connected
3. Microphone used by another app
4. HTTPS required (some browsers)

**Solutions:**

**Solution 1: Grant Browser Permission**
1. Click 🔒 in address bar
2. Find "Microphone" setting
3. Select "Allow"
4. Refresh page

**Solution 2: Check Microphone Hardware**
```javascript
// List available audio devices
navigator.mediaDevices.enumerateDevices()
  .then(devices => {
    const microphones = devices.filter(d => d.kind === 'audioinput');
    console.log('Microphones:', microphones);
    console.log('Count:', microphones.length);
  });
```

**Solution 3: Close Other Apps**
- Close Zoom, Teams, Discord, etc.
- Check system sound settings
- Restart browser

**Solution 4: Use HTTPS (Production)**
```bash
# For production, serve over HTTPS
# getUserMedia requires secure context
```

---

### Error 7: "Failed to Process Audio"

**Symptoms:**
- Recording completes but submission fails
- Console shows "Failed to process audio"
- Loading spinner disappears without transcript

**Diagnosis:**
```javascript
// Check network tab for failed request
// Look for: POST /api/interviews/{sessionId}/answers/audio
// Status: 400, 500, or error

// Check console for error details
console.log('Check network tab for audio submission error');
```

**Possible Causes:**
1. Cloudinary upload failed
2. Gemini API error
3. Invalid audio format
4. Network timeout

**Solutions:**

**Solution 1: Verify Cloudinary Config**
```json
// In appsettings.json
{
  "CloudinarySettings": {
    "CloudName": "your_cloud_name", // ✅ Must be valid
    "ApiKey": "your_api_key",       // ✅ Must be valid
    "ApiSecret": "your_api_secret"  // ✅ Must be valid
  }
}
```

**Solution 2: Verify Gemini API Key**
```json
// In appsettings.json
{
  "Gemini": {
    "ApiKey": "YOUR_VALID_API_KEY", // ✅ Get from https://aistudio.google.com/app/apikey
    "Model": "gemini-2.5-flash"
  }
}
```

**Solution 3: Check Backend Logs**
```
Look for:
[ERROR] Cloudinary upload failed: ...
[ERROR] Gemini API error: ...
[ERROR] Transcription failed: ...
```

**Solution 4: Test Audio Format**
```javascript
// Check audio blob is valid
const audioBlob = new Blob(audioChunks, { type: 'audio/webm' });
console.log('Audio size:', audioBlob.size, 'bytes');
console.log('Audio type:', audioBlob.type);
// Size should be > 1000 bytes for valid recording
```

---

### Error 8: "Transcript Is Empty or Incorrect"

**Symptoms:**
- Audio uploads successfully
- Transcript appears but is wrong or empty
- Scores are 0

**Diagnosis:**
```javascript
// Check response in network tab
// Response should have:
{
  "transcript": "...",
  "technicalScore": 85,
  "clarityScore": 90,
  // ...
}

// Empty transcript means Gemini couldn't transcribe
// Check audio quality
```

**Possible Causes:**
1. Audio too quiet
2. Background noise
3. Audio too short (< 2 seconds)
4. Gemini API quota exceeded
5. Audio encoding issue

**Solutions:**

**Solution 1: Check Audio Quality**
- Speak clearly and loudly
- Reduce background noise
- Record for at least 5 seconds
- Use a good microphone

**Solution 2: Check Gemini Quota**
```bash
# Visit: https://aistudio.google.com/app/apikey
# Check quota limits
# Free tier: 15 requests per minute
```

**Solution 3: Test Transcription Endpoint**
```bash
# Use backend test endpoint
POST http://localhost:5169/api/test/transcribe
Content-Type: multipart/form-data

audioFile: [your-audio-file.webm]
```

**Solution 4: Check Audio Encoding**
```javascript
// Try different MIME type
const mimeType = MediaRecorder.isTypeSupported('audio/webm;codecs=opus')
  ? 'audio/webm;codecs=opus'
  : 'audio/webm';

const mediaRecorder = new MediaRecorder(stream, { mimeType });
```

---

## Error Category: CORS & Network

### Error 9: "CORS Policy Error"

**Symptoms:**
- Console shows "Access to fetch at ... has been blocked by CORS policy"
- Network tab shows OPTIONS requests failing

**Diagnosis:**
```javascript
// Check if backend allows frontend origin
fetch('http://localhost:5169/api/auth/me', {
  method: 'GET',
  headers: { 'Authorization': 'Bearer token' }
})
.then(r => console.log('Response:', r))
.catch(e => console.error('CORS error:', e));
```

**Possible Causes:**
1. Backend CORS not configured
2. Frontend URL not in AllowedOrigins
3. Credentials not included

**Solutions:**

**Solution 1: Configure CORS in Backend**
```csharp
// In Program.cs
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:4200") // ✅ Frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ✅ If using cookies
    });
});

// Use CORS middleware
app.UseCors(); // ✅ Before UseAuthorization()
```

**Solution 2: Add Multiple Origins (Development)**
```csharp
policy.WithOrigins(
    "http://localhost:4200",
    "http://localhost:4201",
    "http://127.0.0.1:4200"
)
```

**Solution 3: Allow All Origins (Development Only)**
```csharp
// WARNING: Development only!
policy.AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();
```

---

### Error 10: "Network Request Failed"

**Symptoms:**
- All requests fail
- Console shows "Failed to fetch" or "Network error"

**Diagnosis:**
```javascript
// Test backend connectivity
fetch('http://localhost:5169/api/test')
  .then(r => r.json())
  .then(data => console.log('✅ Backend reachable:', data))
  .catch(err => console.error('❌ Backend unreachable:', err));
```

**Possible Causes:**
1. Backend not running
2. Wrong port number
3. Firewall blocking connection
4. Backend crashed

**Solutions:**

**Solution 1: Start Backend**
```bash
cd CommunicaAI
dotnet run
```

**Solution 2: Verify Port**
```typescript
// In environment.ts
export const environment = {
  apiBaseUrl: 'http://localhost:5169' // ✅ Check port matches backend
};
```

**Solution 3: Check Backend Logs**
```bash
# Look for startup errors
# Backend should show: "Now listening on: http://localhost:5169"
```

---

## Error Category: Database

### Error 11: "Database Connection Failed"

**Symptoms:**
- Backend crashes on startup
- Error: "Npgsql.PostgresException"
- Error: "Connection refused"

**Diagnosis:**
```bash
# Check if PostgreSQL is running
pg_isready
# Should output: "accepting connections"

# Check connection string
cat appsettings.json | grep DefaultConnection
```

**Possible Causes:**
1. PostgreSQL not running
2. Wrong credentials
3. Database doesn't exist
4. Wrong host/port

**Solutions:**

**Solution 1: Start PostgreSQL**
```bash
# Windows
pg_ctl start

# Linux/Mac
sudo service postgresql start
# or
brew services start postgresql
```

**Solution 2: Verify Connection String**
```json
// In appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=CommunicaAIDB;Username=postgres;Password=Vignesh@123"
  }
}
```

**Solution 3: Create Database**
```bash
# Connect to PostgreSQL
psql -U postgres

# Create database
CREATE DATABASE "CommunicaAIDB";

# Exit
\q
```

**Solution 4: Run Migrations**
```bash
cd CommunicaAI
dotnet ef database update
```

---

### Error 12: "Table Does Not Exist"

**Symptoms:**
- Backend error: "relation \"AppUsers\" does not exist"
- SQL error in backend console

**Diagnosis:**
```sql
-- Connect to database
psql -U postgres -d CommunicaAIDB

-- List tables
\dt

-- Should see:
-- AppUsers
-- InterviewSessions
-- InterviewQuestions
-- InterviewAnswers
-- AnswerEvaluations
-- UserVerificationProfiles
-- QuestionBanks
```

**Possible Causes:**
1. Migrations not run
2. Wrong database
3. Tables were dropped

**Solutions:**

**Solution 1: Run Migrations**
```bash
cd CommunicaAI
dotnet ef database update
```

**Solution 2: Recreate Database**
```bash
# Drop and recreate (WARNING: Deletes all data!)
dotnet ef database drop --force
dotnet ef database update
```

**Solution 3: Verify Connection**
```sql
-- Check you're in the right database
SELECT current_database();
-- Should be: CommunicaAIDB
```

---

## 🔍 Debugging Techniques

### 1. Enable Verbose Logging

**Frontend:**
```typescript
// In interview.service.ts
submitAudioAnswer(...): Observable<SubmitAudioAnswerResponse> {
  console.log('📤 Submitting audio:', { sessionId, questionId, size: audioBlob.size });
  
  return this.http.post<SubmitAudioAnswerResponse>(url, formData).pipe(
    tap(response => console.log('✅ Response:', response)),
    catchError(error => {
      console.error('❌ Error:', error);
      return throwError(() => error);
    })
  );
}
```

**Backend:**
```csharp
// In InterviewAnswerController.cs
[HttpPost("{sessionId}/answers/audio")]
public async Task<ActionResult<SubmitAudioAnswerResponse>> SubmitAudioAnswer(...)
{
    _logger.LogInformation("📤 Received audio submission for session {SessionId}", sessionId);
    
    try {
        var result = await _service.SubmitAudioAnswerAsync(...);
        _logger.LogInformation("✅ Audio processed successfully");
        return Ok(result);
    }
    catch (Exception ex) {
        _logger.LogError(ex, "❌ Error processing audio");
        throw;
    }
}
```

### 2. Test Individual Components

**Test Backend Endpoint:**
```bash
# Using curl
curl -X POST http://localhost:5169/api/interviews \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"role":"Software Engineer","topic":"Test","difficulty":"medium","questionCount":5,"durationMinutes":15}'
```

**Test Frontend Service:**
```javascript
// In browser console
const service = window['ng']?.probe(document.querySelector('app-root')).injector.get('InterviewService');
service.createSession({...}).subscribe(console.log);
```

### 3. Monitor Database Activity

```sql
-- View active connections
SELECT pid, usename, application_name, state 
FROM pg_stat_activity 
WHERE datname = 'CommunicaAIDB';

-- View recent queries (if logging enabled)
SELECT query, query_start 
FROM pg_stat_activity 
WHERE datname = 'CommunicaAIDB' 
ORDER BY query_start DESC;
```

### 4. Use Postman/Thunder Client

Create collection with requests:
```
POST {{baseUrl}}/api/auth/register
POST {{baseUrl}}/api/auth/login/password
POST {{baseUrl}}/api/interviews
GET {{baseUrl}}/api/interviews/{{sessionId}}
POST {{baseUrl}}/api/interviews/{{sessionId}}/answers/audio
```

### 5. Check External Services

**Cloudinary:**
```bash
# Visit dashboard
https://console.cloudinary.com/

# Check recent uploads
# Verify API credentials
```

**Gemini AI:**
```bash
# Visit API console
https://aistudio.google.com/app/apikey

# Check quota usage
# Verify API key is valid
```

---

## 📊 Health Check Script

Create a simple health check:

**Frontend (TypeScript):**
```typescript
// health-check.service.ts
@Injectable({ providedIn: 'root' })
export class HealthCheckService {
  async runChecks(): Promise<void> {
    console.log('🏥 Running health checks...\n');
    
    // 1. Check localStorage
    const token = localStorage.getItem('token');
    console.log('✅ Token exists:', !!token);
    
    // 2. Check API connectivity
    try {
      await fetch('http://localhost:5169/api/test').then(r => r.json());
      console.log('✅ Backend reachable');
    } catch {
      console.error('❌ Backend unreachable');
    }
    
    // 3. Check microphone
    try {
      await navigator.mediaDevices.getUserMedia({ audio: true });
      console.log('✅ Microphone accessible');
    } catch {
      console.error('❌ Microphone not accessible');
    }
    
    // 4. Check auth status
    try {
      const response = await fetch('http://localhost:5169/api/auth/me', {
        headers: { 'Authorization': `Bearer ${token}` }
      });
      console.log('✅ Authenticated:', response.ok);
    } catch {
      console.error('❌ Not authenticated');
    }
  }
}
```

**Backend (C#):**
```csharp
// HealthCheckController.cs
[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            database = CheckDatabase(),
            cloudinary = CheckCloudinary(),
            gemini = CheckGemini()
        });
    }
    
    private bool CheckDatabase()
    {
        try {
            _context.Database.CanConnect();
            return true;
        } catch {
            return false;
        }
    }
}
```

---

## 🎯 Escalation Path

If you still can't resolve the issue:

### Level 1: Self-Diagnosis
- ✅ Checked this troubleshooting guide
- ✅ Reviewed console logs
- ✅ Inspected network tab
- ✅ Verified configuration files

### Level 2: Documentation
- ✅ Read INTEGRATION_STATUS_SUMMARY.md
- ✅ Read QUICK_TEST_GUIDE.md
- ✅ Read backend COMPLETE_ARCHITECTURE_REFERENCE.md

### Level 3: Stack Overflow
- Search for specific error message
- Include framework versions
- Provide minimal reproducible example

### Level 4: GitHub Issues
- Check Angular repository
- Check ASP.NET Core repository
- Check third-party library repositories

---

## 📝 Logging Best Practices

### What to Log

**DO Log:**
- API request start/end
- Authentication success/failure
- Database operations
- External service calls
- Errors with stack traces

**DON'T Log:**
- JWT tokens
- Passwords
- API keys
- Personal data (PII)

### Log Levels

```csharp
// Backend
_logger.LogTrace("Detailed trace");      // Development only
_logger.LogDebug("Debug info");          // Development only
_logger.LogInformation("Important event");// Production
_logger.LogWarning("Warning");           // Production
_logger.LogError(ex, "Error occurred");  // Production
_logger.LogCritical(ex, "Critical!");    // Production
```

```typescript
// Frontend
console.debug('Debug info');       // Development
console.log('Info');               // Development/Production
console.warn('Warning');           // Production
console.error('Error');            // Production
```

---

## 🎓 Prevention Tips

1. **Always check backend logs** before assuming frontend issue
2. **Use network tab** to see actual HTTP traffic
3. **Test backend endpoints directly** with Postman first
4. **Keep dependencies updated** but test after updates
5. **Document custom changes** in code comments
6. **Use environment variables** for configuration
7. **Write tests** for critical paths
8. **Monitor third-party service quotas** (Cloudinary, Gemini)

---

**Remember:** Most issues are configuration or connectivity problems, not code bugs. Check the basics first!

**Good luck troubleshooting! 🔧**
