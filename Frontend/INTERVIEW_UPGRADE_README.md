# Communica AI - Interview Page Upgrade

## Overview
Enhanced live interview experience with AI speech synthesis, voice recording, animated avatars, and real-time transcription.

---

## New Features

### 1. **Animated AI Avatar**
- **Gradient circular avatar** with animated core
- **Pulse animation** when AI is speaking
- **Waveform visualization** below avatar during speech
- **Dynamic status text**: "AI Speaking...", "Your Turn", "Recording...", "Ready"

### 2. **Speech Synthesis (Browser-based)**
- AI interviewer **reads each question aloud** using Web Speech API
- Automatic playback on question load
- **Manual replay button** to re-hear the question
- **Disables controls** while AI is speaking
- **Smooth transitions** between speech states

### 3. **Show/Hide Captions Toggle**
- **Button in sidebar** to show/hide question text
- When **captions hidden**: Question text replaced with icon placeholder
- When **captions shown**: Full question displayed in card
- **AI still speaks** regardless of caption state
- Remembers user preference during session

### 4. **Voice Recording**
- **"Start Answer" button** to begin recording
- **"Stop Answer" button** to end recording
- Uses **MediaRecorder API** for browser-based audio capture
- **Animated waveform** displays during recording
- Auto-transcribes on stop (mock implementation)

### 5. **Transcript Panel**
- **Live transcript** of user's spoken answers
- Appends new recordings to existing transcript
- **"Clear" button** to reset transcript
- **Auto-saves** to interview session
- Scrollable container for long transcripts

### 6. **State Management**
Four distinct states:
1. **idle** - Initial state before AI speaks
2. **ai-speaking** - AI reading question (controls disabled)
3. **user-turn** - User can record answer (controls enabled)
4. **user-recording** - Recording in progress

### 7. **Speech Transcription Service**
- **Abstraction layer** for future Whisper integration
- **Mock implementation** returns placeholder text
- Returns `TranscriptionResult` with text, confidence, timestamp
- **Ready for backend swap** - just replace service method

---

## Technical Implementation

### Services Created

#### **SpeechTranscriptionService**
```typescript
transcribe(audioBlob: Blob): Observable<TranscriptionResult>
```
- Currently returns mock transcription with 800ms simulated delay
- Includes placeholder for real Whisper API integration
- Returns confidence score and timestamp

#### **InterviewService Updates**
```typescript
saveTranscript(sessionId: string, questionId: string, transcript: string): Observable<void>
```
- New method to store transcript text
- Updates existing answer or creates new one
- Persists to localStorage with session

### Component Architecture

#### **LiveComponent State**
```typescript
speechState: 'idle' | 'ai-speaking' | 'user-turn' | 'user-recording'
showCaptions: boolean
currentTranscript: string
```

#### **Key Methods**
- `speakQuestion()` - Uses SpeechSynthesis API
- `startRecording()` - Captures audio via MediaRecorder
- `stopRecording()` - Processes and transcribes audio
- `toggleCaptions()` - Show/hide question text
- `clearTranscript()` - Reset transcript for current question
- `saveTranscriptToSession()` - Persist to interview service

### Browser APIs Used

1. **SpeechSynthesis** - Text-to-speech for AI interviewer
2. **MediaRecorder** - Audio recording from microphone
3. **getUserMedia** - Microphone access
4. **SpeechSynthesisUtterance** - Configure speech parameters

---

## UI/UX Design

### Visual Elements

#### **AI Avatar**
- 120px circular gradient (`#667eea` to `#764ba2`)
- Nested core with white center and primary dot
- Pulse animation on speak
- 5-bar waveform animation below

#### **Waveform Animations**
- **AI Speaking**: 5 bars, blue color, smooth wave
- **User Recording**: 7 bars, primary color, energetic wave
- Staggered animation delays for natural flow

#### **Status Indicator**
- Below avatar in sidebar
- Color changes: gray (idle) → primary (active)
- Font weight increases when active

#### **Recording Controls**
- Large prominent "Start Answer" button (primary color)
- Large "Stop Answer" button (red with pulse animation)
- Centered with waveform below during recording

