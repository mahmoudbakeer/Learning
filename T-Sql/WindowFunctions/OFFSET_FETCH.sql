USE C21_DB1;
GO

-- Variables for pagination
DECLARE @RowsPerPage INT;    -- Number of rows to display per page
DECLARE @PageNumber INT;     -- Page number to display

SET @RowsPerPage = 3;        -- e.g., 3 rows per page
SET @PageNumber = 2;         -- e.g., page 2

-- Query to fetch paginated results
SELECT 
    StudentID,
    Name,
    Subject,
    Grade
FROM Students
ORDER BY StudentID   -- ⚠️ ORDER BY is mandatory when using OFFSET/FETCH
OFFSET (@PageNumber - 1) * @RowsPerPage ROWS  -- Skip rows of previous pages
FETCH NEXT @RowsPerPage ROWS ONLY;            -- Fetch only the number of rows for this page