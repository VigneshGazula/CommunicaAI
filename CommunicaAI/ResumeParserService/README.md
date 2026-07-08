# Resume Parser Service

Python FastAPI service for parsing PDF and DOCX resumes.

## Features
- PDF parsing using PyPDF2
- DOCX parsing using python-docx
- Extracts: skills, experience, education, job titles, technologies
- Returns structured JSON metadata

## Setup

```bash
# Create virtual environment
python -m venv venv

# Activate (Windows)
venv\Scripts\activate

# Activate (Linux/Mac)
source venv/bin/activate

# Install dependencies
pip install -r requirements.txt
```

## Run

```bash
python main.py
```

Service runs on `http://localhost:8002`

## API Endpoints

### POST /parse-resume
Upload resume file (PDF or DOCX) for parsing.

**Request**: multipart/form-data with `file` field

**Response**:
```json
{
  "success": true,
  "metadata": {
    "skills": ["Python", "React", "AWS"],
    "experience": "5+ years",
    "education": ["Bachelor in Computer Science"],
    "jobTitles": ["Senior Software Engineer"],
    "technologies": ["Docker", "Kubernetes"],
    "summary": "5+ years of experience. Recent role: Senior Software Engineer"
  },
  "error": null
}
```

### GET /health
Health check endpoint.
