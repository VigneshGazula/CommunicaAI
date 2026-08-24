-- =====================================================
-- CommunicaAI - QuestionBank Comprehensive Seed Data
-- PostgreSQL Compatible
-- Table: QuestionBanks
-- Total Questions: 670+
-- =====================================================

-- =====================================================
-- SOFTWARE ENGINEER (48 questions)
-- =====================================================

-- Software Engineer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is the difference between == and === in JavaScript?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'Explain what a variable is in programming.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is the purpose of version control systems like Git?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is an array and how is it different from a list?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'Explain what a function is and why we use them.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is the difference between a class and an object?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'Explain what an API is in simple terms.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is the purpose of a constructor in object-oriented programming?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'What is the difference between a stack and a queue?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Easy', 'Explain what debugging means and why it''s important.', NOW());

-- Software Engineer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Explain the concept of object-oriented programming and its four pillars.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What is the difference between SQL and NoSQL databases?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Describe the MVC design pattern and its benefits.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What is REST API and how does it work?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Explain asynchronous programming and why it''s important.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What are design patterns and why are they useful?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Explain the concept of recursion with an example.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What is the difference between abstract class and interface?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Describe what a hash table is and when to use it.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What is dependency injection and what problem does it solve?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Explain the SOLID principles in software development.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What is the difference between concurrency and parallelism?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Describe what a binary search tree is and its time complexity.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'What are webhooks and how do they differ from APIs?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Medium', 'Explain the concept of caching and its benefits.', NOW());

-- Software Engineer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Design a scalable system for handling millions of concurrent users.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Explain how you would implement a distributed caching system.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'How would you optimize database queries for a high-traffic application?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Describe your approach to implementing microservices architecture.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'How would you design a real-time notification system?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Explain how you would implement rate limiting for an API.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Design a system for processing large amounts of data in real-time.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'How would you handle database schema migrations in a production system?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'Describe strategies for preventing and handling memory leaks.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Technical', 'Hard', 'How would you design a load balancer from scratch?', NOW());

-- Software Engineer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Tell me about a time when you had to debug a difficult problem.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Describe a situation where you had to learn a new technology quickly.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'How do you handle code review feedback?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Tell me about a project you''re particularly proud of.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Describe a time when you disagreed with a team member''s technical approach.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'How do you prioritize tasks when working on multiple projects?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Tell me about a time when you made a mistake in production.', NOW()),
(gen_random_uuid(), 'Software Engineer', 'Behavioral', 'Medium', 'Describe a situation where you had to refactor legacy code.', NOW());

-- Software Engineer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'Why do you want to work for our company?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'What are your salary expectations?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'Where do you see yourself in 5 years?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'What motivates you as a software engineer?', NOW()),
(gen_random_uuid(), 'Software Engineer', 'HR', 'Easy', 'How do you stay updated with new technologies?', NOW());

-- =====================================================
-- BACKEND DEVELOPER (48 questions)
-- =====================================================

-- Backend Developer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is an API endpoint?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'Explain what HTTP status codes are and give examples.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is the difference between GET and POST requests?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is a database schema?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'Explain what JSON is and why it''s used.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is the purpose of environment variables?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is CRUD and what does it stand for?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'Explain what a foreign key is in databases.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is the difference between authentication and authorization?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Easy', 'What is a cookie and how is it used in web applications?', NOW());

-- Backend Developer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'How would you implement authentication in a web application?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'Explain the concept of middleware in backend frameworks.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'What are database indexes and when should you use them?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'How do you handle errors and exceptions in backend code?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'Explain what dependency injection is and its benefits.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'What is the N+1 query problem and how do you solve it?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'Describe the difference between monolithic and microservices architecture.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'How do you implement pagination in an API?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'What are database transactions and why are they important?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'Explain how session management works in web applications.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'What is CORS and why is it important?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'How do you secure API endpoints?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'Explain the concept of connection pooling in databases.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'What is the difference between JWT and session-based authentication?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Medium', 'How do you handle file uploads in a backend API?', NOW());

-- Backend Developer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Design a rate limiting system for an API.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'How would you implement a job queue system for background tasks?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Explain your approach to database sharding.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'How would you design a system for handling file uploads at scale?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Describe how you would implement caching strategies in a backend system.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'How would you design an event-driven architecture?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Explain strategies for handling database migration with zero downtime.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'How would you implement a webhook system?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'Design a system for managing distributed transactions.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Technical', 'Hard', 'How would you optimize a slow API endpoint handling complex queries?', NOW());

-- Backend Developer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'Describe a time when you had to work under tight deadlines.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'Tell me about a complex backend system you designed.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'How do you approach performance optimization?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'Describe a time when you had to troubleshoot a production issue.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'How do you ensure the security of your backend systems?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'Tell me about a time you had to make a technical trade-off decision.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'Describe your experience with API design and versioning.', NOW()),
(gen_random_uuid(), 'Backend Developer', 'Behavioral', 'Medium', 'How do you handle database performance issues?', NOW());

