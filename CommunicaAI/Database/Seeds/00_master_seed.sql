-- =====================================================
-- CommunicaAI - Master Seed Script
-- PostgreSQL Compatible
-- =====================================================
-- This script runs all seed files in the correct order
-- Execute this file to seed the entire database
-- =====================================================

-- Enable required PostgreSQL extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Display information
DO $$
BEGIN
    RAISE NOTICE 'Starting CommunicaAI database seeding...';
    RAISE NOTICE 'Timestamp: %', NOW();
END $$;

-- =====================================================
-- 1. Seed QuestionBanks Table
-- =====================================================
\echo 'Seeding QuestionBanks table...'
\i '01_seed_question_bank.sql'

-- =====================================================
-- Add more seed scripts here as needed
-- =====================================================
-- Example:
-- \i '02_seed_company_profiles.sql'
-- \i '03_seed_sample_users.sql'

-- =====================================================
-- Verification and Summary
-- =====================================================
\echo ''
\echo '====================================='
\echo 'Database Seeding Complete!'
\echo '====================================='
\echo ''

-- Display summary statistics
\echo 'Summary Statistics:'
\echo '-------------------'

SELECT 
    'QuestionBanks' as TableName,
    COUNT(*) as RecordCount
FROM "QuestionBanks";

\echo ''
\echo 'Questions by Role:'
SELECT "Role", COUNT(*) as Count
FROM "QuestionBanks"
GROUP BY "Role"
ORDER BY "Role";

\echo ''
\echo 'Questions by Difficulty:'
SELECT "Difficulty", COUNT(*) as Count
FROM "QuestionBanks"
GROUP BY "Difficulty"
ORDER BY "Difficulty";

\echo ''
\echo 'Questions by Category:'
SELECT "Category", COUNT(*) as Count
FROM "QuestionBanks"
GROUP BY "Category"
ORDER BY "Category";

\echo ''
\echo '====================================='
\echo 'Seeding completed successfully!'
\echo '====================================='
