# CommunicaAI Database Operations Guide

## Overview

This document provides a comprehensive guide to all database operations in the CommunicaAI system, including table schemas, seeding operations, and data retrieval patterns.

## Database Technology

- **Database**: PostgreSQL 14+
- **ORM**: Entity Framework Core 8.0
- **Migration Tool**: EF Core Migrations

## Table Schema Summary

### Core Tables

#### 1. AppUsers
Stores user authentication and profile information.

```sql
CREATE TABLE "AppUsers" (
    "Id" uuid PRIMARY KEY,
    "FullName" varchar(100) NOT NULL,
    "Email" varchar(150) NOT NULL UNIQUE,
    "PasswordHash" text NOT NULL,
    "CreatedAt" timestamp NOT NULL
);
```

#### 2. QuestionBanks
Stores interview questions organized by role, category, and difficulty.

```sql
CREATE TABLE "QuestionBanks" (
    "Id" uuid PRIMARY KEY,
    "Role" varchar(100) NOT NULL,
    "Category" varchar(50) NOT NULL,
    "Difficulty" varchar(50) NOT NULL,
    "QuestionText" varchar(1000) NOT NULL,
    "CreatedAt" timestamp NOT NULL,
    INDEX idx_role_category_difficulty ("Role", "Category", "Difficulty")
);
```

**Data Distribution**:
- **Roles**: 14 (Software Engineer, Backend Developer, Frontend Developer, Full Stack Developer, Data Scientist, Data Analyst, DevOps Engineer, Cloud Engineer, Machine Learning Engineer, Product Manager, Marketing Manager, UX Designer, Business Analyst, Sales Executive, Customer Success Manager)
- **Categories**: 3 (Technical, Behavioral, HR)
- **Difficulties**: 3 (Easy, Medium, Hard)
- **Total Questions**: ~150+

#### 3. InterviewSessions
Stores interview session metadata and configuration.

```sql
CREATE TABLE "InterviewSessions" (
    "Id" uuid PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "Role" varchar(100) NOT NULL,
    "Topic" varchar(200) NOT NULL,
    "Difficulty" varchar(50) NOT NULL,
    "Status" varchar(50) NOT NULL,
    "QuestionCount" int NOT NULL,
    "DurationMinutes" int NOT NULL,
    "StartedAt" timestamp NOT NULL,
    "CompletedAt" timestamp NULL,
    "InterviewType" varchar(100) NULL,
    "CompanyProfileId" uuid NULL,
    "ResumeProfileId" uuid NULL,
    FOREIGN KEY ("UserId") REFERENCES "AppUsers"("Id"),
    FOREIGN KEY ("CompanyProfileId") REFERENCES "CompanyProfiles"("Id"),
    FOREIGN KEY ("ResumeProfileId") REFERENCES "ResumeProfiles"("Id"),
    INDEX idx_userid ("UserId")
);
```

#### 4. InterviewQuestions
Stores questions selected for specific interview sessions.

```sql
CREATE TABLE "InterviewQuestions" (
    "Id" uuid PRIMARY KEY,
    "InterviewSessionId" uuid NOT NULL,
    "QuestionText" varchar(1000) NOT NULL,
    "Category" varchar(50) NOT NULL,
    "OrderNumber" int NOT NULL,
    "IsAnswered" boolean NOT NULL DEFAULT false,
    FOREIGN KEY ("InterviewSessionId") REFERENCES "InterviewSessions"("Id") ON DELETE CASCADE,
    INDEX idx_session ("InterviewSessionId")
);
```

#### 5. InterviewAnswers
Stores user answers and transcriptions.

```sql
CREATE TABLE "InterviewAnswers" (
    "Id" uuid PRIMARY KEY,
    "InterviewQuestionId" uuid NOT NULL UNIQUE,
    "InterviewSessionId" uuid NOT NULL,
    "Transcript" text NOT NULL,
    "AudioUrl" varchar(500) NULL,
    "DurationSeconds" int NOT NULL,
    "AnsweredAt" timestamp NOT NULL,
    FOREIGN KEY ("InterviewQuestionId") REFERENCES "InterviewQuestions"("Id") ON DELETE CASCADE,
    FOREIGN KEY ("InterviewSessionId") REFERENCES "InterviewSessions"("Id") ON DELETE CASCADE,
    INDEX idx_session ("InterviewSessionId")
);
```