-- Backend Developer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Backend Developer', 'HR', 'Easy', 'What interests you about backend development?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'HR', 'Easy', 'Why are you interested in this position?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'HR', 'Easy', 'What backend technologies are you most comfortable with?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'HR', 'Easy', 'How do you balance technical debt with new feature development?', NOW()),
(gen_random_uuid(), 'Backend Developer', 'HR', 'Easy', 'What''s your approach to learning new backend frameworks?', NOW());

-- =====================================================
-- FRONTEND DEVELOPER (48 questions)
-- =====================================================

-- Frontend Developer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is the DOM in web development?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'Explain the difference between HTML, CSS, and JavaScript.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is responsive design?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What are CSS selectors?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'Explain what a JavaScript event is.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is the box model in CSS?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is the difference between display: none and visibility: hidden?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'Explain what semantic HTML means.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is the purpose of the alt attribute in images?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Easy', 'What is the difference between margin and padding?', NOW());

-- Frontend Developer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'How does React''s virtual DOM work?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'Explain the concept of state management in frontend applications.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'What are CSS preprocessors and why use them?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'How do you optimize website performance?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'Explain the difference between localStorage and sessionStorage.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'What is the difference between let, const, and var in JavaScript?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'Explain how closures work in JavaScript.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'What are React hooks and how do they work?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'How do you handle cross-browser compatibility issues?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'Explain the concept of CSS Flexbox and Grid.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'What is AJAX and how does it work?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'How do you implement lazy loading for images?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'What is the difference between controlled and uncontrolled components?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'Explain how the event loop works in JavaScript.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Medium', 'What are service workers and how are they used?', NOW());

-- Frontend Developer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'How would you optimize the rendering performance of a complex React application?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'Explain how you would implement server-side rendering.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'How would you design a component library for a large organization?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'Describe strategies for reducing bundle size in a web application.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'How would you implement real-time features in a frontend application?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'Explain how you would handle state management in a large-scale application.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'How would you implement progressive web app features?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'Describe your approach to implementing accessibility in complex UIs.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'How would you optimize critical rendering path?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Technical', 'Hard', 'Explain strategies for managing memory leaks in JavaScript applications.', NOW());

-- Frontend Developer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'How do you handle disagreements with designers about UI implementation?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'Tell me about a challenging UI feature you implemented.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'How do you approach browser compatibility issues?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'Describe a time when you improved the performance of a web application.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'How do you stay updated with rapidly changing frontend technologies?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'Tell me about your experience with responsive design challenges.', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'How do you balance aesthetics with performance?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'Behavioral', 'Medium', 'Describe your approach to testing frontend applications.', NOW());

-- Frontend Developer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Frontend Developer', 'HR', 'Easy', 'Why did you choose frontend development?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'HR', 'Easy', 'What frontend frameworks do you prefer and why?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'HR', 'Easy', 'How do you approach learning new JavaScript frameworks?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'HR', 'Easy', 'What''s your design philosophy when building user interfaces?', NOW()),
(gen_random_uuid(), 'Frontend Developer', 'HR', 'Easy', 'How do you balance user experience with technical constraints?', NOW());

-- =====================================================
-- FULL STACK DEVELOPER (48 questions)
-- =====================================================

-- Full Stack Developer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'What does full stack development mean?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'Explain the client-server architecture.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'What is the difference between frontend and backend?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'What is a database and why do we need it?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'Explain what HTTP and HTTPS are.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'What is the purpose of a web server?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'What is deployment and why is it important?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'Explain what responsive web design means.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'What is the role of DNS in web applications?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Easy', 'What is the difference between development and production environments?', NOW());

-- Full Stack Developer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you design a full stack application architecture?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'Explain how frontend and backend communicate in a web application.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'What is your approach to testing full stack applications?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you handle user authentication across frontend and backend?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'Describe your experience with different database technologies.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you manage state across frontend and backend?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'Explain the difference between server-side and client-side rendering.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you implement real-time features in full stack applications?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'What is your approach to API design?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you handle error management across the full stack?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'Explain the concept of containerization and its benefits.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you implement file upload and download functionality?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'What is your approach to database schema design?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'How do you ensure security across the entire application stack?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Medium', 'Explain the concept of middleware and its uses.', NOW());

-- Full Stack Developer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'How would you architect a scalable full stack application?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'Describe your approach to implementing microservices with a frontend.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'How would you optimize the performance of an entire application stack?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'Explain strategies for handling large-scale data synchronization.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'How would you implement a real-time collaborative application?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'Describe your approach to building a multi-tenant application.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'How would you implement comprehensive monitoring across the stack?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'Explain strategies for managing database migrations in production.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'How would you design a system for handling offline functionality?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Technical', 'Hard', 'Describe your approach to implementing CI/CD for full stack applications.', NOW());

