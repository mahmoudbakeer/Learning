USE Shop_Database;
GO

-- ============================================================================
-- SETUP: CREATING THE HIERARCHICAL TABLE
-- ============================================================================

---- 1. Create the table
--CREATE TABLE Employees (
--    EmployeeID INT PRIMARY KEY,
--    FirstName VARCHAR(50),
--    LastName VARCHAR(50),
--    Age INT,
--    ManagerID INT NULL, -- NULL allows for a top-level boss (CEO) who has no manager
    
--    -- The crucial part for hierarchy: a foreign key referencing its own table
--    CONSTRAINT FK_Manager FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID)
--);
--GO

---- 2. Insert sample data to build our "Org Chart"
---- Level 1 (The Boss)
--INSERT INTO Employees (EmployeeID, FirstName, LastName, Age, ManagerID) 
--VALUES (1, 'Alice', 'Smith', 55, NULL); 

---- Level 2 (Managers)
--INSERT INTO Employees (EmployeeID, FirstName, LastName, Age, ManagerID) 
--VALUES 
--(2, 'Bob', 'Johnson', 45, 1),    -- Reports to Alice
--(3, 'Charlie', 'Brown', 42, 1);  -- Reports to Alice

---- Level 3 (Regular Employees)
--INSERT INTO Employees (EmployeeID, FirstName, LastName, Age, ManagerID) 
--VALUES 
--(4, 'David', 'Williams', 30, 2), -- Reports to Bob
--(5, 'Eve', 'Davis', 28, 2),      -- Reports to Bob
--(6, 'Frank', 'Miller', 35, 3),   -- Reports to Charlie
--(7, 'Grace', 'Wilson', 32, 3);   -- Reports to Charlie
GO


-- ============================================================================
-- RECURSIVE CTE: MAPPING THE ORG CHART
-- ============================================================================
-- PURPOSE: Traverse the hierarchy from the top boss down to the lowest level,
--          calculating each employee's "Level" in the company.

WITH CTE_EMP_HIER AS
(
    -- 1. ANCHOR 
    SELECT 
        EmployeeID,
        FirstName,
        LastName,
        ManagerID, 
        1 AS Level 
    FROM Employees
    WHERE ManagerID IS NULL   --  Start only at the very top (The CEO)

    UNION ALL

    -- 2. THE LOOP
    SELECT 
        e.EmployeeID,         -- Added 'e.' to tell SQL which table to pull from
        e.FirstName,
        e.LastName,
        e.ManagerID, 
        ceh.Level + 1         -- Add 1 to the level from the previous CTE step
    FROM CTE_EMP_HIER ceh
    INNER JOIN Employees e 
        ON e.ManagerID = ceh.EmployeeID  -- Match the new employee's Manager to the CTE's Employee
)

-- 3. THE EXECUTION
SELECT * FROM CTE_EMP_HIER
ORDER BY Level, ManagerID;