using CommunicaAI.Models;

namespace CommunicaAI.Data
{
    public static class CompanyProfileSeeder
    {
        public static async Task SeedCompanyProfilesAsync(ApplicationDbContext context)
        {
            if (context.CompanyProfiles.Any())
            {
                return; // Already seeded
            }

            var companies = new List<CompanyProfile>
            {
                new CompanyProfile
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Google",
                    InterviewStyle = "Highly technical with emphasis on algorithmic thinking and system design. Behavioral questions focus on Googleyness and leadership.",
                    FocusAreas = "Data Structures; Algorithms; System Design; Problem Solving; Scalability; Code Quality",
                    BehavioralExpectations = "Demonstrate collaboration, innovation, and intellectual humility. Show ability to work in ambiguous situations and drive projects forward.",
                    TechnicalExpectations = "Strong CS fundamentals, optimal solutions to complex problems, ability to scale systems to billions of users, clean and maintainable code.",
                    CommunicationExpectations = "Clear articulation of technical decisions, ability to explain complex concepts simply, good listening skills, and receptiveness to feedback.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Amazon",
                    InterviewStyle = "Leadership Principles-driven with STAR method behavioral questions. Technical rounds focus on practical problem-solving and system design.",
                    FocusAreas = "Customer Obsession; Ownership; Bias for Action; Data Structures; System Design; Scalability",
                    BehavioralExpectations = "Demonstrate Amazon's 16 Leadership Principles through specific examples. Show customer focus, ownership mindset, and ability to deliver results.",
                    TechnicalExpectations = "Practical problem-solving skills, ability to handle ambiguity, focus on delivering working solutions, scalability considerations, and cost optimization.",
                    CommunicationExpectations = "Structured responses using STAR method, clear problem breakdown, ability to discuss trade-offs, and strong written communication skills.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Microsoft",
                    InterviewStyle = "Balanced approach combining technical depth with cultural fit assessment. Focus on growth mindset and collaboration.",
                    FocusAreas = "Problem Solving; System Design; Collaboration; Growth Mindset; Azure/Cloud Technologies; .NET/C#",
                    BehavioralExpectations = "Show growth mindset, respect for diversity, passion for technology, and ability to work in teams. Demonstrate learning from failures.",
                    TechnicalExpectations = "Strong technical foundation, ability to design scalable systems, understanding of cloud architecture, clean code practices, and testing.",
                    CommunicationExpectations = "Collaborative communication style, active listening, ability to give and receive constructive feedback, clear technical explanations.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Meta (Facebook)",
                    InterviewStyle = "High-intensity technical interviews with focus on speed and efficiency. Behavioral rounds assess cultural alignment with Meta's values.",
                    FocusAreas = "Algorithms; Data Structures; System Design; Performance Optimization; Mobile Development; Product Thinking",
                    BehavioralExpectations = "Show boldness, focus on impact, move fast mentality, and ability to build social products. Demonstrate user empathy and product sense.",
                    TechnicalExpectations = "Fast problem-solving, optimal solutions under time pressure, scalability for billions of users, performance optimization, and mobile-first thinking.",
                    CommunicationExpectations = "Quick and clear communication, ability to think out loud, defend technical decisions, and discuss product implications of technical choices.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Startup (Fast-paced)",
                    InterviewStyle = "Practical and hands-on with focus on versatility. Less structured, more conversational. Emphasis on cultural fit and adaptability.",
                    FocusAreas = "Full-stack capabilities; Rapid prototyping; Product mindset; Versatility; Resourcefulness; Startup experience",
                    BehavioralExpectations = "Show scrappiness, ability to wear multiple hats, comfort with ambiguity, self-starter mentality, and passion for the mission.",
                    TechnicalExpectations = "Breadth over depth, ability to ship quickly, pragmatic technical decisions, full-stack capabilities, and willingness to learn new technologies.",
                    CommunicationExpectations = "Direct and informal communication, ability to work with minimal supervision, proactive updates, and collaborative problem-solving.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.CompanyProfiles.AddRange(companies);
            await context.SaveChangesAsync();
        }
    }
}