-- Full Stack Developer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'Tell me about a challenging full stack project you completed.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'How do you prioritize between frontend and backend tasks?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'Describe a time when you had to learn both frontend and backend technologies quickly.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'How do you handle context switching between different parts of the stack?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'Tell me about a time you optimized performance across the entire stack.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'How do you approach debugging issues that span multiple layers?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'Describe your experience with end-to-end feature development.', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'Behavioral', 'Medium', 'How do you stay current with technologies across the full stack?', NOW());

-- Full Stack Developer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Full Stack Developer', 'HR', 'Easy', 'What do you enjoy about full stack development?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'HR', 'Easy', 'Do you prefer frontend or backend development, and why?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'HR', 'Easy', 'How do you manage the breadth of knowledge required for full stack development?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'HR', 'Easy', 'What''s your ideal tech stack for building applications?', NOW()),
(gen_random_uuid(), 'Full Stack Developer', 'HR', 'Easy', 'How do you balance depth and breadth in your technical skills?', NOW());

-- =====================================================
-- DATA SCIENTIST (48 questions)
-- =====================================================

-- Data Scientist - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'What is the difference between supervised and unsupervised learning?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'Explain what a dataset is and its components.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'What is the purpose of data visualization?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'Explain what mean, median, and mode are.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'What is the difference between correlation and causation?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'What is a confusion matrix?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'Explain what regression analysis is used for.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'What is the purpose of training and test sets?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'What does data preprocessing involve?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Easy', 'Explain what a feature is in machine learning.', NOW());

-- Data Scientist - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'What is overfitting and how do you prevent it?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Describe the process of exploratory data analysis.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'How do you handle missing data in a dataset?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Explain the bias-variance tradeoff.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'What is cross-validation and why is it important?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Describe the difference between bagging and boosting.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'How do you choose the right machine learning algorithm?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Explain the concept of regularization in machine learning.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'What are precision and recall, and when do you prioritize each?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'How do you handle imbalanced datasets?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Explain the difference between L1 and L2 regularization.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'What is feature engineering and why is it important?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Describe the random forest algorithm and its advantages.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'How do you evaluate the performance of a classification model?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Medium', 'Explain what dimensionality reduction is and when to use it.', NOW());

-- Data Scientist - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How would you build a recommendation system from scratch?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'Explain your approach to feature engineering for complex datasets.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How do you evaluate the performance of a machine learning model in production?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'Describe strategies for handling highly imbalanced datasets.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How would you deploy a machine learning model to production?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'Explain your approach to A/B testing and experiment design.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How would you build a real-time prediction system?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'Describe techniques for handling high-dimensional data.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'How would you implement model monitoring and retraining pipelines?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Technical', 'Hard', 'Explain strategies for interpretable machine learning models.', NOW());

-- Data Scientist - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'Describe a time when your analysis led to an important business decision.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'How do you communicate complex technical concepts to non-technical stakeholders?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'Tell me about a challenging data problem you solved.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'How do you approach projects with unclear requirements?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'Describe a time when your model didn''t perform as expected.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'How do you balance model complexity with interpretability?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'Tell me about your experience collaborating with engineering teams.', NOW()),
(gen_random_uuid(), 'Data Scientist', 'Behavioral', 'Medium', 'How do you stay current with new data science techniques?', NOW());

-- Data Scientist - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Scientist', 'HR', 'Easy', 'Why do you want to be a data scientist?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'HR', 'Easy', 'What data science tools and libraries are you most proficient in?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'HR', 'Easy', 'How do you approach learning new statistical techniques?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'HR', 'Easy', 'What type of data science problems interest you most?', NOW()),
(gen_random_uuid(), 'Data Scientist', 'HR', 'Easy', 'How do you balance business impact with technical excellence?', NOW());

-- =====================================================
-- DATA ANALYST (48 questions)
-- =====================================================

-- Data Analyst - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'What is the purpose of data analysis?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'Explain what SQL is used for.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'What is the difference between a bar chart and a histogram?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'What is data cleaning and why is it important?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'Explain what a pivot table is.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'What is the purpose of data visualization?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'What is the difference between qualitative and quantitative data?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'Explain what a database query is.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'What are KPIs and why are they important?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Easy', 'What is the purpose of Excel in data analysis?', NOW());

-- Data Analyst - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you approach data cleaning and preparation?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'What tools do you use for data visualization?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'Explain how you would perform A/B testing analysis.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you identify trends and patterns in data?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'Describe your experience with SQL for data analysis.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you handle outliers in your analysis?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'Explain the concept of data modeling.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you validate the accuracy of your analysis?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'What is your approach to creating dashboards?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you work with large datasets efficiently?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'Explain the difference between correlation and regression.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you determine which metrics to track?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'Describe your process for exploratory data analysis.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'How do you ensure data quality in your reports?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Medium', 'What statistical methods do you commonly use?', NOW());

