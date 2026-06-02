# Communica AI - Interview Flow Implementation

## Overview
Complete Angular frontend for the Communica AI mock interview platform with authentication and full interview lifecycle management.

## Architecture

### Backend Integration
- **Real Backend:** Authentication (`/api/auth/register`, `/api/auth/login/password`, `/api/auth/me`)
- **Frontend Mock:** Interview setup, live session, results, and history (using localStorage)

### Core Services

#### 1. **AuthService** (`core/services/auth.service.ts`)
- `register(formData)` - Register with video/audio
- `loginPassword(payload)` - Password-based login
- `loginAudio(formData)` - Audio verification login
- `loginVideo(formData)` - Video verification login
- `me()` - Get current user profile
- `logout()` - Clear token and logout
- `getToken()` - Retrieve JWT token
- `isLoggedIn()` - Check auth status

#### 2. **InterviewService** (`core/services/interview.service.ts`)
- `createSession(setup)` - Create new interview session
- `getCurrentSession()` - Get active session from localStorage
- `saveAnswer(sessionId, answer)` - Save user answer
- `updateQuestionIndex(sessionId, index)` - Navigate questions
- `finishSession(sessionId)` - Complete interview and compute results

**Mock Question Bank:**
- Software Engineer
- Product Manager
- Data Scientist
- Marketing Manager

#### 3. **InterviewHistoryService** (`core/services/interview-history.service.ts`)
- `listSessions()` - Get all completed interviews
- `getSessionById(id)` - Get specific interview result
- `saveSession(result)` - Save completed interview
- `getStats()` - Get user statistics (total, avg score, streak)

### Routes

| Path | Component | Guard | Description |
|------|-----------|-------|-------------|
| `/` | - | - | Redirects to `/login` |
| `/login` | LoginComponent | Public | Login with password/audio/video |
| `/register` | RegisterComponent | Public | Register with video/audio capture |
| `/dashboard` | DashboardComponent | Protected | Main dashboard with stats |
| `/interview/setup` | SetupComponent | Protected | Configure interview settings |
| `/interview/live/:sessionId` | LiveComponent | Protected | Live interview session |
| `/interview/result/:sessionId` | ResultComponent | Protected | View interview results |
| `/history` | HistoryComponent | Protected | All past interviews |
| `/**` | - | - | Redirects to `/login` |

## Feature Components

### 1. Dashboard (`features/dashboard`)
**Displays:**
- Welcome message with user's name
- "Start Interview" CTA button
- Stats cards: Total interviews, Average score, Current streak
- Recent sessions preview (last 3)
- Empty state if no interviews

**Navigation:**
- Click "Start Interview" → `/interview/setup`
- Click session card → `/interview/result/:sessionId`
- Click "View all" → `/history`

### 2. Interview Setup (`features/interview/setup`)
**Form Fields:**
- Role/Job Title (dropdown with 8 predefined roles)
- Interview Topic (text input)
- Difficulty Level (easy/medium/hard)
- Duration (5-60 minutes)
- Question Count (1-20 questions)

**Actions:**
- Submit → Creates session → Navigate to `/interview/live/:sessionId`
- Back → Return to dashboard

### 3. Live Interview (`features/interview/live`)
**Layout:**
- Sidebar: AI Interviewer avatar, session info
- Main area: Question card, answer textarea, navigation

**Features:**
- Question progress indicator (e.g., "Question 2 of 5")
- Countdown timer (auto-finish when time expires)
- Answer input with auto-save
- Previous/Next buttons
- Finish button on last question
- Recording UI placeholder (visual only, no actual recording)

**Navigation:**
- Previous/Next → Navigate between questions
- Finish → Save session → Navigate to `/interview/result/:sessionId`

### 4. Result Page (`features/interview/result`)
**Displays:**
- Overall score (large circle with color-coded border)
- Score breakdown: Communication, Confidence (with progress bars)
- Strengths list
- Improvements list
- Full interview transcript
- Copy transcript button

**Actions:**
- "Start New Interview" → `/interview/setup`
- "View All Sessions" → `/history`
- "Copy Transcript" → Copies to clipboard

### 5. History Page (`features/interview/history`)
**Displays:**
- Grid of all completed interview cards
- Each card shows:
  - Role, Score badge
  - Topic, Difficulty, Question count
  - Completion date/time
- Empty state if no interviews

**Actions:**
- Click card → View result page

## Data Models

### InterviewSetup
```typescript
{
  role: string;
  topic: string;
  difficulty: 'easy' | 'medium' | 'hard';
  duration: number;
  questionCount: number;
}
```

### InterviewSession
```typescript
{
  id: string;
  setup: InterviewSetup;
  questions: InterviewQuestion[];
  answers: InterviewAnswer[];
  status: 'draft' | 'in-progress' | 'completed';
  createdAt: Date;
  completedAt?: Date;
  currentQuestionIndex: number;
}
```

### InterviewResult
```typescript
{
  sessionId: string;
  overallScore: number;
  communicationScore: number;
  confidenceScore: number;
  strengths: string[];
  improvements: string[];
  transcript: string;
  setup: InterviewSetup;
  completedAt: Date;
}
```

## Storage

### LocalStorage Keys
- `"token"` - JWT authentication token
- `"communica_current_session"` - Active interview session
- `"communica_interview_history"` - Array of completed interviews

## Scoring Logic (Mock)

### Overall Score
- 50% completion rate (answered questions / total questions)
- 50% answer quality (average answer length normalized to 500 chars)

### Communication Score
Based on average answer length (normalized to 400 chars max = 100%)

### Confidence Score
- 70% completion rate
- 30% random variance

### Strengths & Improvements
- Score ≥80%: 4-5 strengths, 1 improvement
- Score 60-79%: 3 strengths, 2 improvements
- Score <60%: 2 strengths, 3 improvements

## UI Design

### Design Tokens
```css
--primary: #6c47ff
--primary-hover: #5a38e0
--bg: #f5f5f7
--surface: #ffffff
--border: #e2e2e7
--text: #111118
--text-muted: #6b6b80
--error: #dc2626
--radius: 8px
```

### Visual Style
- Minimal, modern card-based layout
- Soft shadows on hover
- Rounded corners (8px)
- Clear typography hierarchy
- Responsive grid layouts
- SVG icons throughout
- Color-coded score badges:
  - Green: ≥80%
  - Yellow: 60-79%
  - Red: <60%

## Future Backend Integration

When backend interview endpoints are ready:

1. **Update InterviewService:**
   - Replace mock `createSession()` with POST `/api/interview/create`
   - Replace `finishSession()` with POST `/api/interview/complete`
   - Add GET `/api/interview/:id` for session retrieval

2. **Update InterviewHistoryService:**
   - Replace localStorage with GET `/api/interview/history`
   - Replace mock stats with GET `/api/interview/stats`

3. **No Component Changes Required**
   - Services abstract data layer from UI
   - Components consume Observables agnostically

## Development Commands

```bash
# Install dependencies
npm install

# Run dev server
ng serve

# Build for production
ng build

# Run tests
ng test
```

## Browser Support
- Chrome/Edge (latest)
- Firefox (latest)
- Safari (latest)

## Notes
- Audio/video recording UI is visual placeholder only
- All interview data stored in browser localStorage
- No external AI integration yet
- SSR-safe with `isPlatformBrowser` checks
- Auth guard protects all interview routes
- Token auto-attached via HTTP interceptor