#### 6. AnswerEvaluations
Stores AI-generated evaluations of user answers.

```sql
CREATE TABLE "AnswerEvaluations" (
    "Id" uuid PRIMARY KEY,
    "InterviewAnswerId" uuid NOT NULL UNIQUE,
    "TechnicalScore" decimal(5,2) NOT NULL,
    "ClarityScore" decimal(5,2) NOT NULL,
    "CompletenessScore" decimal(5,2) NOT NULL,
    "OverallScore" decimal(5,2) NOT NULL,
    "CommunicationScore" decimal(5,2) NOT NULL,
    "ConfidenceScore" decimal(5,2) NOT NULL,
    "GrammarScore" decimal(5,2) NOT NULL,
    "VocabularyScore" decimal(5,2) NOT NULL,
    "ProfessionalismScore" decimal(5,2) NOT NULL,
    "AnswerStructureScore" decimal(5,2) NOT NULL,
    "PersuasivenessScore" decimal(5,2) NOT NULL,
    "ConcisenessScore" decimal(5,2) NOT NULL,
    "Strengths" text NOT NULL,
    "Improvements" text NOT NULL,
    "Feedback" text NOT NULL,
    "EvaluatedAt" timestamp NOT NULL,
    FOREIGN KEY ("InterviewAnswerId") REFERENCES "InterviewAnswers"("Id") ON DELETE CASCADE
);
```

#### 7. InterviewResults
Stores overall interview session results.

```sql
CREATE TABLE "InterviewResults" (
    "Id" uuid PRIMARY KEY,
    "InterviewSessionId" uuid NOT NULL UNIQUE,
    "OverallScore" decimal(5,2) NOT NULL,
    "TechnicalScore" decimal(5,2) NOT NULL,
    "CommunicationScore" decimal(5,2) NOT NULL,
    "ConfidenceScore" decimal(5,2) NOT NULL,
    "Strengths" text NOT NULL,
    "Weaknesses" text NOT NULL,
    "Recommendations" text NOT NULL,
    "Summary" text NOT NULL,
    "GeneratedAt" timestamp NOT NULL,
    FOREIGN KEY ("InterviewSessionId") REFERENCES "InterviewSessions"("Id") ON DELETE CASCADE
);
```

#### 8. CompanyProfiles
Stores company-specific interview expectations and styles.

```sql
CREATE TABLE "CompanyProfiles" (
    "Id" uuid PRIMARY KEY,
    "CompanyName" varchar(200) NOT NULL,
    "InterviewStyle" text NOT NULL,
    "FocusAreas" text NOT NULL,
    "BehavioralExpectations" text NOT NULL,
    "TechnicalExpectations" text NOT NULL,
    "CommunicationExpectations" text NOT NULL,
    "CreatedAt" timestamp NOT NULL
);
```

#### 9. ResumeProfiles
Stores parsed resume data for personalized interviews.

```sql
CREATE TABLE "ResumeProfiles" (
    "Id" uuid PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "FileName" varchar(255) NOT NULL,
    "ResumeUrl" varchar(500) NOT NULL,
    "FileType" varchar(50) NOT NULL,
    "ParsedData" text NOT NULL,
    "UploadedAt" timestamp NOT NULL,
    FOREIGN KEY ("UserId") REFERENCES "AppUsers"("Id"),
    INDEX idx_userid ("UserId")
);
```

## Data Retrieval Operations

### 1. Metadata Retrieval (Roles, Difficulties, Categories)

**API Endpoint**: `GET /api/question-bank/metadata`

**SQL Query**:
```sql
-- Get distinct roles
SELECT DISTINCT "Role" FROM "QuestionBanks" ORDER BY "Role";

-- Get distinct difficulties
SELECT DISTINCT "Difficulty" FROM "QuestionBanks" ORDER BY "Difficulty";

-- Get distinct categories
SELECT DISTINCT "Category" FROM "QuestionBanks" ORDER BY "Category";
```

