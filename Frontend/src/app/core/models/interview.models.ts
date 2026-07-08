// Backend API Request/Response DTOs

export interface CreateInterviewRequest {
  role: string;
  topic: string;
  difficulty: string;
  questionCount: number;
  durationMinutes: number;
  companyProfileId?: string; // Module 6: Company Intelligence
  resumeProfileId?: string; // Module 7: Resume Intelligence
}

export interface CreateInterviewResponse {
  sessionId: string;
  status: string;
  startedAt: string;
}

export interface QuestionResponse {
  id: string;
  orderNumber: number;
  category: string;
  questionText: string;
  isAnswered: boolean;
}

export interface AnswerResponse {
  id: string;
  questionId: string;
  transcript: string;
  answeredAt: string;
}

export interface SubmitAudioAnswerResponse {
  answerId: string;
  transcript: string;
  audioUrl: string;
  // Technical Evaluation
  technicalScore: number;
  clarityScore: number;
  completenessScore: number;
  overallScore: number;
  // AI Communication Evaluation (Module 3)
  communicationScore: number;
  confidenceScore: number;
  grammarScore: number;
  vocabularyScore: number;
  professionalismScore: number;
  answerStructureScore: number;
  persuasivenessScore: number;
  concisenessScore: number;
  // Feedback
  strengths: string;
  improvements: string;
  feedback: string;
}

export interface InterviewResultResponse {
  totalQuestions: number;
  answeredQuestions: number;
  completionPercentage: number;
  generatedAt: string;
  overallScore?: number;
  technicalScore?: number;
  communicationScore?: number;
  confidenceScore?: number;
  // Video Intelligence (Module 4)
  eyeContactScore?: number;
  postureScore?: number;
  facialExpressionScore?: number;
  videoConfidenceScore?: number;
  videoFeedback?: string;
  // Feedback
  strengths?: string;
  weaknesses?: string;
  recommendations?: string;
  summary?: string;
  // AI Interview Coach (Module 5)
  coachingSummary?: string;
  coachingStrengths?: string;
  coachingWeaknesses?: string;
  communicationImprovements?: string;
  technicalImprovements?: string;
  videoImprovements?: string;
  voiceImprovements?: string;
  practiceRecommendations?: string;
  suggestedRole?: string;
  suggestedDifficulty?: string;
  suggestedQuestionCount?: number;
  learningResources?: string;
  motivationalMessage?: string;
  // Company Intelligence (Module 6)
  companyReadinessScore?: number;
  technicalAlignment?: number;
  communicationAlignment?: number;
  cultureFit?: number;
  companySpecificFeedback?: string;
  // Resume Intelligence (Module 7)
  resumeMatchScore?: number;
  skillGapSummary?: string;
  careerRecommendations?: string;
}

export interface QuestionWithAnswerResponse {
  id: string;
  orderNumber: number;
  category: string;
  questionText: string;
  isAnswered: boolean;
  answer: AnswerResponse | null;
}

export interface InterviewDetailResponse {
  sessionId: string;
  role: string;
  topic: string;
  difficulty: string;
  questionCount: number;
  durationMinutes: number;
  status: string;
  startedAt: string;
  completedAt: string | null;
  questions: QuestionWithAnswerResponse[];
  result: InterviewResultResponse | null;
}

// Frontend Models (for component usage)

export interface InterviewSetup {
  role: string;
  topic: string;
  difficulty: 'easy' | 'medium' | 'hard';
  duration: number; // minutes
  questionCount: number;
}

export interface InterviewQuestion {
  id: string;
  text: string;
  order: number;
  category?: string;
  isAnswered?: boolean;
}

export interface InterviewAnswer {
  questionId: string;
  text: string;
  timestamp: Date;
  audioUrl?: string;
  evaluation?: AnswerEvaluation;
}

export interface AnswerEvaluation {
  // Technical Evaluation
  technicalScore: number;
  clarityScore: number;
  completenessScore: number;
  overallScore: number;
  // AI Communication Evaluation (Module 3)
  communicationScore: number;
  confidenceScore: number;
  grammarScore: number;
  vocabularyScore: number;
  professionalismScore: number;
  answerStructureScore: number;
  persuasivenessScore: number;
  concisenessScore: number;
  // Feedback
  strengths: string;
  improvements: string;
  feedback: string;
}

