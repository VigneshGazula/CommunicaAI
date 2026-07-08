"""
Resume Parser Service using FastAPI
Parses PDF and DOCX resumes to extract structured metadata
"""

from fastapi import FastAPI, UploadFile, File, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from typing import List, Optional
import PyPDF2
import docx
import re
import logging
from io import BytesIO

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(title="Resume Parser Service", version="1.0.0")

# CORS configuration
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Response Models
class ResumeMetadata(BaseModel):
    skills: List[str]
    experience: str  # e.g., "5 years", "2-3 years"
    education: List[str]
    jobTitles: List[str]
    technologies: List[str]
    summary: str

class ResumeParserResponse(BaseModel):
    success: bool
    metadata: Optional[ResumeMetadata]
    error: Optional[str]


class ResumeParser:
    
    # Common skills keywords
    SKILL_KEYWORDS = [
        'python', 'java', 'javascript', 'typescript', 'c#', 'c++', 'ruby', 'php', 'swift', 'kotlin',
        'react', 'angular', 'vue', 'node', 'express', 'django', 'flask', 'spring', 'asp.net',
        'sql', 'nosql', 'mongodb', 'postgresql', 'mysql', 'redis', 'elasticsearch',
        'aws', 'azure', 'gcp', 'docker', 'kubernetes', 'jenkins', 'ci/cd', 'devops',
        'machine learning', 'ai', 'deep learning', 'data science', 'analytics',
        'agile', 'scrum', 'jira', 'git', 'rest', 'api', 'microservices', 'graphql',
        'html', 'css', 'sass', 'webpack', 'babel', 'testing', 'jest', 'cypress'
    ]
    
    # Common job title keywords
    JOB_TITLE_KEYWORDS = [
        'engineer', 'developer', 'architect', 'manager', 'lead', 'senior', 'junior',
        'analyst', 'consultant', 'specialist', 'coordinator', 'designer', 'scientist',
        'director', 'vp', 'cto', 'ceo', 'founder', 'intern', 'associate'
    ]
    
    def parse_pdf(self, file_content: bytes) -> str:
        """Extract text from PDF"""
        try:
            pdf_reader = PyPDF2.PdfReader(BytesIO(file_content))
            text = ""
            for page in pdf_reader.pages:
                text += page.extract_text() + "\n"
            return text
        except Exception as e:
            logger.error(f"Error parsing PDF: {str(e)}")
            raise HTTPException(status_code=400, detail=f"Failed to parse PDF: {str(e)}")
    
    def parse_docx(self, file_content: bytes) -> str:
        """Extract text from DOCX"""
        try:
            doc = docx.Document(BytesIO(file_content))
            text = "\n".join([paragraph.text for paragraph in doc.paragraphs])
            return text
        except Exception as e:
            logger.error(f"Error parsing DOCX: {str(e)}")
            raise HTTPException(status_code=400, detail=f"Failed to parse DOCX: {str(e)}")
    
    def extract_skills(self, text: str) -> List[str]:
        """Extract technical skills from resume text"""
        text_lower = text.lower()
        found_skills = []
        
        for skill in self.SKILL_KEYWORDS:
            # Use word boundaries to avoid partial matches
            pattern = r'\b' + re.escape(skill) + r'\b'
            if re.search(pattern, text_lower):
                found_skills.append(skill.title())
        
        # Remove duplicates and sort
        return sorted(list(set(found_skills)))
    
    def extract_experience(self, text: str) -> str:
        """Extract years of experience from resume text"""
        # Look for patterns like "5 years", "2-3 years", "5+ years"
        patterns = [
            r'(\d+)\+?\s*years?',
            r'(\d+)-(\d+)\s*years?',
            r'(\d+)\s*months?'
        ]
        
        for pattern in patterns:
            match = re.search(pattern, text, re.IGNORECASE)
            if match:
                if '-' in match.group(0):
                    return f"{match.group(1)}-{match.group(2)} years"
                elif 'month' in match.group(0).lower():
                    months = int(match.group(1))
                    if months < 12:
                        return f"{months} months"
                    else:
                        return f"{months // 12} years"
                else:
                    return f"{match.group(1)}+ years"
        
        return "Not specified"
    
    def extract_education(self, text: str) -> List[str]:
        """Extract education information from resume text"""
        education = []
        
        # Common degree patterns
        degree_patterns = [
            r'\b(bachelor|b\.s\.|b\.a\.|bs|ba)\b.*?(computer science|engineering|science|arts)',
            r'\b(master|m\.s\.|m\.a\.|ms|ma|mba)\b.*?(computer science|engineering|business|science)',
            r'\b(phd|ph\.d\.|doctorate)\b.*?(computer science|engineering|science)',
            r'\b(associate|a\.s\.|a\.a\.)\b.*?(science|arts)'
        ]
        
        for pattern in degree_patterns:
            matches = re.finditer(pattern, text, re.IGNORECASE)
            for match in matches:
                education.append(match.group(0).strip())
        
        if not education:
            # Look for university names
            if re.search(r'university|college|institute', text, re.IGNORECASE):
                education.append("Degree mentioned")
        
        return education if education else ["Not specified"]
    
    def extract_job_titles(self, text: str) -> List[str]:
        """Extract job titles from resume text"""
        titles = []
        lines = text.split('\n')
        
        for line in lines:
            line_lower = line.lower()
            # Check if line contains job title keywords
            for keyword in self.JOB_TITLE_KEYWORDS:
                if keyword in line_lower and len(line.split()) < 10:  # Likely a job title
                    titles.append(line.strip())
                    break
        
        # Remove duplicates and limit
        unique_titles = []
        for title in titles:
            if title not in unique_titles and len(unique_titles) < 5:
                unique_titles.append(title)
        
        return unique_titles if unique_titles else ["Not specified"]
    
    def extract_technologies(self, text: str) -> List[str]:
        """Extract specific technologies mentioned"""
        # This is similar to skills but more focused on frameworks/tools
        tech_keywords = [
            'React', 'Angular', 'Vue', 'Node.js', 'Django', 'Flask', 'Spring Boot',
            'Docker', 'Kubernetes', 'AWS', 'Azure', 'GCP', 'MongoDB', 'PostgreSQL',
            'MySQL', 'Redis', 'Elasticsearch', 'Jenkins', 'Git', 'Jira'
        ]
        
        text_lower = text.lower()
        found_tech = []
        
        for tech in tech_keywords:
            if tech.lower() in text_lower:
                found_tech.append(tech)
        
        return sorted(list(set(found_tech)))
    
    def generate_summary(self, skills: List[str], experience: str, 
                        job_titles: List[str], education: List[str]) -> str:
        """Generate a brief summary of the resume"""
        summary_parts = []
        
        if experience and experience != "Not specified":
            summary_parts.append(f"{experience} of experience")
        
        if job_titles and job_titles[0] != "Not specified":
            summary_parts.append(f"Recent role: {job_titles[0]}")
        
        if skills:
            top_skills = ", ".join(skills[:5])
            summary_parts.append(f"Key skills: {top_skills}")
        
        if education and education[0] != "Not specified":
            summary_parts.append(f"Education: {education[0]}")
        
        return ". ".join(summary_parts) if summary_parts else "Resume parsed successfully"
    
    def parse_resume(self, file_content: bytes, file_type: str) -> ResumeMetadata:
        """Main parsing function"""
        try:
            # Extract text based on file type
            if file_type == 'pdf':
                text = self.parse_pdf(file_content)
            elif file_type in ['docx', 'doc']:
                text = self.parse_docx(file_content)
            else:
                raise HTTPException(status_code=400, detail="Unsupported file type")
            
            # Extract metadata
            skills = self.extract_skills(text)
            experience = self.extract_experience(text)
            education = self.extract_education(text)
            job_titles = self.extract_job_titles(text)
            technologies = self.extract_technologies(text)
            summary = self.generate_summary(skills, experience, job_titles, education)
            
            return ResumeMetadata(
                skills=skills,
                experience=experience,
                education=education,
                jobTitles=job_titles,
                technologies=technologies,
                summary=summary
            )
        
        except HTTPException:
            raise
        except Exception as e:
            logger.error(f"Error parsing resume: {str(e)}")
            raise HTTPException(status_code=500, detail=f"Failed to parse resume: {str(e)}")


