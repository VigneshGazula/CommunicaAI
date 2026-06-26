using CommunicaAI.DTO.QuestionBank;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IQuestionBankRepository _repository;

        public QuestionBankService(IQuestionBankRepository repository)
        {
            _repository = repository;
        }

        public async Task<QuestionBankResponse> CreateQuestionAsync(CreateQuestionRequest request)
        {
            var question = new QuestionBank
            {
                Id = Guid.NewGuid(),
                Role = request.Role,
                Category = request.Category,
                Difficulty = request.Difficulty,
                QuestionText = request.QuestionText,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(question);
            return MapToResponse(created);
        }

        public async Task<QuestionBankResponse?> GetQuestionByIdAsync(Guid id)
        {
            var question = await _repository.GetByIdAsync(id);
            return question == null ? null : MapToResponse(question);
        }

        public async Task<List<QuestionBankResponse>> GetAllQuestionsAsync()
        {
            var questions = await _repository.GetAllAsync();
            return questions.Select(MapToResponse).ToList();
        }

        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            var question = await _repository.GetByIdAsync(id);
            if (question == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task SeedQuestionsAsync()
        {
            var existing = await _repository.GetAllAsync();
            if (existing.Any()) return;

            var questions = GetSeedQuestions();
            foreach (var question in questions)
            {
                await _repository.CreateAsync(question);
            }
        }

        private static QuestionBankResponse MapToResponse(QuestionBank question)
        {
            return new QuestionBankResponse
            {
                Id = question.Id,
                Role = question.Role,
                Category = question.Category,
                Difficulty = question.Difficulty,
                QuestionText = question.QuestionText,
                CreatedAt = question.CreatedAt
            };
        }

        private static List<QuestionBank> GetSeedQuestions()
        {
            return new List<QuestionBank>
            {
                // Software Engineer - Technical - Easy
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is the difference between == and === in JavaScript?" },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Easy", QuestionText = "Explain what a variable is in programming." },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is the purpose of version control systems like Git?" },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is an array and how is it different from a list?" },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Easy", QuestionText = "Explain what a function is and why we use them." },

                // Software Engineer - Technical - Medium
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the concept of object-oriented programming." },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is the difference between SQL and NoSQL databases?" },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "Describe the MVC design pattern." },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is REST API and how does it work?" },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain asynchronous programming and why it's important." },

                // Software Engineer - Technical - Hard
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Design a scalable system for handling millions of concurrent users." },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Explain how you would implement a distributed caching system." },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you optimize database queries for a high-traffic application?" },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Describe your approach to implementing microservices architecture." },
                new() { Role = "Software Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you design a real-time notification system?" },

                // Software Engineer - Behavioral
                new() { Role = "Software Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a time when you had to debug a difficult problem." },
                new() { Role = "Software Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Describe a situation where you had to learn a new technology quickly." },
                new() { Role = "Software Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle code review feedback?" },
                new() { Role = "Software Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a project you're particularly proud of." },
                new() { Role = "Software Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Describe a time when you disagreed with a team member's technical approach." },

                // Software Engineer - HR
                new() { Role = "Software Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "Why do you want to work for our company?" },
                new() { Role = "Software Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "What are your salary expectations?" },
                new() { Role = "Software Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "Where do you see yourself in 5 years?" },
                new() { Role = "Software Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "What motivates you as a software engineer?" },
                new() { Role = "Software Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "How do you stay updated with new technologies?" },

                // Backend Developer - Technical - Easy
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is an API endpoint?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "Explain what HTTP status codes are." },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is the difference between GET and POST requests?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is a database schema?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "Explain what JSON is and why it's used." },

                // Backend Developer - Technical - Medium
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "How would you implement authentication in a web application?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the concept of middleware in backend frameworks." },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "What are database indexes and when should you use them?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you handle errors and exceptions in backend code?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain what dependency injection is." },

                // Backend Developer - Technical - Hard
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Hard", QuestionText = "Design a rate limiting system for an API." },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you implement a job queue system for background tasks?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Hard", QuestionText = "Explain your approach to database sharding." },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you design a system for handling file uploads at scale?" },
                new() { Role = "Backend Developer", Category = "Technical", Difficulty = "Hard", QuestionText = "Describe how you would implement caching strategies in a backend system." },

                // Frontend Developer - Technical - Easy
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is the DOM in web development?" },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "Explain the difference between HTML, CSS, and JavaScript." },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "What is responsive design?" },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "What are CSS selectors?" },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Easy", QuestionText = "Explain what a JavaScript event is." },

                // Frontend Developer - Technical - Medium
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "How does React's virtual DOM work?" },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the concept of state management in frontend applications." },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "What are CSS preprocessors and why use them?" },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you optimize website performance?" },
                new() { Role = "Frontend Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the difference between localStorage and sessionStorage." },

                // Data Scientist - Technical - Medium
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the difference between supervised and unsupervised learning." },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Medium", QuestionText = "What is overfitting and how do you prevent it?" },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Medium", QuestionText = "Describe the process of exploratory data analysis." },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you handle missing data in a dataset?" },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain what a confusion matrix is." },

                // Data Scientist - Technical - Hard
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you build a recommendation system from scratch?" },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Hard", QuestionText = "Explain your approach to feature engineering." },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Hard", QuestionText = "How do you evaluate the performance of a machine learning model?" },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Hard", QuestionText = "Describe how you would handle imbalanced datasets." },
                new() { Role = "Data Scientist", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you deploy a machine learning model to production?" },

                // DevOps Engineer - Technical - Medium
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the concept of CI/CD." },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is Docker and why is it useful?" },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "How does Kubernetes work?" },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is infrastructure as code?" },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the purpose of monitoring and logging in production systems." },

                // DevOps Engineer - Technical - Hard
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you design a zero-downtime deployment strategy?" },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Describe your approach to managing secrets in a cloud environment." },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you implement disaster recovery for a critical system?" },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Explain how you would set up auto-scaling for a web application." },
                new() { Role = "DevOps Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How do you handle security in a DevOps pipeline?" },

                // Cloud Engineer - Technical - Medium
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "What are the benefits of cloud computing?" },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the difference between IaaS, PaaS, and SaaS." },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is serverless computing?" },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you ensure high availability in the cloud?" },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is a VPC in cloud networking?" },

                // Cloud Engineer - Technical - Hard
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you design a multi-region cloud architecture?" },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Explain your approach to cloud cost optimization." },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you implement a disaster recovery strategy in the cloud?" },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Describe how you would secure cloud infrastructure." },
                new() { Role = "Cloud Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you migrate a legacy application to the cloud?" },

                // Full Stack Developer - Technical - Medium
                new() { Role = "Full Stack Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you design a full stack application architecture?" },
                new() { Role = "Full Stack Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain how frontend and backend communicate in a web application." },
                new() { Role = "Full Stack Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is your approach to testing full stack applications?" },
                new() { Role = "Full Stack Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you handle user authentication across frontend and backend?" },
                new() { Role = "Full Stack Developer", Category = "Technical", Difficulty = "Medium", QuestionText = "Describe your experience with different database technologies." },

                // Data Analyst - Technical - Medium
                new() { Role = "Data Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you approach data cleaning and preparation?" },
                new() { Role = "Data Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "What tools do you use for data visualization?" },
                new() { Role = "Data Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain how you would perform A/B testing analysis." },
                new() { Role = "Data Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you identify trends and patterns in data?" },
                new() { Role = "Data Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "Describe your experience with SQL for data analysis." },

                // Machine Learning Engineer - Technical - Hard
                new() { Role = "Machine Learning Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you design an ML pipeline from data collection to deployment?" },
                new() { Role = "Machine Learning Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Explain your approach to model versioning and experiment tracking." },
                new() { Role = "Machine Learning Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How do you handle model drift in production?" },
                new() { Role = "Machine Learning Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "Describe how you would implement real-time ML predictions." },
                new() { Role = "Machine Learning Engineer", Category = "Technical", Difficulty = "Hard", QuestionText = "How would you optimize ML model performance for production?" },

                // Behavioral questions (common across roles)
                new() { Role = "Backend Developer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Describe a time when you had to work under tight deadlines." },
                new() { Role = "Frontend Developer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle disagreements with team members?" },
                new() { Role = "Full Stack Developer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a challenging project you completed." },
                new() { Role = "Data Analyst", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you communicate technical findings to non-technical stakeholders?" },
                new() { Role = "Data Scientist", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Describe a time when your analysis led to an important decision." },
                new() { Role = "DevOps Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle production incidents?" },
                new() { Role = "Cloud Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a time you optimized cloud costs." },
                new() { Role = "Machine Learning Engineer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you approach solving complex ML problems?" },

                // HR questions (common across roles)
                new() { Role = "Backend Developer", Category = "HR", Difficulty = "Easy", QuestionText = "What interests you about backend development?" },
                new() { Role = "Frontend Developer", Category = "HR", Difficulty = "Easy", QuestionText = "Why did you choose frontend development?" },
                new() { Role = "Full Stack Developer", Category = "HR", Difficulty = "Easy", QuestionText = "What do you enjoy about full stack development?" },
                new() { Role = "Data Analyst", Category = "HR", Difficulty = "Easy", QuestionText = "What attracted you to data analysis?" },
                new() { Role = "Data Scientist", Category = "HR", Difficulty = "Easy", QuestionText = "Why do you want to be a data scientist?" },
                new() { Role = "DevOps Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "What motivates you in DevOps work?" },
                new() { Role = "Cloud Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "Why are you interested in cloud engineering?" },
                new() { Role = "Machine Learning Engineer", Category = "HR", Difficulty = "Easy", QuestionText = "What excites you about machine learning?" },

                // Product Manager - Technical - Medium
                new() { Role = "Product Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you prioritize features in a product roadmap?" },
                new() { Role = "Product Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain your approach to product discovery." },
                new() { Role = "Product Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you measure product success?" },
                new() { Role = "Product Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "What frameworks do you use for product strategy?" },
                new() { Role = "Product Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you work with engineering teams?" },
                new() { Role = "Product Manager", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a product you launched successfully." },
                new() { Role = "Product Manager", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle conflicting stakeholder requirements?" },
                new() { Role = "Product Manager", Category = "HR", Difficulty = "Easy", QuestionText = "What attracts you to product management?" },
                new() { Role = "Product Manager", Category = "HR", Difficulty = "Easy", QuestionText = "How do you stay updated with market trends?" },

                // Marketing Manager - Technical - Medium
                new() { Role = "Marketing Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you develop a marketing strategy?" },
                new() { Role = "Marketing Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "What metrics do you use to measure campaign success?" },
                new() { Role = "Marketing Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain your approach to digital marketing." },
                new() { Role = "Marketing Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you identify target audiences?" },
                new() { Role = "Marketing Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "What tools do you use for marketing analytics?" },
                new() { Role = "Marketing Manager", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Describe a successful marketing campaign you led." },
                new() { Role = "Marketing Manager", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle budget constraints?" },
                new() { Role = "Marketing Manager", Category = "HR", Difficulty = "Easy", QuestionText = "Why do you want to work in marketing?" },
                new() { Role = "Marketing Manager", Category = "HR", Difficulty = "Easy", QuestionText = "What marketing trends excite you most?" },

                // UX Designer - Technical - Medium
                new() { Role = "UX Designer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain your UX design process." },
                new() { Role = "UX Designer", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you conduct user research?" },
                new() { Role = "UX Designer", Category = "Technical", Difficulty = "Medium", QuestionText = "What is your approach to creating user personas?" },
                new() { Role = "UX Designer", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you measure UX success?" },
                new() { Role = "UX Designer", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain the difference between UX and UI design." },
                new() { Role = "UX Designer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a design challenge you solved." },
                new() { Role = "UX Designer", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle feedback on your designs?" },
                new() { Role = "UX Designer", Category = "HR", Difficulty = "Easy", QuestionText = "What inspired you to become a UX designer?" },
                new() { Role = "UX Designer", Category = "HR", Difficulty = "Easy", QuestionText = "What design tools do you prefer and why?" },

                // Business Analyst - Technical - Medium
                new() { Role = "Business Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you gather business requirements?" },
                new() { Role = "Business Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain your approach to business process analysis." },
                new() { Role = "Business Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "What tools do you use for data analysis?" },
                new() { Role = "Business Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you create business cases?" },
                new() { Role = "Business Analyst", Category = "Technical", Difficulty = "Medium", QuestionText = "Describe your experience with stakeholder management." },
                new() { Role = "Business Analyst", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a business problem you helped solve." },
                new() { Role = "Business Analyst", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle ambiguous requirements?" },
                new() { Role = "Business Analyst", Category = "HR", Difficulty = "Easy", QuestionText = "Why did you choose business analysis?" },
                new() { Role = "Business Analyst", Category = "HR", Difficulty = "Easy", QuestionText = "What aspects of business analysis do you enjoy most?" },

                // Sales Executive - Technical - Medium
                new() { Role = "Sales Executive", Category = "Technical", Difficulty = "Medium", QuestionText = "Describe your sales process." },
                new() { Role = "Sales Executive", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you qualify leads?" },
                new() { Role = "Sales Executive", Category = "Technical", Difficulty = "Medium", QuestionText = "What CRM tools have you used?" },
                new() { Role = "Sales Executive", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you handle objections?" },
                new() { Role = "Sales Executive", Category = "Technical", Difficulty = "Medium", QuestionText = "Explain your approach to closing deals." },
                new() { Role = "Sales Executive", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about your biggest sales achievement." },
                new() { Role = "Sales Executive", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you handle rejection?" },
                new() { Role = "Sales Executive", Category = "HR", Difficulty = "Easy", QuestionText = "What motivates you in sales?" },
                new() { Role = "Sales Executive", Category = "HR", Difficulty = "Easy", QuestionText = "Why do you want to work in sales?" },

                // Customer Success Manager - Technical - Medium
                new() { Role = "Customer Success Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you ensure customer satisfaction?" },
                new() { Role = "Customer Success Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "Describe your approach to onboarding new customers." },
                new() { Role = "Customer Success Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you measure customer success?" },
                new() { Role = "Customer Success Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "What strategies do you use to reduce churn?" },
                new() { Role = "Customer Success Manager", Category = "Technical", Difficulty = "Medium", QuestionText = "How do you handle escalations?" },
                new() { Role = "Customer Success Manager", Category = "Behavioral", Difficulty = "Medium", QuestionText = "Tell me about a time you turned around a dissatisfied customer." },
                new() { Role = "Customer Success Manager", Category = "Behavioral", Difficulty = "Medium", QuestionText = "How do you build relationships with customers?" },
                new() { Role = "Customer Success Manager", Category = "HR", Difficulty = "Easy", QuestionText = "Why are you interested in customer success?" },
                new() { Role = "Customer Success Manager", Category = "HR", Difficulty = "Easy", QuestionText = "What do you enjoy about helping customers?" }
            };
        }
    }
}
