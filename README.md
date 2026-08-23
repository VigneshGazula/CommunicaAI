# CommunicaAI

**AI-Powered Interview Practice & Evaluation Platform**

CommunicaAI is a comprehensive interview preparation platform that uses artificial intelligence to help candidates practice, improve, and succeed in their job interviews. The platform provides realistic interview simulations, instant feedback, and detailed performance analytics.

---

## Features

### 🎯 Interview Practice
- **12 Specialized Interview Types**: Technical, HR, Behavioral, Coding, System Design, DevOps, Cloud, Data Science, AI/ML, Cyber Security, Product Manager, Solution Architect
- **Dynamic Question Generation**: AI-generated questions tailored to role and difficulty level
- **Audio Recording & Transcription**: Practice speaking your answers naturally with automatic transcription
- **Real-time Progress Tracking**: Monitor your performance throughout the interview

### 📊 Performance Analytics
- **Comprehensive Scoring**: Technical skills, communication, confidence, grammar, professionalism
- **Detailed Feedback**: AI-powered evaluation with specific strengths and improvement areas
- **Historical Tracking**: Review past interviews and track progress over time
- **Visual Reports**: Interactive charts and graphs showing performance trends

### 🤖 AI-Powered Intelligence
- **Smart Question Selection**: Questions matched to your skill level and role
- **Answer Evaluation**: Advanced AI analysis of your responses
- **Personalized Coaching**: Custom recommendations based on your performance
- **Company Intelligence**: Role-specific preparation when company profile is provided
- **Resume Matching**: Skill gap analysis and career recommendations

### 🎨 Professional UI
- **Modern Design**: Clean, Squarespace-inspired interface
- **Responsive Layout**: Works seamlessly on desktop and mobile
- **Color Theme**: Professional Indigo & Emerald palette
- **Intuitive Navigation**: Easy-to-use interface for all experience levels

---

## Technology Stack

### Backend (.NET Core)
- **Framework**: ASP.NET Core 8.0
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **Cloud Storage**: Cloudinary (audio files)
- **AI Services**: Google Gemini API (transcription & evaluation)

### Frontend (Angular)
- **Framework**: Angular 19
- **Styling**: SCSS with custom design system
- **Charts**: Chart.js
- **State Management**: RxJS
- **Build Tool**: Vite

### DevOps
- **Version Control**: Git/GitHub
- **Backend Hosting**: Render.com
- **Frontend Hosting**: Render.com
- **Database Hosting**: Render PostgreSQL
- **CI/CD**: GitHub Actions (optional)

---

## Getting Started

### Prerequisites
- **Backend**:
  - .NET 8.0 SDK
  - PostgreSQL 14+
  - Gemini API Key
  - Cloudinary Account

- **Frontend**:
  - Node.js 18+
  - npm or yarn

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/VigneshGazula/CommunicaAI.git
   cd CommunicaAI/CommunicaAI
   ```

2. **Configure appsettings.json**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=CommunicaAIDB;Username=postgres;Password=yourpassword"
     },
     "Jwt": {
       "Issuer": "CommunicaAI",
       "Audience": "CommunicaAIUsers",
       "Key": "your-secure-secret-key-min-32-characters"
     },
     "CloudinarySettings": {
       "CloudName": "your-cloud-name",
       "ApiKey": "your-api-key",
       "ApiSecret": "your-api-secret"
     },
     "Gemini": {
       "ApiKey": "your-gemini-api-key",
       "Model": "gemini-2.0-flash-exp"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the backend**
   ```bash
   dotnet run
   ```
   Backend will start at `http://localhost:5169`

### Frontend Setup

1. **Navigate to frontend directory**
   ```bash
   cd ../Frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Configure environment**
   
   Update `src/environments/environment.ts`:
   ```typescript
   export const environment = {
     production: false,
     apiBaseUrl: 'http://localhost:5169'
   };
   ```

4. **Run the frontend**
   ```bash
   npm start
   ```
   Frontend will start at `http://localhost:4200`

---

## Project Structure

```
CommunicaAI/
├── CommunicaAI/                    # Backend (.NET)
│   ├── Controllers/                # API endpoints
│   ├── Services/                   # Business logic
│   ├── Repositories/               # Data access
│   ├── Models/                     # Database entities
│   ├── DTO/                        # Data transfer objects
│   ├── Configurations/             # App settings
│   ├── Data/                       # DbContext
│   └── Migrations/                 # EF migrations
│
├── Frontend/                       # Frontend (Angular)
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/              # Services, guards, interceptors
│   │   │   ├── features/          # Feature modules
│   │   │   │   ├── auth/          # Authentication
│   │   │   │   ├── dashboard/     # Main dashboard
│   │   │   │   └── interview/     # Interview features
│   │   │   ├── shared/            # Shared components
│   │   │   └── app.component.*    # Root component
│   │   ├── assets/                # Static files
│   │   ├── environments/          # Environment configs
│   │   └── styles.scss            # Global styles
│   └── package.json
│
└── README.md                       # This file
```