-- Data Analyst - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'How would you design a comprehensive analytics framework for a business?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'Explain your approach to predictive analytics.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'How would you identify and communicate data-driven insights?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'Describe strategies for automating repetitive analysis tasks.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'How would you build a data pipeline for real-time analytics?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'Explain your approach to cohort analysis and customer segmentation.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'How would you measure the ROI of a marketing campaign?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'Describe techniques for analyzing time series data.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'How would you design an experiment to test a business hypothesis?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Technical', 'Hard', 'Explain strategies for handling conflicting data sources.', NOW());

-- Data Analyst - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'How do you communicate technical findings to non-technical stakeholders?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'Tell me about a time your analysis changed a business decision.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'How do you prioritize multiple analysis requests?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'Describe a situation where you had to work with incomplete data.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'How do you handle requests for analysis that may not be feasible?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'Tell me about a complex analysis project you completed.', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'How do you ensure your reports are actionable?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'Behavioral', 'Medium', 'Describe your experience presenting findings to executives.', NOW());

-- Data Analyst - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Data Analyst', 'HR', 'Easy', 'What attracted you to data analysis?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'HR', 'Easy', 'What analytics tools are you most comfortable with?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'HR', 'Easy', 'How do you stay current with data analysis trends?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'HR', 'Easy', 'What type of business problems do you enjoy analyzing?', NOW()),
(gen_random_uuid(), 'Data Analyst', 'HR', 'Easy', 'How do you balance technical accuracy with business needs?', NOW());

-- =====================================================
-- DEVOPS ENGINEER (48 questions)
-- =====================================================

-- DevOps Engineer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'What is DevOps and why is it important?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'Explain what CI/CD stands for.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'What is version control and why do we use it?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'What is the purpose of a container?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'Explain what automation means in DevOps.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'What is the difference between deployment and release?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'What is monitoring and why is it important?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'Explain what a build pipeline is.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'What is infrastructure and why do we manage it as code?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Easy', 'What is the purpose of logging in production systems?', NOW());

-- DevOps Engineer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'Explain the concept of CI/CD in detail.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is Docker and why is it useful?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'How does Kubernetes work?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is infrastructure as code and its benefits?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'Explain the purpose of monitoring and logging in production systems.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'How do you implement automated testing in CI/CD?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is the difference between horizontal and vertical scaling?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'Explain how you would set up a deployment pipeline.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is container orchestration and why is it needed?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'How do you manage secrets and sensitive data?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'Explain the concept of blue-green deployment.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is the purpose of a reverse proxy?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'How do you implement backup and disaster recovery?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'Explain the difference between IaaS, PaaS, and SaaS.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Medium', 'What is your approach to incident management?', NOW());

-- DevOps Engineer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How would you design a zero-downtime deployment strategy?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'Describe your approach to managing secrets in a cloud environment.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How would you implement disaster recovery for a critical system?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'Explain how you would set up auto-scaling for a web application.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How do you handle security in a DevOps pipeline?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'Design a comprehensive monitoring and alerting strategy.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How would you implement multi-region deployment?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'Explain strategies for optimizing cloud infrastructure costs.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'How would you design a self-healing infrastructure?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Technical', 'Hard', 'Describe your approach to implementing chaos engineering.', NOW());

-- DevOps Engineer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'How do you handle production incidents?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'Tell me about a time you improved system reliability.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'How do you balance speed of delivery with system stability?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'Describe a challenging deployment you managed.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'How do you collaborate with development teams?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'Tell me about a time you automated a manual process.', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'How do you approach post-mortem analysis?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'Behavioral', 'Medium', 'Describe your experience with infrastructure optimization.', NOW());

-- DevOps Engineer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'DevOps Engineer', 'HR', 'Easy', 'What motivates you in DevOps work?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'HR', 'Easy', 'What DevOps tools are you most experienced with?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'HR', 'Easy', 'How do you stay current with DevOps practices?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'HR', 'Easy', 'What do you find most challenging about DevOps?', NOW()),
(gen_random_uuid(), 'DevOps Engineer', 'HR', 'Easy', 'How do you approach on-call responsibilities?', NOW());

-- =====================================================
-- CLOUD ENGINEER (48 questions)
-- =====================================================

-- Cloud Engineer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'What is cloud computing?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'Name the three major cloud providers.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'What is the difference between public and private cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'Explain what a virtual machine is.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'What is object storage in cloud computing?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'What does scalability mean in cloud context?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'What is a region in cloud computing?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'Explain what pay-as-you-go pricing means.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'What is the purpose of a load balancer?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Easy', 'What is cloud migration?', NOW());

-- Cloud Engineer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What are the benefits of cloud computing?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'Explain the difference between IaaS, PaaS, and SaaS.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What is serverless computing and its use cases?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'How do you ensure high availability in the cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What is a VPC in cloud networking?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'Explain cloud security best practices.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What is auto-scaling and how does it work?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'How do you implement backup strategies in the cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'Explain the concept of cloud-native applications.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What are availability zones and why are they important?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'How do you manage cloud costs effectively?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What is the difference between containers and virtual machines?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'Explain content delivery networks and their benefits.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'How do you implement identity and access management?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Medium', 'What is infrastructure as code in cloud context?', NOW());

