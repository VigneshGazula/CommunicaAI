# COMMUNICA AI - COMPLETE TECHNICAL ARCHITECTURE
## Single Source of Truth Documentation

**Generated:** June 25, 2026  
**Version:** 1.0  
**Total Source Files:** 133 (78 Backend + 55 Frontend)  
**Documentation Type:** Exhaustive Code-Based Architecture Reference

---

## EXECUTIVE SUMMARY

### Project Identity
- **Name:** Communica AI
- **Type:** AI-Powered Mock Interview Platform
- **Architecture:** ASP.NET Core 10 + Angular 21 + PostgreSQL
- **Status:** 75% Complete, Production-Grade Quality
- **Primary Features:** Biometric Auth, AI Question Generation, Voice Interviews, AI Evaluation

### Technology Foundation
- **.NET:** 10.0 (Latest LTS)
- **Angular:** 21.2.0 (Standalone Components)
- **Database:** PostgreSQL (Npgsql 10.0.1)
- **AI:** Google Gemini 2.0 Flash
- **Storage:** Cloudinary
- **Auth:** JWT Bearer Tokens

---

# SECTION 1: PROJECT OVERVIEW & ARCHITECTURE

## 1.1 Purpose & Scope

Communica AI enables job seekers to practice interviews through:

1. **Biometric Authentication**
   - Password login (standard)
   - Audio verification (voice biometrics)
   - Video verification (facial recognition)
   
2. **Interview Practice**
   - Role-specific question generation
   - Real-time voice recording
   - AI speech-to-text transcription
   
3. **AI Evaluation**
   - Answer quality scoring
   - Technical accuracy assessment
   - Communication feedback

## 1.2 Architectural Style

**Pattern:** Clean Architecture + Repository Pattern + Service Layer


```
┌─────────────────────────────────────────────────┐
│          PRESENTATION LAYER                     │
│  Controllers (6) - API Endpoints & Validation  │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────┐
│          BUSINESS LOGIC LAYER                   │
│  Services (14) - Domain Logic & Orchestration  │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────┐
│          DATA ACCESS LAYER                      │
│  Repositories (6) - DB Queries & Operations    │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────┐
│          PERSISTENCE LAYER                      │
│  Entity Framework Core + PostgreSQL             │
└─────────────────────────────────────────────────┘
```

**Cross-Cutting Concerns:**
- JWT Authentication (Middleware)
- CORS Policy (Angular Origins)
- Exception Handling (Global)
- Validation (Data Annotations)

## 1.3 External Dependencies

### AI Services
- **Google Gemini 2.0 Flash**
  - Endpoint: `https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp`
  - Uses: Transcription, Answer Evaluation
  - API Key: Configured in appsettings.json
  - Rate Limit: Not implemented

### Cloud Storage
- **Cloudinary**
  - Audio Storage: `/audio` folder
  - Video Storage: `/video` folder
  - Configuration: CloudName, ApiKey, ApiSecret

### Python Microservice
- **FastAPI Service** (PLANNED, Not Fully Implemented)
  - Base URL: `http://127.0.0.1:8000`
  - Endpoint: `/verify-audio`
  - Purpose: Voice biometric verification
  - Status: Service interface exists, implementation incomplete

---

# SECTION 2: DATABASE ARCHITECTURE

## 2.1 Entity Relationship Diagram

```mermaid
erDiagram
    AppUser ||--o| UserMediaProfile : has
    AppUser ||--o| UserVerificationProfile : has
    AppUser ||--o{ InterviewSession : conducts
    
    InterviewSession ||--o{ InterviewQuestion : contains
    InterviewSession ||--o{ InterviewAnswer : has
    InterviewSession ||--o| InterviewResult : generates
    
