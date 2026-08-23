# Audio Transcription Troubleshooting Guide

## Quick Diagnostic Steps

### Step 1: Check Backend Logs
When you submit an audio answer, you should now see detailed logs in your backend console:

```
=== Submit Audio Answer ===
Session: [guid], Question: [guid], User: [guid]
Audio: answer.webm, Size: 45632 bytes, Type: audio/webm
Uploading audio to Cloudinary...
Upload successful: https://res.cloudinary.com/...
Starting transcription...
Audio transcription - Size: 45632 bytes, ContentType: audio/webm
Transcription attempt 1/4
Transcription successful: Hello, my answer is...
```

### Step 2: Identify the Error Point

#### If logs show:
```
Audio transcription - Size: 0 bytes
Audio file is empty
```
**Problem**: Frontend not recording audio properly
**Solution**: Check browser microphone permissions and recording functionality

#### If logs show:
```
Gemini API error: 403
Response: { "error": { "code": 403, "message": "API key not valid" }}
```
**Problem**: Invalid or missing Gemini API key
**Solution**: 
1. Check `appsettings.json` or `appsettings.Development.json`
2. Verify Gemini API key at https://aistudio.google.com/app/apikey
3. Update the key in configuration

#### If logs show:
```
Gemini API error: 429
Response: { "error": { "code": 429, "message": "Resource has been exhausted" }}
```
**Problem**: Gemini API quota exceeded
**Solution**: 
1. Check quota at https://aistudio.google.com/
2. Wait for quota reset (usually daily)
3. Consider upgrading API plan if needed

#### If logs show:
```
Rate limited. Retrying in 2000ms
Rate limited. Retrying in 4000ms
```
**Problem**: Too many requests in short time
**Solution**: Service will auto-retry with backoff (this is normal)

#### If logs show:
```
Warning: Unusual content type: audio/xyz
```
**Problem**: Browser sending unsupported audio format
**Solution**: Check frontend MediaRecorder MIME type configuration

---

## Common Issues & Solutions

### Issue 1: "Failed to transcribe audio" on LAST question only

**Possible Causes**:
1. **API Quota Almost Exhausted**: First 9 questions work, 10th fails when quota runs out
2. **Rate Limiting Cumulative Effect**: Multiple requests in short time trigger rate limit
3. **Larger Audio File**: Last answer might be longer, hitting size/timeout limits

**Debug Steps**:
```bash
# Check your Gemini API usage
# Visit: https://aistudio.google.com/app/apikey
# Look at: Requests per minute (RPM) and Tokens per minute (TPM)
```

**Solutions**:
- Add delay between questions (already handled by retry logic)
- Check Gemini quota and upgrade if needed
- Monitor audio file sizes in logs

### Issue 2: Transcription works locally but fails in production

**Possible Causes**:
1. **Environment Variables**: Gemini API key not set in production
2. **Network Issues**: Production server can't reach Gemini API
3. **Timeout**: Production has stricter timeouts

**Debug Steps**:
```bash
# Check production environment variables
echo $GEMINI_API_KEY
echo $GEMINI_MODEL

# Test Gemini API access from production server
curl -H "Content-Type: application/json" \
  -d '{"contents":[{"parts":[{"text":"Hello"}]}]}' \
  "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key=YOUR_KEY"
```

### Issue 3: "Failed to process audio answer" generic error

**New Behavior**: You should now see specific error details like:
- "Audio file is empty"
- "Gemini API returned 403: API key not valid"
- "Failed to upload to Cloudinary"

**If still generic**: Check that backend changes are deployed

---

## Verifying the Fix

### Test 1: Check Randomization Works
```bash
# Start 2 different interviews with same parameters
# Role: Software Engineer
# Difficulty: Medium
# Questions: 10

# Compare the questions
# ✅ EXPECTED: Questions are different
# ❌ OLD BUG: Questions are identical
```

### Test 2: Check Error Messages Are Detailed
```bash
# Scenario A: Invalid API Key
# 1. Set wrong API key in appsettings.json
# 2. Try to submit audio answer
# 3. Check backend logs

# ✅ EXPECTED:
# "Gemini API error: 403"
# "Response: { ... API key not valid ... }"

# Scenario B: Empty Audio
# 1. Submit empty/corrupted audio file
# 2. Check backend logs

# ✅ EXPECTED:
# "Audio transcription - Size: 0 bytes"
# "Audio file is empty"
```

---

## Configuration Requirements

### Backend Configuration

**appsettings.json** (Local Development):
```json
{
  "Gemini": {
    "ApiKey": "AIzaSyXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
    "Model": "gemini-2.0-flash-exp"
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "123456789012345",
    "ApiSecret": "your-api-secret"
  }
}
```

**Environment Variables** (Production - Render.com):
```bash
GEMINI_API_KEY=AIzaSyXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
GEMINI_MODEL=gemini-2.0-flash-exp
CLOUDINARY_CLOUD_NAME=your-cloud-name
CLOUDINARY_API_KEY=123456789012345
CLOUDINARY_API_SECRET=your-api-secret
```

### Getting Gemini API Key

1. Visit https://aistudio.google.com/app/apikey
2. Sign in with Google account
3. Click "Create API Key"
4. Copy the key (starts with "AIza...")
5. Add to your configuration

**Important**: 
- Free tier: 15 requests per minute
- Each transcription = 1 request
- Interview with 10 questions = 10 requests
- Need to pace requests or upgrade plan

---

## Monitoring Recommendations

### During Interview Session

Watch backend logs for this flow:
```
=== Submit Audio Answer ===
✅ Session validated
✅ Question validated
✅ Uploading audio to Cloudinary...
✅ Upload successful
✅ Starting transcription...
✅ Transcription attempt 1/4
✅ Transcription successful
✅ Answer created
```

### Red Flags to Watch For

```
❌ Audio transcription - Size: 0 bytes
   → Frontend recording issue

❌ Gemini API error: 403
   → Invalid API key

❌ Gemini API error: 429
   → Quota exceeded or rate limited

❌ Failed to upload to Cloudinary
   → Cloudinary credentials issue

❌ Failed to transcribe audio after multiple retries
   → Network or persistent API issue
```

---

## Emergency Fallback

If transcription continues to fail despite fixes:

### Option 1: Text-Only Mode
Temporarily allow users to type answers instead of recording:
```typescript
// Frontend: Add text input as fallback
<textarea 
  placeholder="Type your answer here if audio fails"
  [(ngModel)]="textAnswer">
</textarea>
```

### Option 2: Skip Transcription Temporarily
```csharp
// Backend: Return placeholder transcript
var transcript = "Audio recorded successfully. Transcription pending.";
```

### Option 3: Use Alternative Model
```json
// Try different Gemini model
"Gemini": {
  "Model": "gemini-1.5-flash"  // Older, more stable model
}
```

---

## Success Indicators

After deploying these fixes, you should see:

✅ **Detailed Logs**: Every step of audio processing is logged
✅ **Specific Errors**: Know exactly what failed (API key, quota, empty file, etc.)
✅ **Unique Questions**: Every interview has different questions
✅ **Better UX**: Users see meaningful error messages, not generic failures

---

## Need More Help?

If transcription still fails after trying above steps:

1. **Copy full backend logs** from when error occurs
2. **Check these specific details**:
   - Audio file size in logs
   - Gemini API response
   - Cloudinary upload status
3. **Share the error details** with exact error message

The enhanced logging will make it much easier to diagnose the exact problem!
