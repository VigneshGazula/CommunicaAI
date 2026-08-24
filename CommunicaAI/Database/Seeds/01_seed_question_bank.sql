-- =====================================================
-- CommunicaAI - QuestionBank Seed Data
-- PostgreSQL Compatible
-- Table: QuestionBanks
-- =====================================================
-- This script seeds the QuestionBanks table with interview questions
-- for various roles, categories, and difficulty levels.
-- =====================================================

-- Software Engineer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is the difference between == and === in JavaScript?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'Explain what a variable is in programming.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is the purpose of version control systems like Git?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is an array and how is it different from a list?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'Explain what a function is and why we use them.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Explain the concept of object-oriented programming.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What is the difference between SQL and NoSQL databases?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Describe the MVC design pattern.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What is REST API and how does it work?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Explain asynchronous programming and why it''s important.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Design a scalable system for handling millions of concurrent users.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Explain how you would implement a distributed caching system.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'How would you optimize database queries for a high-traffic application?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Describe your approach to implementing microservices architecture.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'How would you design a real-time notification system?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Tell me about a time when you had to debug a difficult problem.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Describe a situation where you had to learn a new technology quickly.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'How do you handle code review feedback?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Tell me about a project you''re particularly proud of.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Describe a time when you disagreed with a team member''s technical approach.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'Why do you want to work for our company?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'What are your salary expectations?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'Where do you see yourself in 5 years?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'What motivates you as a software engineer?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'How do you stay updated with new technologies?', NOW());

-- Backend Developer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is an API endpoint?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'Explain what HTTP status codes are.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is the difference between GET and POST requests?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is a database schema?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'Explain what JSON is and why it''s used.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'How would you implement authentication in a web application?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'Explain the concept of middleware in backend frameworks.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'What are database indexes and when should you use them?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'How do you handle errors and exceptions in backend code?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'Explain what dependency injection is.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Design a rate limiting system for an API.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'How would you implement a job queue system for background tasks?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Explain your approach to database sharding.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'How would you design a system for handling file uploads at scale?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Describe how you would implement caching strategies in a backend system.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'Describe a time when you had to work under tight deadlines.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'HR', 'Easy', 'What interests you about backend development?', NOW());

-- Frontend Developer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is the DOM in web development?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'Explain the difference between HTML, CSS, and JavaScript.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is responsive design?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What are CSS selectors?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'Explain what a JavaScript event is.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'How does React''s virtual DOM work?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'Explain the concept of state management in frontend applications.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'What are CSS preprocessors and why use them?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'How do you optimize website performance?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'Explain the difference between localStorage and sessionStorage.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'How do you handle disagreements with team members?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'HR', 'Easy', 'Why did you choose frontend development?', NOW());

-- Data Scientist Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Explain the difference between supervised and unsupervised learning.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'What is overfitting and how do you prevent it?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Describe the process of exploratory data analysis.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'How do you handle missing data in a dataset?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Explain what a confusion matrix is.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How would you build a recommendation system from scratch?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'Explain your approach to feature engineering.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How do you evaluate the performance of a machine learning model?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'Describe how you would handle imbalanced datasets.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How would you deploy a machine learning model to production?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'Describe a time when your analysis led to an important decision.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'HR', 'Easy', 'Why do you want to be a data scientist?', NOW());

-- DevOps Engineer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'Explain the concept of CI/CD.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is Docker and why is it useful?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'How does Kubernetes work?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is infrastructure as code?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'Explain the purpose of monitoring and logging in production systems.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How would you design a zero-downtime deployment strategy?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'Describe your approach to managing secrets in a cloud environment.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How would you implement disaster recovery for a critical system?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'Explain how you would set up auto-scaling for a web application.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How do you handle security in a DevOps pipeline?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'How do you handle production incidents?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'HR', 'Easy', 'What motivates you in DevOps work?', NOW());

-- Cloud Engineer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What are the benefits of cloud computing?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'Explain the difference between IaaS, PaaS, and SaaS.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What is serverless computing?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'How do you ensure high availability in the cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What is a VPC in cloud networking?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you design a multi-region cloud architecture?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'Explain your approach to cloud cost optimization.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you implement a disaster recovery strategy in the cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'Describe how you would secure cloud infrastructure.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you migrate a legacy application to the cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'Tell me about a time you optimized cloud costs.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'HR', 'Easy', 'Why are you interested in cloud engineering?', NOW());