-- Cloud Engineer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you design a multi-region cloud architecture?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'Explain your approach to cloud cost optimization.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you implement a disaster recovery strategy in the cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'Describe how you would secure cloud infrastructure comprehensively.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you migrate a legacy application to the cloud?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'Design a hybrid cloud architecture for an enterprise.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you implement cloud governance at scale?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'Explain strategies for optimizing network performance in cloud.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'How would you design a multi-tenant cloud application?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Technical', 'Hard', 'Describe your approach to implementing compliance in cloud environments.', NOW());

-- Cloud Engineer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'Tell me about a time you optimized cloud costs.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'How do you handle cloud outages and incidents?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'Describe a complex cloud migration you led.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'How do you stay updated with cloud platform changes?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'Tell me about a security issue you resolved in the cloud.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'How do you prioritize cloud infrastructure improvements?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'Describe your experience with multi-cloud strategies.', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'Behavioral', 'Medium', 'How do you collaborate with application teams on cloud adoption?', NOW());

-- Cloud Engineer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Cloud Engineer', 'HR', 'Easy', 'Why are you interested in cloud engineering?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'HR', 'Easy', 'Which cloud platform do you prefer and why?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'HR', 'Easy', 'What cloud certifications do you hold?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'HR', 'Easy', 'How do you approach learning new cloud services?', NOW()),
(gen_random_uuid(), 'Cloud Engineer', 'HR', 'Easy', 'What excites you most about cloud technology?', NOW());

-- =====================================================
-- MACHINE LEARNING ENGINEER (48 questions)
-- =====================================================

-- Machine Learning Engineer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'What is machine learning?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'Explain the difference between AI and machine learning.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'What is a training dataset?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'What is the purpose of a test set?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'Explain what a neural network is in simple terms.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'What is supervised learning?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'What does model training mean?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'What is a hyperparameter?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'Explain what accuracy means in ML models.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Easy', 'What is the purpose of feature extraction?', NOW());

-- Machine Learning Engineer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'What is the difference between classification and regression?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'Explain gradient descent and its variants.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'What is backpropagation in neural networks?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'How do you handle overfitting in machine learning models?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'Explain the concept of transfer learning.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'What are convolutional neural networks used for?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'How do you choose appropriate evaluation metrics?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'Explain the difference between batch and online learning.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'What is ensemble learning and when is it useful?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'How do you handle imbalanced datasets in ML?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'Explain the concept of regularization in ML.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'What is cross-validation and why is it important?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'How do you perform hyperparameter tuning?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'Explain the bias-variance tradeoff in ML.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Medium', 'What are recurrent neural networks used for?', NOW());

-- Machine Learning Engineer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How would you design an ML pipeline from data collection to deployment?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'Explain your approach to model versioning and experiment tracking.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How do you handle model drift in production?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'Describe how you would implement real-time ML predictions.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How would you optimize ML model performance for production?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'Explain strategies for scaling ML training infrastructure.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How would you implement A/B testing for ML models?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'Describe techniques for model interpretability and explainability.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'How would you design a recommendation system at scale?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Technical', 'Hard', 'Explain your approach to handling data quality issues in ML.', NOW());

-- Machine Learning Engineer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'How do you approach solving complex ML problems?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'Tell me about a challenging ML project you completed.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'How do you balance model performance with production constraints?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'Describe a time when your model failed in production.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'How do you collaborate with data scientists and engineers?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'Tell me about your experience with ML model debugging.', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'How do you stay current with ML research and techniques?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'Behavioral', 'Medium', 'Describe your approach to communicating ML results to stakeholders.', NOW());

-- Machine Learning Engineer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Machine Learning Engineer', 'HR', 'Easy', 'What excites you about machine learning?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'HR', 'Easy', 'What ML frameworks are you most proficient in?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'HR', 'Easy', 'How do you approach learning new ML techniques?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'HR', 'Easy', 'What type of ML problems interest you most?', NOW()),
(gen_random_uuid(), 'Machine Learning Engineer', 'HR', 'Easy', 'How do you balance research and engineering in your work?', NOW());

-- =====================================================
-- PRODUCT MANAGER (48 questions)
-- =====================================================

-- Product Manager - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'What does a product manager do?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'What is a product roadmap?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'Explain what user stories are.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'What is an MVP?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'What are product requirements?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'Explain what a sprint is in agile development.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'What is user feedback and why is it important?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'What is product-market fit?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'Explain what A/B testing is.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Easy', 'What are OKRs?', NOW());

