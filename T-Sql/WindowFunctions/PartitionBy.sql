USE C21_DB1;
GO

-- PARTITION BY groups rows logically (like GROUP BY)
-- but does NOT collapse them; it keeps all rows
-- Used when you want calculations per group while preserving row-level data

SELECT 
    StudentID, 
    Name, 
    Subject, 
    Grade,
    -- RANK assigns ranking within each Subject based on Grade (highest first)
    RANK() OVER (PARTITION BY Subject ORDER BY Grade DESC) AS RankInSubject
FROM Students;



-- Aggregate functions can be used with PARTITION BY
-- Without ORDER BY → returns total per group (same value repeated for each row in the group)

SELECT 
    StudentID, 
    Name, 
    Subject, 
    Grade,
    SUM(Grade) OVER (PARTITION BY Subject) AS SumGradesPerSubject
FROM Students;



-- If ORDER BY is added → the aggregate becomes a running (cumulative) calculation within the group

SELECT 
    StudentID, 
    Name, 
    Subject, 
    Grade,
    SUM(Grade) OVER (PARTITION BY Subject ORDER BY Grade) AS RunningSumPerSubject
FROM Students;