export interface InterviewSession {
  id: string;
  setup: InterviewSetup;
  questions: InterviewQuestion[];
  answers: InterviewAnswer[];
  status: 'draft' | 'in-progress' | 'completed';
  createdAt: Date;
  completedAt?: Date;
  currentQuestionIndex: number;
  result?: {
    overallScore: number;
    technicalScore: number;
    communicationScore: number;
    confidenceScore: number;
    eyeContactScore?: number;
    postureScore?: number;
    facialExpressionScore?: number;
    videoConfidenceScore?: number;
    videoFeedback?: string;
    strengths: string;
    weaknesses: string;
    recommendations: string;
    summary: string;
    // AI Coach
    coachingSummary?: string;
    coachingStrengths?: string;
    coachingWeaknesses?: string;
    communicationImprovements?: string;
    technicalImprovements?: string;
    videoImprovements?: string;
    voiceImprovements?: string;
    practiceRecommendations?: string;
    suggestedRole?: string;
    suggestedDifficulty?: string;
    suggestedQuestionCount?: number;
    learningResources?: string;
    motivationalMessage?: string;
    // Company Intelligence (Module 6)
    companyReadinessScore?: number;
    technicalAlignment?: number;
    communicationAlignment?: number;
    cultureFit?: number;
    companySpecificFeedback?: string;
    // Resume Intelligence (Module 7)
    resumeMatchScore?: number;
    skillGapSummary?: string;
    careerRecommendations?: string;
  };
}

export interface InterviewHistoryResponse {
  sessionId: string;
  role: string;
  difficulty: string;
  startedAt: string;
  completedAt: string | null;
  status: string;
  completionPercentage: number | null;
}

// Interview metadata from backend
export interface InterviewMetadata {
  roles: string[];
  difficulties: string[];
  categories: string[];
}


// Company Intelligence (Module 6)
export interface CompanyProfile {
  id: string;
  companyName: string;
  interviewStyle: string;
  focusAreas: string;
}


// Resume Intelligence (Module 7)
export interface ResumeMetadata {
  skills: string[];
  experience: string;
  education: string[];
  jobTitles: string[];
  technologies: string[];
  summary: string;
}

export interface UploadResumeResponse {
  resumeId: string;
  fileName: string;
  metadata: ResumeMetadata;
}

export interface ResumeProfile {
  id: string;
  fileName: string;
  experience: string;
  skills: string[];
  uploadedAt: string;
}

// Module 8: Performance Analytics
export interface PerformanceAnalyticsResponse {
  overallProgress: OverallProgressData;
  technicalScoreTrend: TrendDataPoint[];
  communicationScoreTrend: TrendDataPoint[];
  confidenceScoreTrend: TrendDataPoint[];
  videoAnalysisTrend: TrendDataPoint[];
  resumeMatchTrend: TrendDataPoint[];
  companyReadinessTrend: TrendDataPoint[];
  strongestSkills: SkillData[];
  weakestSkills: SkillData[];
  practiceRecommendations: PracticeRecommendationsData;
  weeklyProgress: WeeklyProgressData;
}

export interface OverallProgressData {
  totalInterviews: number;
  completedInterviews: number;
  averageOverallScore: number;
  averageTechnicalScore: number;
  averageCommunicationScore: number;
  averageConfidenceScore: number;
  currentStreak: number;
  longestStreak: number;
  improvementRate: number;
}

export interface TrendDataPoint {
  date: string;
  score: number;
  role: string;
  difficulty: string;
}

export interface SkillData {
  skillName: string;
  averageScore: number;
  frequency: number;
  category: string;
}

export interface PracticeRecommendationsData {
  focusAreas: string[];
  recommendedRole: string;
  recommendedDifficulty: string;
  topicsToImprove: string[];
  nextStepsSummary: string;
}

export interface WeeklyProgressData {
  interviewsThisWeek: number;
  interviewsLastWeek: number;
  averageScoreThisWeek: number;
  averageScoreLastWeek: number;
  weekOverWeekImprovement: number;
}
