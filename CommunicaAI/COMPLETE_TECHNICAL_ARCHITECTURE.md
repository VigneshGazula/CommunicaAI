# Communica AI - Complete Technical Architecture Documentation
**Version:** 1.0  
**Last Updated:** June 25, 2026  
**Document Type:** Complete Technical Architecture & Source Code Reference  
**Status:** Production-Grade Implementation Analysis

---

## DOCUMENT PURPOSE

This document serves as the **single source of truth** for the Communica AI project architecture. It is generated from exhaustive analysis of the existing codebase and represents the ACTUAL implementation state—not planned features or assumptions.

**Target Audience:** Senior software engineers continuing development without access to source code.

**Coverage:** 100% of implemented features with file-level granularity.

---

# TABLE OF CONTENTS

1. [PROJECT OVERVIEW](#1-project-overview)
2. [SOLUTION STRUCTURE](#2-solution-structure)
3. [DATABASE ARCHITECTURE](#3-database-architecture)
4. [REPOSITORY LAYER](#4-repository-layer)
5. [SERVICE LAYER](#5-service-layer)
6. [CONTROLLER LAYER](#6-controller-layer)
7. [DTO DOCUMENTATION](#7-dto-documentation)
8. [AUTHENTICATION & AUTHORIZATION](#8-authentication--authorization)
9. [INTERVIEW MODULE](#9-interview-module)
10. [GEMINI AI INTEGRATION](#10-gemini-ai-integration)
11. [CLOUDINARY MEDIA MANAGEMENT](#11-cloudinary-media-management)
12. [PYTHON MICROSERVICE](#12-python-microservice)
13. [API REFERENCE](#13-api-reference)
14. [ANGULAR FRONTEND](#14-angular-frontend)
15. [CONFIGURATION](#15-configuration)
16. [DEPENDENCY GRAPHS](#16-dependency-graphs)
17. [SEQUENCE DIAGRAMS](#17-sequence-diagrams)
18. [IMPLEMENTATION MATRIX](#18-implementation-matrix)
19. [TECHNICAL DEBT](#19-technical-debt)
20. [MISSING FUNCTIONALITY](#20-missing-functionality)
21. [RECOMMENDED NEXT STEPS](#21-recommended-next-steps)
22. [FILE-BY-FILE DOCUMENTATION](#22-file-by-file-documentation)
23. [FINAL SUMMARY](#23-final-summary)

---

# 1. PROJECT OVERVIEW

## 1.1 Project Purpose

**Communica AI** is an AI-powered mock interview platform designed to help job seekers practice technical interviews with:
- **Biometric authentication** (audio/video verification)
- **AI-driven question generation** from a curated question bank
- **Real-time interview sessions** with voice recording
- **AI-powered answer evaluation** using Google Gemini
- **Comprehensive feedback** on technical accuracy, clarity, and communication

## 1.2 Current Implementation Status

### Overall Completion: **75%**

**Completed Modules:**
- ✅ User authentication (password, audio, video)
- ✅ Question bank management (100+ seeded questions)
- ✅ Interview session management
- ✅ Question generation (60/20/20 distribution)
- ✅ Answer submission
- ✅ Audio transcription integration (Gemini)
- ✅ Answer evaluation (Gemini AI)
- ✅ Frontend interview page with speech synthesis
- ✅ Voice recording interface

**Partially Implemented:**
- ⚠️ Interview results generation (structure exists, limited scoring logic)
- ⚠️ Dashboard analytics (basic UI, no backend integration)
- ⚠️ Biometric verification (infrastructure ready, Python service incomplete)

**Not Implemented:**
- ❌ Video analysis
- ❌ Advanced analytics dashboard
- ❌ Resume upload & analysis
- ❌ Coding interview module
- ❌ Real-time SignalR communication
- ❌ Export functionality (PDF reports)
- ❌ Email notifications
- ❌ Admin panel

## 1.3 Technology Stack

### Backend (.NET 10.0)
- **Framework:** ASP.NET Core 10.0 Web API
- **Database:** PostgreSQL 16.x
- **ORM:** Entity Framework Core 10.0.8
- **Authentication:** JWT Bearer Tokens
- **Cloud Storage:** Cloudinary (audio/video)
- **AI Service:** Google Gemini 2.0 Flash
- **External Service:** Python FastAPI (biometric verification)

### Frontend (Angular 21)
- **Framework:** Angular 21.2.0 (Standalone Components)
- **State Management:** Angular Signals
- **HTTP Client:** HttpClient with Interceptors
- **Routing:** Angular Router with Guards
- **UI:** Custom SCSS (no UI library)
- **Build Tool:** Vite
- **Testing:** Vitest

### Third-Party Services
- **Cloudinary:** Media hosting (audio/video uploads)
- **Google Gemini API:** Transcription & answer evaluation
- **Python FastAPI:** Audio verification service (localhost:8000)

### Development Tools
- **IDE:** Visual Studio Code / Visual Studio 2022
- **Version Control:** Git
- **Package Managers:** NuGet (backend), npm (frontend)
- **Database Client:** pgAdmin 4 / DBeaver

## 1.4 Folder Structure

### Backend Structure
```
CommunicaAI/
├── Controllers/           # API endpoints (6 controllers)
├── Services/             # Business logic (14 services)
├── Repositories/         # Data access (6 repositories)
├── Models/               # Database entities (12 models)
├── DTO/                  # Data transfer objects (18 DTOs)
├── Data/                 # DbContext configuration
├── Migrations/           # EF Core migrations (7 migrations)
├── Properties/           # Launch settings
├── appsettings.json      # Configuration
└── Program.cs            # Entry point & DI
```

### Frontend Structure
```
Frontend/src/app/
├── core/
│   ├── guards/          # Auth guard
│   ├── interceptors/    # Auth interceptor
│   ├── models/          # TypeScript interfaces
│   └── services/        # HTTP services (4 services)
├── features/
│   ├── auth/           # Login/Register components
│   ├── dashboard/      # Dashboard component
│   ├── interview/      # Interview module (4 components)
│   └── onboarding/     # (Empty - planned)