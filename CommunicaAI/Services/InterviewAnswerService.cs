using CommunicaAI.DTO.Interview;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services
{
    public class InterviewAnswerService : IInterviewAnswerService
    {
        private readonly IInterviewAnswerRepository _answerRepository;
        private readonly IInterviewQuestionRepository _questionRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly ITranscriptionService _transcriptionService;
        private readonly IGeminiService _geminiService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IAnswerEvaluationRepository _answerEvaluationRepository;

        public InterviewAnswerService(
            IInterviewAnswerRepository answerRepository,
            IInterviewQuestionRepository questionRepository,
            IInterviewRepository interviewRepository,
            ITranscriptionService transcriptionService,
            IGeminiService geminiService,
            ICloudinaryService cloudinaryService,
            IAnswerEvaluationRepository answerEvaluationRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _interviewRepository = interviewRepository;
            _transcriptionService = transcriptionService;
            _geminiService = geminiService;
            _cloudinaryService = cloudinaryService;
            _answerEvaluationRepository = answerEvaluationRepository;
        }

        public async Task<AnswerResponse> SubmitAnswerAsync(Guid sessionId, Guid userId, AnswerSubmitRequest request)
        {
            var session = await _interviewRepository.GetByIdAsync(sessionId);
            if (session == null || session.UserId != userId)
            {
                throw new UnauthorizedAccessException("Session not found or unauthorized");
            }

            var question = await _questionRepository.GetBySessionAndQuestionIdAsync(sessionId, request.QuestionId);
            if (question == null)
            {
                throw new InvalidOperationException("Question does not belong to this session");
            }

            var existingAnswer = await _answerRepository.GetByQuestionIdAsync(request.QuestionId);
            if (existingAnswer != null)
            {
                throw new InvalidOperationException("Question already answered");
            }

            var answer = new InterviewAnswer
            {
                Id = Guid.NewGuid(),
                InterviewQuestionId = request.QuestionId,
                InterviewSessionId = sessionId,
                Transcript = request.Transcript,
                AnsweredAt = DateTime.UtcNow
            };

            var created = await _answerRepository.CreateAsync(answer);

            question.IsAnswered = true;
            await _questionRepository.UpdateAsync(question);

            return new AnswerResponse
            {
                Id = created.Id,
                QuestionId = created.InterviewQuestionId,
                Transcript = created.Transcript,
                AnsweredAt = created.AnsweredAt
            };
        }

        public async Task<SubmitAudioAnswerResponse> SubmitAudioAnswerAsync(
    Guid sessionId,
    Guid questionId,
    IFormFile audioFile,
    int durationSeconds,
    Guid userId)
        {
            // Validate session
            var session = await _interviewRepository.GetByIdAsync(sessionId);

            if (session == null || session.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "Session not found or unauthorized.");
            }

            // Validate question
            var question =
                await _questionRepository.GetBySessionAndQuestionIdAsync(
                    sessionId,
                    questionId);

            if (question == null)
            {
                throw new InvalidOperationException(
                    "Question not found.");
            }

            // Check duplicate answer
            var existing =
                await _answerRepository.GetByQuestionIdAsync(questionId);

            if (existing != null)
            {
                throw new InvalidOperationException(
                    "Question already answered.");
            }

            // Upload audio to Cloudinary
            var upload =
                await _cloudinaryService.UploadAudioAsync(
                    audioFile,
                    (Guid)session.UserId);   // <-- change this if UserId is int

            // Transcribe audio
            using var stream = audioFile.OpenReadStream();

            var transcript =
                await _transcriptionService.TranscribeAsync(
                    stream,
                    audioFile.ContentType);

            // Evaluate transcript
            var evaluation =
                await _geminiService.EvaluateAnswerAsync(
                    question.QuestionText,
                    transcript);

            // Create answer
            var answer = new InterviewAnswer
            {
                Id = Guid.NewGuid(),

                InterviewQuestionId = questionId,

                InterviewSessionId = sessionId,

                Transcript = transcript,

                AudioUrl = upload.Url,

                DurationSeconds = durationSeconds,

                AnsweredAt = DateTime.UtcNow
            };

            await _answerRepository.CreateAsync(answer);

            var answerEvaluation = new AnswerEvaluation
            {
                Id = Guid.NewGuid(),
                InterviewAnswerId = answer.Id,
                TechnicalScore = evaluation.TechnicalScore,
                ClarityScore = evaluation.ClarityScore,
                CompletenessScore = evaluation.CompletenessScore,
                OverallScore = evaluation.OverallScore,
                Strengths = evaluation.Strengths,
                Improvements = evaluation.Improvements,
                Feedback = evaluation.Feedback,
                EvaluatedAt = DateTime.UtcNow
            };

            await _answerEvaluationRepository.CreateAsync(answerEvaluation);

            // Mark question answered
            question.IsAnswered = true;

            await _questionRepository.UpdateAsync(question);

            // TODO:
            // Save AnswerEvaluation once AnswerEvaluationRepository exists

            return new SubmitAudioAnswerResponse
            {
                AnswerId = answer.Id,

                Transcript = transcript,

                AudioUrl = upload.Url,

                TechnicalScore = evaluation.TechnicalScore,

                ClarityScore = evaluation.ClarityScore,

                CompletenessScore = evaluation.CompletenessScore,

                OverallScore = evaluation.OverallScore,

                Feedback = evaluation.Feedback
            };
        }
    }
}