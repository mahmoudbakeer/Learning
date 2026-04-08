USE C21_DB1;
GO

-- LAG and LEAD are window functions used to access data from **previous or next rows** 
-- relative to the current row without using self-joins.
-- Very useful for comparing a row with its neighbors (trends, differences, gaps).

SELECT 
    -- LAG returns the value of Grade from the **previous row** 
    -- according to the ORDER BY clause
    -- Syntax: LAG(column, offset) OVER (ORDER BY ...)
    LAG(Grade, 1) OVER (ORDER BY Grade DESC) AS PreviousGrade,  

    -- Current row's grade
    Grade,

    -- LEAD returns the value of Grade from the **next row**
    -- according to the ORDER BY clause
    -- Syntax: LEAD(column, offset) OVER (ORDER BY ...)
    LEAD(Grade, 1) OVER (ORDER BY Grade DESC) AS NextGrade

FROM Students;