-- Product Manager - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you prioritize features in a product roadmap?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'Explain your approach to product discovery.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you measure product success?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'What frameworks do you use for product strategy?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you work with engineering teams effectively?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'Explain how you conduct user research.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you balance stakeholder requests with product vision?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'What is your approach to competitive analysis?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you define and track product metrics?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'Explain the difference between product strategy and product tactics.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you handle feature requests from customers?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'What is your process for writing product requirements?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you manage technical debt discussions with engineering?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'Explain your approach to product launches.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Medium', 'How do you validate product ideas before building?', NOW());

-- Product Manager - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'How would you build a product strategy from scratch?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'Describe your approach to entering a new market.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'How would you handle a failing product?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'Explain strategies for scaling a product globally.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'How would you decide whether to build, buy, or partner?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'Describe your approach to platform vs. product decisions.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'How would you manage a product portfolio?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'Explain strategies for product differentiation in competitive markets.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'How would you transition a product to a new business model?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Technical', 'Hard', 'Describe your approach to sunset a product or feature.', NOW());

-- Product Manager - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'Tell me about a product you launched successfully.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'How do you handle conflicting stakeholder requirements?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'Describe a time when you had to pivot product direction.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'How do you build consensus among diverse teams?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'Tell me about a time when customer feedback changed your roadmap.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'How do you handle disagreements with engineering about feasibility?', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'Describe your experience with a product that didn''t meet expectations.', NOW()),
(gen_random_uuid(), 'Product Manager', 'Behavioral', 'Medium', 'How do you balance short-term wins with long-term strategy?', NOW());

-- Product Manager - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Product Manager', 'HR', 'Easy', 'What attracts you to product management?', NOW()),
(gen_random_uuid(), 'Product Manager', 'HR', 'Easy', 'How do you stay updated with market trends?', NOW()),
(gen_random_uuid(), 'Product Manager', 'HR', 'Easy', 'What product management frameworks do you prefer?', NOW()),
(gen_random_uuid(), 'Product Manager', 'HR', 'Easy', 'How do you define product success?', NOW()),
(gen_random_uuid(), 'Product Manager', 'HR', 'Easy', 'What''s your approach to continuous learning in product management?', NOW());

-- =====================================================
-- MARKETING MANAGER (48 questions)
-- =====================================================

-- Marketing Manager - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'What is digital marketing?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'Explain what SEO stands for.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'What is a marketing campaign?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'What is the purpose of market research?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'Explain what a target audience is.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'What is brand awareness?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'What is content marketing?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'Explain what conversion rate means.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'What is social media marketing?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Easy', 'What is email marketing?', NOW());

-- Marketing Manager - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you develop a marketing strategy?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'What metrics do you use to measure campaign success?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'Explain your approach to digital marketing.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you identify target audiences?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'What tools do you use for marketing analytics?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you optimize marketing funnels?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'Explain the concept of customer segmentation.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you measure ROI on marketing campaigns?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'What is your approach to brand positioning?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you integrate different marketing channels?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'Explain your experience with marketing automation.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you conduct competitive analysis?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'What is your approach to content strategy?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'How do you optimize for search engines?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Medium', 'Explain the customer journey and how you influence it.', NOW());

-- Marketing Manager - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'How would you build a marketing strategy for a new product launch?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'Describe your approach to brand repositioning.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'How would you handle a marketing crisis?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'Explain strategies for entering a new market segment.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'How would you optimize marketing spend across channels?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'Describe your approach to building a marketing team.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'How would you implement data-driven marketing?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'Explain strategies for scaling marketing operations globally.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'How would you measure brand equity?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Technical', 'Hard', 'Describe your approach to customer lifetime value optimization.', NOW());

-- Marketing Manager - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'Describe a successful marketing campaign you led.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'How do you handle budget constraints?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'Tell me about a time a campaign didn''t perform as expected.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'How do you collaborate with sales teams?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'Describe your experience with rebranding initiatives.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'How do you prioritize multiple marketing initiatives?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'Tell me about a time you identified a new market opportunity.', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'Behavioral', 'Medium', 'How do you handle creative differences with your team?', NOW());

-- Marketing Manager - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Marketing Manager', 'HR', 'Easy', 'Why do you want to work in marketing?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'HR', 'Easy', 'What marketing trends excite you most?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'HR', 'Easy', 'What marketing tools are you most proficient in?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'HR', 'Easy', 'How do you stay current with marketing trends?', NOW()),
(gen_random_uuid(), 'Marketing Manager', 'HR', 'Easy', 'What''s your philosophy on data-driven marketing?', NOW());

-- =====================================================
-- UX DESIGNER (48 questions)
-- =====================================================

-- UX Designer - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What does UX stand for?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is the difference between UX and UI?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is a wireframe?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is a prototype?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'Explain what user research is.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is usability testing?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is a user persona?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is information architecture?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is accessibility in UX design?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Easy', 'What is a user journey map?', NOW());

