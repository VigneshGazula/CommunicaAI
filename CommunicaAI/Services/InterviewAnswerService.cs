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
            try
            {
                Console.WriteLine($"=== Submit Audio Answer ===");
                Console.WriteLine($"Session: {sessionId}, Question: {questionId}, User: {userId}");
                Console.WriteLine($"Audio: {audioFile.FileName}, Size: {audioFile.Length} bytes, Type: {audioFile.ContentType}");
                
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
                    Console.WriteLine($"Updating existing answer {existing.Id}");
                    // Update existing answer instead of throwing error
                    existing.AudioUrl = null; // Will be updated below
                    existing.Transcript = ""; // Will be updated below
                    existing.AnsweredAt = DateTime.UtcNow;
                    await _answerRepository.UpdateAsync(existing);
                    
                    // Delete old evaluation if exists
                    var oldEval = await _answerEvaluationRepository.GetByAnswerIdAsync(existing.Id);
                    if (oldEval != null)
                    {
                        await _answerEvaluationRepository.DeleteAsync(oldEval.Id);
                    }
                }

                // Upload audio to Cloudinary
                Console.WriteLine("Uploading audio to Cloudinary...");
                var upload =
                    await _cloudinaryService.UploadAudioAsync(
                        audioFile,
                        (Guid)session.UserId);
                Console.WriteLine($"Upload successful: {upload.Url}");

                // Transcribe audio ONLY (no evaluation yet)
                Console.WriteLine("Starting transcription...");
                using var stream = audioFile.OpenReadStream();

                var transcript =
                    await _transcriptionService.TranscribeAsync(
                        stream,
                        audioFile.ContentType);
                
                Console.WriteLine($"Transcription successful: {transcript.Substring(0, Math.Min(100, transcript.Length))}...");

                // Create or update answer (transcription only, no evaluation)
                InterviewAnswer answer;
                if (existing != null)
                {
                    answer = existing;
                    answer.Transcript = transcript;
                    answer.AudioUrl = upload.Url;
                    answer.DurationSeconds = durationSeconds;
                    await _answerRepository.UpdateAsync(answer);
                    Console.WriteLine($"Answer updated: {answer.Id}");
                }
                else
                {
                    answer = new InterviewAnswer
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
                    Console.WriteLine($"New answer created: {answer.Id}");
                }

                // Mark question answered
                question.IsAnswered = true;
                await _questionRepository.UpdateAsync(question);

                // Return transcript only (no evaluation scores)
                return new SubmitAudioAnswerResponse
                {
                    AnswerId = answer.Id,
                    Transcript = transcript,
                    AudioUrl = upload.Url,
                    TechnicalScore = 0,
                    ClarityScore = 0,
                    CompletenessScore = 0,
                    OverallScore = 0,
                    Feedback = "Answer recorded. Evaluation will be performed after interview completion."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in SubmitAudioAnswerAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}