#### **Transcript Panel**
- Bordered container with scrollable content
- Empty state message when no transcript
- Clear button appears only when transcript exists
- Max height 200px with auto-scroll

### Responsive Behavior
- Sidebar stacks below on mobile (<968px)
- Controls remain accessible on all screen sizes
- Touch-friendly button sizes

---

## Control Flow

### Question Load Sequence
1. Component loads question from session
2. AI avatar animates
3. AI speaks question (300ms delay)
4. Controls disabled during speech
5. On speech end → State changes to "user-turn"
6. Recording button enabled

### Recording Flow
1. User clicks "Start Answer"
2. Browser requests microphone permission
3. MediaRecorder starts capturing
4. Waveform animation displays
5. User clicks "Stop Answer"
6. Recording stops, audio blob created
7. Transcription service processes audio (mock)
8. Transcript appends to existing text
9. Auto-saves to session

### Navigation Flow
- **Next/Previous**: Stops any speech/recording, saves transcript, loads new question, auto-speaks
- **Finish**: Saves transcript, processes session, navigates to results

---

## Future Integration Points

### Whisper API Integration
When backend speech-to-text is ready:

**Replace in `SpeechTranscriptionService`:**
```typescript
transcribe(audioBlob: Blob): Observable<TranscriptionResult> {
  const formData = new FormData();
  formData.append('audio', audioBlob, 'recording.webm');
  
  return this.http.post<TranscriptionResult>(
    `${environment.apiBaseUrl}/api/speech/transcribe`,
    formData
  );
}
```

**No component changes required** - service abstraction handles it.

### Backend Session Storage
Current: localStorage  
Future: POST to `/api/interview/:sessionId/transcript`

Replace `saveTranscript()` in `InterviewService` to call backend instead of localStorage.

---

## Accessibility

- **Keyboard navigation** supported
- **Screen reader** friendly status updates
- **Visual feedback** for all states
- **Error messages** for permission issues
- **Replay button** for users who need to re-hear
- **Captions toggle** for hearing preferences

---

## Browser Compatibility

### Required APIs
- **SpeechSynthesis** - All modern browsers (Chrome, Firefox, Edge, Safari)
- **MediaRecorder** - All modern browsers
- **getUserMedia** - All modern browsers (requires HTTPS or localhost)

### Fallbacks
- If SpeechSynthesis unavailable → Status shows "Speech not supported", controls still work
- If microphone access denied → Error message displayed, user can retry
- If MediaRecorder fails → Error message, can still use text input (future enhancement)

---

## Security & Privacy

- **Microphone access** requires user permission
- **Audio data** processed client-side (mock transcription)
- **No audio uploaded** to server in current implementation
- **Future**: Encrypt audio before upload to Whisper API

---

## Testing Considerations

### Manual Testing
1. Verify AI speaks on question load
2. Test caption toggle functionality
3. Record multiple answers and check transcript
4. Test navigation between questions
5. Verify controls disabled during AI speech
6. Test microphone permission flow
7. Verify finish interview flow

### Edge Cases
- Microphone permission denied
- SpeechSynthesis not available
- Very long transcripts (scrolling)
- Rapid navigation between questions
- Session timeout during recording

---

## Performance

- **No backend calls** during recording/transcription (mock)
- **Efficient state management** with Angular signals
- **Cleanup** on component destroy (stop media, clear timers)
- **Lazy loading** - component only loads when route accessed

---

## Known Limitations (Current Mock)

1. Transcription is **placeholder text** (not real speech-to-text)
2. No **voice matching** or speaker verification
3. No **audio quality** analysis
4. No **language detection**
5. No **custom vocabulary** or domain-specific terms

All these will be available when integrated with backend Whisper API.

---

## Summary

The upgraded interview page now provides a **professional, interactive AI interview experience** with:
- ✅ Animated AI avatar with speech synthesis
- ✅ Voice recording with waveform visualization  
- ✅ Real-time transcript management
- ✅ Caption toggle for accessibility
- ✅ Disabled controls during AI speech
- ✅ Clean state transitions
- ✅ Future-ready service abstraction
- ✅ Responsive, modern UI

**Ready for production** with mock transcription, and **ready for Whisper integration** with minimal code changes.
