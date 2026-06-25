// Backend API Request/Response DTOs

export interface CreateInterviewRequest {
  role: string;
  topic: string;
  difficulty: string;
  questionCount: number;
  durationMinutes: number;
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
  technicalScore: number;
  clarityScore: number;
  completenessScore: number;
  overallScore: number;
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
  strengths?: string;
  weaknesses?: string;
  recommendations?: string;
  summary?: string;
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
  technicalScore: number;
  clarityScore: number;
  completenessScore: number;
  overallScore: number;
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
}

export interface InterviewResult {
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

export interface InterviewStats {
  totalInterviews: number;
  averageScore: number;
  currentStreak: number;
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