**Response Example**:
```json
{
  "roles": [
    "Backend Developer",
    "Business Analyst",
    "Cloud Engineer",
    "Customer Success Manager",
    "Data Analyst",
    "Data Scientist",
    "DevOps Engineer",
    "Frontend Developer",
    "Full Stack Developer",
    "Machine Learning Engineer",
    "Marketing Manager",
    "Product Manager",
    "Sales Executive",
    "Software Engineer",
    "UX Designer"
  ],
  "difficulties": ["Easy", "Hard", "Medium"],
  "categories": ["Behavioral", "HR", "Technical"]
}
```

### 2. Interview Types Retrieval

**API Endpoint**: `GET /api/interviews/types`

**Data Source**: Hardcoded in `InterviewController.cs` (not from database)

**Response Example**:
```json
{
  "interviewTypes": [
    {
      "type": "Technical",
      "displayName": "Technical",
      "description": "Focuses on technical skills, problem-solving, and domain knowledge",
      "icon": "💻",
      "focusAreas": ["Coding", "Algorithms", "System Knowledge", "Best Practices"]
    },
    {
      "type": "HR",
      "displayName": "HR",
      "description": "Assesses cultural fit, work style, and interpersonal skills",
      "icon": "👥",
      "focusAreas": ["Culture Fit", "Work Style", "Team Collaboration", "Career Goals"]
    }
    // ... 10 more types
  ]
}
```

### 3. Random Question Selection

**SQL Query Pattern**:
```sql
-- Get random questions for an interview
SELECT * FROM "QuestionBanks"
WHERE "Role" = @role 
  AND "Difficulty" = @difficulty
ORDER BY random()
LIMIT @questionCount;
```

**Implementation**: `InterviewQuestionService.GetRandomQuestionsAsync()`

### 4. User Interview History

**API Endpoint**: `GET /api/interviews/my-history`

**SQL Query**:
```sql
SELECT 
    s."Id",
    s."Role",
    s."Topic",
    s."Difficulty",
    s."Status",
    s."StartedAt",
    s."CompletedAt",
    s."InterviewType",
    r."OverallScore"
FROM "InterviewSessions" s
LEFT JOIN "InterviewResults" r ON s."Id" = r."InterviewSessionId"
WHERE s."UserId" = @userId
ORDER BY s."StartedAt" DESC;
```

### 5. Session Questions with Answers

**API Endpoint**: `GET /api/interviews/{sessionId}`

**SQL Query**:
```sql
SELECT 
    q."Id",
    q."QuestionText",
    q."Category",
    q."OrderNumber",
    q."IsAnswered",
    a."Transcript",
    a."AudioUrl",
    a."AnsweredAt"
FROM "InterviewQuestions" q
LEFT JOIN "InterviewAnswers" a ON q."Id" = a."InterviewQuestionId"
WHERE q."InterviewSessionId" = @sessionId
ORDER BY q."OrderNumber";
```

## Database Seeding

### Method 1: SQL Scripts (Recommended for Production)

```bash
# Navigate to seed directory
cd CommunicaAI/Database/Seeds

# Run master seed script
psql -h localhost -U postgres -d communicaai -f 00_master_seed.sql

# Or run individual seed scripts
psql -h localhost -U postgres -d communicaai -f 01_seed_question_bank.sql
```

### Method 2: API Endpoint (Development Only)

```bash
# Seed questions via API
POST http://localhost:5000/api/question-bank/seed
```

**Notes**:
- API seeding only works if `QuestionBanks` table is empty
- Checks for existing data before inserting
- Uses hardcoded seed data from `QuestionBankService.GetSeedQuestions()`

## Migration Management

### Create New Migration

```bash
cd CommunicaAI
dotnet ef migrations add MigrationName
```

### Apply Migrations

```bash
# Local development
dotnet ef database update

# Production (AWS EC2)
dotnet ef database update --connection "Host=localhost;Database=communicaai;Username=postgres;Password=your_password"
```

### Rollback Migration

```bash
# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Rollback all migrations
dotnet ef database update 0
```

## Database Backup and Restore

### Backup

```bash
# Full database backup
pg_dump -h localhost -U postgres -d communicaai -F c -f communicaai_backup.dump

# Backup specific table
pg_dump -h localhost -U postgres -d communicaai -t "QuestionBanks" -F c -f questionbanks_backup.dump
```

### Restore

