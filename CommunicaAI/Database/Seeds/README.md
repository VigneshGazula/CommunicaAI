# Database Seed Files

This directory contains PostgreSQL seed scripts for the CommunicaAI database.

## Files

### 01_seed_question_bank.sql
Seeds the `QuestionBanks` table with 150+ interview questions across:
- **14 Roles**: Software Engineer, Backend Developer, Frontend Developer, Full Stack Developer, Data Scientist, Data Analyst, DevOps Engineer, Cloud Engineer, Machine Learning Engineer, Product Manager, Marketing Manager, UX Designer, Business Analyst, Sales Executive, Customer Success Manager
- **3 Categories**: Technical, Behavioral, HR
- **3 Difficulty Levels**: Easy, Medium, Hard

## Prerequisites

1. PostgreSQL database must be created
2. EF Core migrations must be applied (creates table schema)
3. Database connection string configured in `appsettings.json`

## How to Run Seed Scripts

### Option 1: Using psql Command Line

```bash
# Connect to your database
psql -h localhost -U your_username -d communicaai

# Run the seed script
\i 'C:/Users/gazul/OneDrive/Desktop/Projects/CommunicaAI/CommunicaAI/Database/Seeds/01_seed_question_bank.sql'

# Or on Windows with absolute path
\i 'C:\\Users\\gazul\\OneDrive\\Desktop\\Projects\\CommunicaAI\\CommunicaAI\\Database\\Seeds\\01_seed_question_bank.sql'
```

### Option 2: Using pgAdmin

1. Open pgAdmin and connect to your database
2. Right-click on your database → Query Tool
3. Open the SQL file: File → Open → Select `01_seed_question_bank.sql`
4. Click Execute (F5)

### Option 3: Using DBeaver

1. Connect to your PostgreSQL database
2. Open SQL Editor (SQL Editor → New SQL Editor)
3. Load the SQL file: File → Open → Select `01_seed_question_bank.sql`
4. Execute the script (Ctrl+Enter or Execute button)

### Option 4: Using API Endpoint (Alternative)

Instead of running SQL scripts, you can use the built-in seed endpoint:

```bash
POST http://localhost:5000/api/question-bank/seed
```

**Note**: This endpoint only works if the `QuestionBanks` table is empty.

## Verification

After running the seed script, verify the data was inserted:

```sql
-- Count total questions
SELECT COUNT(*) FROM "QuestionBanks";
-- Expected: 150+ rows

-- Count questions by role
SELECT "Role", COUNT(*) as QuestionCount
FROM "QuestionBanks"
GROUP BY "Role"
ORDER BY "Role";

-- Count questions by difficulty
SELECT "Difficulty", COUNT(*) as QuestionCount
FROM "QuestionBanks"
GROUP BY "Difficulty"
ORDER BY "Difficulty";

-- Count questions by category
SELECT "Category", COUNT(*) as QuestionCount
FROM "QuestionBanks"
GROUP BY "Category"
ORDER BY "Category";

-- Detailed breakdown
SELECT "Role", "Category", "Difficulty", COUNT(*) as QuestionCount
FROM "QuestionBanks"
GROUP BY "Role", "Category", "Difficulty"
ORDER BY "Role", "Category", "Difficulty";
```

## Re-seeding

If you need to re-seed the database:

```sql
-- WARNING: This deletes ALL questions
TRUNCATE TABLE "QuestionBanks" CASCADE;

-- Then run the seed script again
```

## Table Schema Reference

The `QuestionBanks` table has the following structure:

```sql
CREATE TABLE "QuestionBanks" (
    "Id" uuid PRIMARY KEY,
    "Role" varchar(100) NOT NULL,
    "Category" varchar(50) NOT NULL,
    "Difficulty" varchar(50) NOT NULL,
    "QuestionText" varchar(1000) NOT NULL,
    "CreatedAt" timestamp NOT NULL
);
```

## Notes

- All UUIDs are generated using PostgreSQL's `gen_random_uuid()` function
- All timestamps use `NOW()` for current time
- String literals with apostrophes use double single quotes (`''`) for escaping
- Table and column names are case-sensitive and quoted to match EF Core conventions

## Troubleshooting

### Error: relation "QuestionBanks" does not exist
**Solution**: Run EF Core migrations first:
```bash
cd CommunicaAI
dotnet ef database update
```

### Error: function gen_random_uuid() does not exist
**Solution**: Enable the pgcrypto extension:
```sql
CREATE EXTENSION IF NOT EXISTS pgcrypto;
```

### Error: duplicate key value violates unique constraint
**Solution**: The database already has data. Either truncate the table or skip seeding.

### Permission denied
**Solution**: Ensure your PostgreSQL user has INSERT permissions on the QuestionBanks table:
```sql
GRANT INSERT ON "QuestionBanks" TO your_username;
```

## AWS Deployment

When deploying to AWS EC2 with PostgreSQL:

1. SSH into your EC2 instance
2. Connect to PostgreSQL:
   ```bash
   sudo -u postgres psql -d communicaai
   ```
3. Run the seed script:
   ```sql
   \i /path/to/01_seed_question_bank.sql
   ```

Or use the API endpoint method (recommended for AWS):
```bash
curl -X POST http://your-ec2-public-ip:5000/api/question-bank/seed
```