---

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login/password` - Login with password
- `POST /api/auth/login/guest` - Guest login
- `GET /api/auth/me` - Get current user

### Interviews
- `POST /api/interviews` - Start new interview
- `GET /api/interviews` - Get user's interviews
- `GET /api/interviews/{id}` - Get interview details
- `POST /api/interviews/{id}/complete` - Complete interview
- `DELETE /api/interviews/{id}` - Delete interview

### Questions
- `GET /api/interviews/{sessionId}/questions` - Get interview questions

### Answers
- `POST /api/interviews/{sessionId}/answers/audio` - Submit audio answer

### Results
- `GET /api/interviews/{sessionId}/result` - Get interview results

---

## Configuration

### Gemini API
1. Get API key from [Google AI Studio](https://aistudio.google.com/app/apikey)
2. Free tier: 15 requests/minute, 1M tokens/minute
3. Recommended model: `gemini-2.0-flash-exp`

### Cloudinary
1. Sign up at [Cloudinary](https://cloudinary.com/)
2. Get Cloud Name, API Key, API Secret from dashboard
3. Used for audio file storage

### PostgreSQL
1. Local: Install PostgreSQL 14+
2. Production: Use managed service (Render, AWS RDS, etc.)
3. Create database: `CREATE DATABASE CommunicaAIDB;`

---

## Deployment

### Backend (Render.com)

1. **Create Web Service**
   - Build Command: `dotnet publish -c Release -o out`
   - Start Command: `cd out && dotnet CommunicaAI.dll`

2. **Environment Variables**
   ```
   DATABASE_URL=postgresql://...
   GEMINI_API_KEY=your-key
   GEMINI_MODEL=gemini-2.0-flash-exp
   CLOUDINARY_CLOUD_NAME=your-cloud
   CLOUDINARY_API_KEY=your-key
   CLOUDINARY_API_SECRET=your-secret
   JWT_KEY=your-secret-key
   JWT_ISSUER=CommunicaAI
   JWT_AUDIENCE=CommunicaAIUsers
   FRONTEND_ORIGINS=https://your-frontend.onrender.com
   ```

### Frontend (Render.com)

1. **Create Static Site**
   - Build Command: `npm install && npm run build`
   - Publish Directory: `dist/frontend/browser`

2. **Environment Variables**
   ```
   API_BASE_URL=https://your-backend.onrender.com
   ```

---

## Usage Guide

### Starting an Interview

1. **Login or Register**
   - Use email/password or guest login

2. **Create Interview**
   - Select interview type (e.g., Technical, HR)
   - Choose difficulty (Easy, Medium, Hard)
   - Set number of questions (5-15)
   - Add job role (e.g., Software Engineer)

3. **Answer Questions**
   - Click "Record" to start recording
   - Speak your answer clearly
   - Click "Stop" when finished
   - Review transcription and submit

4. **Complete Interview**
   - Answer all questions
   - Click "Complete Interview"
   - Wait for AI evaluation (~15-20 seconds)

5. **View Results**
   - Review overall scores
   - Read detailed feedback
   - Check individual question performance
   - Download report (optional)

---

## Features in Detail

### Interview Types

1. **Technical** - Core technical knowledge
2. **HR** - Cultural fit and soft skills
3. **Behavioral** - Past experiences and situations
4. **Coding** - Algorithm and data structure problems
5. **System Design** - Architecture and scalability
6. **DevOps** - CI/CD, infrastructure, automation
7. **Cloud** - AWS, Azure, GCP expertise
8. **Data Science** - Statistics, ML, data analysis
9. **AI/ML** - Machine learning and AI concepts
10. **Cyber Security** - Security principles and practices
11. **Product Manager** - Product strategy and management
12. **Solution Architect** - Enterprise architecture

### Scoring Categories

- **Technical Score** - Domain knowledge accuracy
- **Communication Score** - Clarity and articulation
- **Confidence Score** - Speaking confidence and tone
- **Grammar Score** - Language correctness
- **Vocabulary Score** - Word choice and expression
- **Professionalism Score** - Professional demeanor
- **Overall Score** - Weighted average of all scores

---

## Troubleshooting

### Backend Issues

**Database Connection Failed**
```bash
# Check PostgreSQL is running
sudo systemctl status postgresql

# Verify connection string
# Ensure host, port, database, username, password are correct
```

**Migration Errors**
```bash
# Reset database (WARNING: deletes all data)
dotnet ef database drop
dotnet ef database update
```

**Gemini API Errors**
- Check API key is valid
- Verify quota not exceeded (15 RPM on free tier)
- Ensure correct model name

### Frontend Issues

**CORS Errors**
- Add frontend URL to backend CORS configuration
- Check `FRONTEND_ORIGINS` environment variable

**API Connection Failed**
- Verify `apiBaseUrl` in environment files
- Check backend is running
- Test API with curl or Postman

---

## Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open Pull Request

---

## License

This project is private and proprietary.

---

## Support

For issues, questions, or suggestions:
- Create an issue on GitHub
- Email: support@communicaai.com

---

## Acknowledgments

- **Google Gemini AI** - Audio transcription and answer evaluation
- **Cloudinary** - Audio file storage and delivery
- **Render** - Hosting platform
- **Angular** - Frontend framework
- **ASP.NET Core** - Backend framework

---

## Version History

- **v1.0.0** - Initial release with core interview features
- **v2.0.0** - Added 12 specialized interview types, improved AI evaluation
- **v2.1.0** - UI overhaul with professional design system
- **v2.2.0** - Rate limit handling and transcription improvements

---

**Built with ❤️ by the CommunicaAI Team**