```bash
# Restore full database
pg_restore -h localhost -U postgres -d communicaai -c communicaai_backup.dump

# Restore specific table
pg_restore -h localhost -U postgres -d communicaai -t "QuestionBanks" questionbanks_backup.dump
```

## Performance Optimization

### Recommended Indexes

```sql
-- QuestionBanks table
CREATE INDEX idx_questionbanks_role_difficulty ON "QuestionBanks"("Role", "Difficulty");
CREATE INDEX idx_questionbanks_role_category_difficulty ON "QuestionBanks"("Role", "Category", "Difficulty");

-- InterviewSessions table
CREATE INDEX idx_interviewsessions_userid_status ON "InterviewSessions"("UserId", "Status");
CREATE INDEX idx_interviewsessions_startedat ON "InterviewSessions"("StartedAt" DESC);

-- InterviewQuestions table
CREATE INDEX idx_interviewquestions_sessionid_order ON "InterviewQuestions"("InterviewSessionId", "OrderNumber");

-- InterviewAnswers table
CREATE INDEX idx_interviewanswers_sessionid_answeredat ON "InterviewAnswers"("InterviewSessionId", "AnsweredAt");
```

### Query Optimization Tips

1. **Use EXPLAIN ANALYZE** to check query performance
2. **Add indexes** on frequently queried columns
3. **Use pagination** for large result sets
4. **Avoid N+1 queries** by using `.Include()` in EF Core
5. **Use compiled queries** for frequently executed queries

## Common Database Operations

### Check Data Integrity

```sql
-- Verify all sessions have corresponding questions
SELECT s."Id" 
FROM "InterviewSessions" s
LEFT JOIN "InterviewQuestions" q ON s."Id" = q."InterviewSessionId"
WHERE q."Id" IS NULL;

-- Find orphaned answers (without sessions)
SELECT a."Id"
FROM "InterviewAnswers" a
LEFT JOIN "InterviewSessions" s ON a."InterviewSessionId" = s."Id"
WHERE s."Id" IS NULL;
```

### Clean Up Old Data

```sql
-- Delete sessions older than 1 year
DELETE FROM "InterviewSessions"
WHERE "StartedAt" < NOW() - INTERVAL '1 year';

-- Archive completed interviews (before deleting)
INSERT INTO "ArchivedInterviewSessions"
SELECT * FROM "InterviewSessions"
WHERE "Status" = 'Completed' AND "CompletedAt" < NOW() - INTERVAL '6 months';
```

## Connection Strings

### Development
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=communicaai;Username=postgres;Password=your_password"
}
```

### Production (AWS RDS)
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=your-rds-endpoint.region.rds.amazonaws.com;Database=communicaai;Username=admin;Password=your_secure_password;SSL Mode=Require"
}
```

## Troubleshooting

### Issue: Cannot connect to database
**Solution**: Check PostgreSQL service status and connection string

```bash
# Check PostgreSQL status
sudo systemctl status postgresql

# Test connection
psql -h localhost -U postgres -d communicaai
```

### Issue: Migration fails
**Solution**: Check for pending migrations and database state

```bash
# List all migrations
dotnet ef migrations list

# Check if database exists
psql -h localhost -U postgres -l | grep communicaai
```

### Issue: Slow query performance
**Solution**: Analyze and optimize queries

```sql
-- Enable timing
\timing

-- Analyze query
EXPLAIN ANALYZE SELECT * FROM "QuestionBanks" WHERE "Role" = 'Software Engineer';

-- Check table statistics
SELECT * FROM pg_stat_user_tables WHERE schemaname = 'public';
```

## Security Best Practices

1. **Never commit connection strings** with passwords to version control
2. **Use environment variables** for sensitive configuration
3. **Enable SSL** for production database connections
4. **Restrict database user permissions** (principle of least privilege)
5. **Regularly backup** production databases
6. **Use parameterized queries** to prevent SQL injection (EF Core does this by default)
7. **Encrypt sensitive data** at rest and in transit

## Monitoring

### Check Database Size

```sql
SELECT pg_size_pretty(pg_database_size('communicaai'));
```

### Check Table Sizes

```sql
SELECT 
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;
```

### Active Connections

```sql
SELECT count(*) FROM pg_stat_activity WHERE datname = 'communicaai';
```