-- UX Designer - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'Explain your UX design process from start to finish.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you conduct user research?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'What is your approach to creating user personas?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you measure UX success?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'Explain the difference between qualitative and quantitative research.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you prioritize design improvements?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'What is your approach to responsive design?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you handle accessibility requirements?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'Explain your process for creating user flows.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you conduct usability testing?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'What is your approach to design systems?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you collaborate with developers?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'Explain the concept of design thinking.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'How do you balance user needs with business goals?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Medium', 'What tools do you use for UX design and why?', NOW());

-- UX Designer - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'How would you redesign a poorly performing product?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'Describe your approach to designing for multiple platforms.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'How would you establish a design system from scratch?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'Explain strategies for conducting research with limited resources.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'How would you design for international audiences?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'Describe your approach to complex enterprise UX challenges.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'How would you measure the ROI of UX improvements?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'Explain strategies for scaling UX practices across an organization.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'How would you design for emerging technologies?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Technical', 'Hard', 'Describe your approach to ethical design considerations.', NOW());

-- UX Designer - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'Tell me about a design challenge you solved.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'How do you handle feedback on your designs?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'Describe a time when user research changed your design approach.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'How do you handle disagreements with stakeholders about design?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'Tell me about a design that didn''t test well with users.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'How do you advocate for users in product decisions?', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'Describe your experience with cross-functional collaboration.', NOW()),
(gen_random_uuid(), 'UX Designer', 'Behavioral', 'Medium', 'How do you stay inspired and current in UX design?', NOW());

-- UX Designer - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'UX Designer', 'HR', 'Easy', 'What inspired you to become a UX designer?', NOW()),
(gen_random_uuid(), 'UX Designer', 'HR', 'Easy', 'What design tools do you prefer and why?', NOW()),
(gen_random_uuid(), 'UX Designer', 'HR', 'Easy', 'How do you approach learning new design techniques?', NOW()),
(gen_random_uuid(), 'UX Designer', 'HR', 'Easy', 'What aspects of UX design do you find most rewarding?', NOW()),
(gen_random_uuid(), 'UX Designer', 'HR', 'Easy', 'How do you balance creativity with user research data?', NOW());

-- =====================================================
-- BUSINESS ANALYST (48 questions)
-- =====================================================

-- Business Analyst - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'What does a business analyst do?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'What are business requirements?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'Explain what a use case is.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'What is a stakeholder?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'What is the purpose of requirements gathering?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'Explain what a process flow diagram is.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'What is a business case?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'What is gap analysis?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'Explain what SWOT analysis stands for.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Easy', 'What is data analysis in business context?', NOW());

-- Business Analyst - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you gather business requirements effectively?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'Explain your approach to business process analysis.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'What tools do you use for data analysis and why?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you create effective business cases?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'Describe your experience with stakeholder management.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you prioritize competing requirements?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'Explain your approach to process improvement.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you validate requirements with stakeholders?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'What is your experience with agile methodologies?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you document complex business processes?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'Explain the concept of business process modeling.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you handle changing requirements?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'What is your approach to feasibility analysis?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'How do you ensure requirements traceability?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Medium', 'Explain your experience with data modeling.', NOW());

-- Business Analyst - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'How would you analyze and optimize end-to-end business processes?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'Describe your approach to enterprise-wide transformation projects.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'How would you build a business intelligence strategy?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'Explain strategies for managing complex stakeholder relationships.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'How would you assess the ROI of a major system implementation?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'Describe your approach to change management in organizations.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'How would you design a requirements framework for large projects?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'Explain strategies for bridging business and technical teams.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'How would you handle conflicting business objectives?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Technical', 'Hard', 'Describe your approach to building a data-driven organization.', NOW());

-- Business Analyst - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'Tell me about a business problem you helped solve.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'How do you handle ambiguous requirements?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'Describe a time when stakeholders disagreed on requirements.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'How do you manage difficult stakeholder relationships?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'Tell me about a successful process improvement you led.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'How do you handle scope creep in projects?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'Describe your experience facilitating workshops or meetings.', NOW()),
(gen_random_uuid(), 'Business Analyst', 'Behavioral', 'Medium', 'How do you communicate technical concepts to business users?', NOW());

-- Business Analyst - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Business Analyst', 'HR', 'Easy', 'Why did you choose business analysis?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'HR', 'Easy', 'What aspects of business analysis do you enjoy most?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'HR', 'Easy', 'What business analysis tools are you proficient in?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'HR', 'Easy', 'How do you stay current with business analysis practices?', NOW()),
(gen_random_uuid(), 'Business Analyst', 'HR', 'Easy', 'What certifications do you hold or are pursuing?', NOW());

-- =====================================================
-- SALES EXECUTIVE (48 questions)
-- =====================================================

-- Sales Executive - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'What is a sales funnel?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'Explain what a lead is in sales.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'What is cold calling?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'What is a sales quota?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'Explain what closing means in sales.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'What is a sales pipeline?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'What is prospecting?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'Explain what a sales pitch is.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'What is customer relationship management?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Easy', 'What is upselling?', NOW());

