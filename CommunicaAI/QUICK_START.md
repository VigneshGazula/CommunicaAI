# 🚀 CommunicaAI Version 2 - Quick Start Guide

## Prerequisites
- ✅ .NET 8 SDK
- ✅ Node.js 18+
- ✅ SQL Server 2019+
- ✅ Python 3.9+ (optional - for video intelligence)
- ✅ Gemini API Key

---

## 1️⃣ Backend Setup (5 minutes)

### Step 1: Configure Settings
Edit `appsettings.json`:

```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CommunicaAI;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Step 2: Apply Database Migrations
```bash
cd CommunicaAI
dotnet ef database update
```

### Step 3: Run Backend
```bash
dotnet run
```

✅ Backend running on `https://localhost:5001`

---

## 2️⃣ Frontend Setup (3 minutes)

### Step 1: Install Dependencies
```bash
cd Frontend
npm install
```

### Step 2: Run Frontend
```bash
npm start
```

✅ Frontend running on `http://localhost:4200`

---

## 3️⃣ Test the Application (2 minutes)

1. Open browser to `http://localhost:4200`
2. Register new account
3. Login
4. Click "Start New Interview"
5. Select role, difficulty, question count
6. Answer questions with microphone
7. Watch real-time analytics
8. Finish interview
9. View results with AI coaching

---

## 🎥 Optional: Video Intelligence (Module 4)

### Step 1: Setup Python Environment
```bash
cd CommunicaAI/VideoAnalysisService
python -m venv venv
venv\Scripts\activate  # Windows
pip install -r requirements.txt
```

### Step 2: Run Python Service
```bash
python main.py
```

✅ Python service running on `http://localhost:8000`

---

## 📚 Documentation

- **Complete Overview**: `VERSION_2_COMPLETE_SUMMARY.md`
- **Build Status**: `BUILD_STATUS_REPORT.md`
- **Module 5 Details**: `MODULE_5_IMPLEMENTATION_SUMMARY.md`
- **Architecture**: `COMPLETE_ARCHITECTURE_REFERENCE.md`

---

## 🆘 Troubleshooting

### Backend won't start
- Check SQL Server is running
- Verify connection string in appsettings.json
- Run `dotnet ef database update`

### Frontend won't compile
- Delete `node_modules` and run `npm install`
- Check Node.js version (18+ required)

### Audio recording fails
- Use HTTPS in production (Web Speech API requirement)
- Check browser permissions for microphone

### Gemini API errors
- Verify API key is correct
- Check rate limits (retry logic handles 429 errors)

---

## ✅ Success Indicators

- ✅ Backend console shows "Now listening on: https://localhost:5001"
- ✅ Frontend shows login page
- ✅ Database has tables (check with SSMS)
- ✅ Can register and login
- ✅ Can start interview and see questions
- ✅ Analytics panel displays during recording
- ✅ Results show all scores and coaching

---

*Version 2.0.0 - All 5 Modules Operational*