-- Full Stack Developer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you design a full stack application architecture?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'Explain how frontend and backend communicate in a web application.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'What is your approach to testing full stack applications?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you handle user authentication across frontend and backend?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'Describe your experience with different database technologies.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'Tell me about a challenging project you completed.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'HR', 'Easy', 'What do you enjoy about full stack development?', NOW());

-- Data Analyst Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you approach data cleaning and preparation?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'What tools do you use for data visualization?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'Explain how you would perform A/B testing analysis.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you identify trends and patterns in data?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'Describe your experience with SQL for data analysis.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'How do you communicate technical findings to non-technical stakeholders?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'HR', 'Easy', 'What attracted you to data analysis?', NOW());

-- Machine Learning Engineer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How would you design an ML pipeline from data collection to deployment?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'Explain your approach to model versioning and experiment tracking.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How do you handle model drift in production?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'Describe how you would implement real-time ML predictions.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How would you optimize ML model performance for production?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'How do you approach solving complex ML problems?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'HR', 'Easy', 'What excites you about machine learning?', NOW());

-- Product Manager Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you prioritize features in a product roadmap?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'Explain your approach to product discovery.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you measure product success?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'What frameworks do you use for product strategy?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you work with engineering teams?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'Tell me about a product you launched successfully.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'How do you handle conflicting stakeholder requirements?', NOW()),
(gen_random_uuid(), 'Product Manager', 'HR', 'Easy', 'What attracts you to product management?', NOW()),
(gen_random_uuid(), 'Product Manager', 'HR', 'Easy', 'How do you stay updated with market trends?', NOW());

-- Marketing Manager Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you develop a marketing strategy?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'What metrics do you use to measure campaign success?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'Explain your approach to digital marketing.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you identify target audiences?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'What tools do you use for marketing analytics?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'Describe a successful marketing campaign you led.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'How do you handle budget constraints?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'HR', 'Easy', 'Why do you want to work in marketing?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'HR', 'Easy', 'What marketing trends excite you most?', NOW());

-- UX Designer Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'Explain your UX design process.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you conduct user research?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'What is your approach to creating user personas?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you measure UX success?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'Explain the difference between UX and UI design.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'Tell me about a design challenge you solved.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'How do you handle feedback on your designs?', NOW()),
(gen_random_uuid(), 'UX Designer', 'HR', 'Easy', 'What inspired you to become a UX designer?', NOW()),
(gen_random_uuid(), 'UX Designer', 'HR', 'Easy', 'What design tools do you prefer and why?', NOW());

-- Business Analyst Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you gather business requirements?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'Explain your approach to business process analysis.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'What tools do you use for data analysis?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you create business cases?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'Describe your experience with stakeholder management.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'Tell me about a business problem you helped solve.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'How do you handle ambiguous requirements?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'HR', 'Easy', 'Why did you choose business analysis?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'HR', 'Easy', 'What aspects of business analysis do you enjoy most?', NOW());

-- Sales Executive Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'Describe your sales process.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you qualify leads?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'What CRM tools have you used?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you handle objections?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'Explain your approach to closing deals.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'Tell me about your biggest sales achievement.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'How do you handle rejection?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'HR', 'Easy', 'What motivates you in sales?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'HR', 'Easy', 'Why do you want to work in sales?', NOW());

-- Customer Success Manager Questions
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you ensure customer satisfaction?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'Describe your approach to onboarding new customers.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you measure customer success?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'What strategies do you use to reduce churn?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you handle escalations?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'Tell me about a time you turned around a dissatisfied customer.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'How do you build relationships with customers?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'HR', 'Easy', 'Why are you interested in customer success?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'HR', 'Easy', 'What do you enjoy about helping customers?', NOW());

-- =====================================================
-- Verification Query
-- Run this after seeding to verify the data
-- =====================================================
-- SELECT "Role", "Category", "Difficulty", COUNT(*) as QuestionCount
-- FROM "QuestionBanks"
-- GROUP BY "Role", "Category", "Difficulty"
-- ORDER BY "Role", "Category", "Difficulty";