parser = ResumeParser()


@app.get("/")
def read_root():
    return {"service": "Resume Parser Service", "status": "running", "version": "1.0.0"}


@app.get("/health")
def health_check():
    return {"status": "healthy"}


@app.post("/parse-resume", response_model=ResumeParserResponse)
async def parse_resume(file: UploadFile = File(...)):
    """
    Parse uploaded resume (PDF or DOCX) and extract structured metadata
    """
    try:
        # Validate file type
        allowed_types = ['application/pdf', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document']
        if file.content_type not in allowed_types:
            return ResumeParserResponse(
                success=False,
                metadata=None,
                error="Only PDF and DOCX files are supported"
            )
        
        # Read file content
        content = await file.read()
        
        # Determine file type
        file_type = 'pdf' if file.content_type == 'application/pdf' else 'docx'
        
        # Parse resume
        metadata = parser.parse_resume(content, file_type)
        
        return ResumeParserResponse(
            success=True,
            metadata=metadata,
            error=None
        )
    
    except HTTPException as he:
        return ResumeParserResponse(
            success=False,
            metadata=None,
            error=str(he.detail)
        )
    except Exception as e:
        logger.error(f"Unexpected error in parse_resume: {str(e)}")
        return ResumeParserResponse(
            success=False,
            metadata=None,
            error=f"Failed to parse resume: {str(e)}"
        )


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8002, log_level="info")
