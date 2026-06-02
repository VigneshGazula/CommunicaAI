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
}

export interface InterviewAnswer {
  questionId: string;
  text: string;
  timestamp: Date;
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