-- Sales Executive - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'Describe your sales process from lead to close.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you qualify leads effectively?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'What CRM tools have you used and how?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you handle objections from prospects?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'Explain your approach to closing deals.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you build rapport with potential customers?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'What is your strategy for pipeline management?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you research prospects before reaching out?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'Explain the SPIN selling methodology.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you maintain customer relationships post-sale?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'What metrics do you track to measure sales performance?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you adapt your sales approach to different customers?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'Explain your process for negotiating deals.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'How do you leverage social selling?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Medium', 'What is your approach to account-based selling?', NOW());

-- Sales Executive - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'How would you develop a sales strategy for a new market?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'Describe your approach to enterprise sales.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'How would you build and manage a sales team?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'Explain strategies for penetrating competitive markets.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'How would you handle a significant deal that''s stalled?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'Describe your approach to complex multi-stakeholder sales.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'How would you turn around underperforming sales territories?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'Explain strategies for scaling sales operations.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'How would you implement a sales enablement program?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Technical', 'Hard', 'Describe your approach to strategic account management.', NOW());

-- Sales Executive - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'Tell me about your biggest sales achievement.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'How do you handle rejection in sales?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'Describe a time you lost a deal and what you learned.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'How do you maintain motivation during slow periods?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'Tell me about a complex sale you closed.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'How do you build long-term customer relationships?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'Describe a time you exceeded your sales targets.', NOW()),
(gen_random_uuid(), 'Sales Executive', 'Behavioral', 'Medium', 'How do you handle difficult customers or negotiations?', NOW());

-- Sales Executive - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Sales Executive', 'HR', 'Easy', 'What motivates you in sales?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'HR', 'Easy', 'Why do you want to work in sales?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'HR', 'Easy', 'What is your ideal sales environment?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'HR', 'Easy', 'How do you define sales success?', NOW()),
(gen_random_uuid(), 'Sales Executive', 'HR', 'Easy', 'What sales methodologies are you trained in?', NOW());

-- =====================================================
-- CUSTOMER SUCCESS MANAGER (48 questions)
-- =====================================================

-- Customer Success Manager - Technical - Easy (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is customer success?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is customer retention?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'Explain what churn means.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is customer onboarding?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is customer satisfaction?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is NPS (Net Promoter Score)?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is customer lifetime value?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'Explain what an escalation is.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is a product adoption?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Easy', 'What is customer health scoring?', NOW());

-- Customer Success Manager - Technical - Medium (15)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you ensure customer satisfaction?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'Describe your approach to onboarding new customers.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you measure customer success?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'What strategies do you use to reduce churn?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you handle customer escalations?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'Explain your approach to customer health monitoring.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you drive product adoption?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'What is your process for customer check-ins?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you identify expansion opportunities?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'Explain your approach to customer education and training.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you gather and act on customer feedback?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'What tools do you use for customer success management?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you prioritize customer needs?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'Explain your approach to customer segmentation.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Medium', 'How do you collaborate with sales and support teams?', NOW());

-- Customer Success Manager - Technical - Hard (10)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'How would you build a customer success program from scratch?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'Describe your approach to strategic account management.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'How would you handle a major customer at risk of churning?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'Explain strategies for scaling customer success operations.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'How would you measure the ROI of customer success initiatives?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'Describe your approach to building customer advocacy programs.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'How would you design a customer success playbook?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'Explain strategies for managing high-value enterprise customers.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'How would you implement predictive churn modeling?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Technical', 'Hard', 'Describe your approach to customer journey mapping and optimization.', NOW());

-- Customer Success Manager - Behavioral (8)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'Tell me about a time you turned around a dissatisfied customer.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'How do you build relationships with customers?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'Describe a situation where you prevented customer churn.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'How do you handle difficult customer situations?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'Tell me about a successful upsell or expansion you achieved.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'How do you manage multiple customer accounts simultaneously?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'Describe your experience implementing customer feedback into products.', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'Behavioral', 'Medium', 'How do you balance customer needs with company limitations?', NOW());

-- Customer Success Manager - HR (5)
INSERT INTO "QuestionBanks" ("Id", "Role", "Category", "Difficulty", "QuestionText", "CreatedAt") VALUES
(gen_random_uuid(), 'Customer Success Manager', 'HR', 'Easy', 'Why are you interested in customer success?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'HR', 'Easy', 'What do you enjoy about helping customers?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'HR', 'Easy', 'What customer success tools are you proficient in?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'HR', 'Easy', 'How do you define customer success?', NOW()),
(gen_random_uuid(), 'Customer Success Manager', 'HR', 'Easy', 'What motivates you in a customer-facing role?', NOW());

-- =====================================================
-- Verification Query
-- =====================================================
-- SELECT "Role", "Category", "Difficulty", COUNT(*) as QuestionCount
-- FROM "QuestionBanks"
-- GROUP BY "Role", "Category", "Difficulty"
-- ORDER BY "Role", "Category", "Difficulty";
--
-- Expected Total: 672 questions (14 roles × 48 questions)
