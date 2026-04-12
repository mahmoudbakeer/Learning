USE Shop_Database;
GO

-- ============================================================================
-- RECURSIVE CTEs (Loops in SQL)
-- ============================================================================
-- USUALLY they are used when we have a hierarichal data
-- PURPOSE: Used to generate sequences (like numbers or dates) or to traverse 
--          hierarchical data (like Employee-to-Manager organizational charts).
-- RULES:   1. Must contain an "Anchor" member (the starting point).
--          2. Must use "UNION ALL" to combine rows.
--          3. Must contain a "Recursive" member that references the CTE itself 
--             and includes a termination condition (the breaking point).

-- ----------------------------------------------------------------------------
-- EXAMPLE 1: Simple Number Sequence (1 to 20)
-- ----------------------------------------------------------------------------
WITH CTE_Series AS (
    -- STEP 1: Anchor (The initial starting value)
    SELECT 1 AS Number

    UNION ALL

    -- STEP 2: The Loop & Breaking Point
    -- Adds 1 to the previous 'Number'. Stops when it hits 20.
    SELECT Number + 1 
    FROM CTE_Series 
    WHERE Number < 20
)
-- STEP 3: Execution
SELECT * FROM CTE_Series
-- Note: SQL Server limits recursion to 100 loops by default to prevent server crashes. 
-- To go higher (or set it to 0 for infinite), attach the OPTION clause right here:
-- OPTION (MAXRECURSION 200);


-- ----------------------------------------------------------------------------
-- EXAMPLE 2: Real-World Business Scenario (Generating a Calendar List)
-- ----------------------------------------------------------------------------
-- Data analysts frequently use recursive CTEs to generate a list of every 
-- single date in a month or year when building reports.
go
WITH CTE_Calendar AS (
    -- Anchor: Start on the first day of the month
    SELECT CAST('2024-01-01' AS DATE) AS CalendarDate
            
    UNION ALL
            
    -- Recursive Loop: Add exactly one day to the previous date
    SELECT DATEADD(day, 1, CalendarDate)
    FROM CTE_Calendar
    
    -- Breaking Point: Stop at the end of the month
    WHERE CalendarDate < '2024-01-31'
)
SELECT * FROM CTE_Calendar;