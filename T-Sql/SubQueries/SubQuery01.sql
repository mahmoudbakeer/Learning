USE Shop_Database;
GO

-- ============================================================================
-- 1. SUBQUERY IN THE 'FROM' CLAUSE (Derived Tables)
-- ============================================================================
-- PURPOSE: Acts as a temporary table that exists only for this query.
-- RULE: Must always have an alias (like 't' below).
-- SCENARIO: We want to calculate the total sales per customer, and then 
--           rank them from highest to lowest.
SELECT 
    CustomerID, 
    Sales, 
    RANK() OVER(ORDER BY Sales DESC) AS Ranking 
FROM (
    -- This inner query runs first and creates our temporary 't' table
    SELECT CustomerID, SUM(Amount) AS Sales 
    FROM Orders 
    GROUP BY CustomerID
) t;


-- ============================================================================
-- 2. SUBQUERY IN THE 'SELECT' CLAUSE (Scalar Subqueries)
-- ============================================================================
-- PURPOSE: Pulls in an isolated data point to display next to your main data.
-- RULE: MUST be a "Scalar" subquery (meaning it returns exactly ONE row 
--       and ONE column). If it returns multiple, the query crashes.
-- SCENARIO: We want to see every order, but also show the grand total 
--           number of orders in the company next to it for comparison.
SELECT 
    *, 
    (SELECT COUNT(*) FROM Orders) AS TotalCompanyOrders 
FROM Orders;


-- ============================================================================
-- 3. SUBQUERY IN THE 'JOIN' CLAUSE
-- ============================================================================
-- PURPOSE: Joins your main table to an aggregated temporary table.
-- SCENARIO: Get customer details alongside their total number of orders.
-- PRO TIP: Using a LEFT JOIN means customers with 0 orders still show up. 
--          Using COALESCE turns the 'NULL' order count into a clean '0'.
SELECT 
    c.*, 
    COALESCE(o.TotalOrders, 0) AS TotalOrders 
FROM Customers c
LEFT JOIN (
    SELECT CustomerID, COUNT(*) AS TotalOrders 
    FROM Orders 
    GROUP BY CustomerID 
) o 
ON c.CustomerID = o.CustomerID
ORDER BY TotalOrders DESC;


-- ============================================================================
-- 4. SUBQUERY IN THE 'WHERE' CLAUSE (Filtering via a single value)
-- ============================================================================
-- PURPOSE: Calculates a dynamic threshold to filter your main query.
-- RULE: Because we are using the '>' operator, the subquery must be scalar.
-- SCENARIO: Find any specific order that is larger than the overall average.
SELECT 
    OrderID, 
    Amount, 
    CustomerID 
FROM Orders 
WHERE Amount > (
    SELECT AVG(Amount) FROM Orders
);


-- ============================================================================
-- 5. THE 'IN' & 'NOT IN' OPERATORS (Filtering via a list)
-- ============================================================================
-- PURPOSE: Used when your subquery returns a column/list of multiple values.
-- SCENARIO: Find orders placed by anyone whose name contains the letter 'a'.
SELECT 
    OrderID, 
    Amount, 
    CustomerID 
FROM Orders 
WHERE CustomerID IN (
    -- This returns a list of IDs (e.g., 1, 4, 7). 
    -- The outer query checks if its ID matches any on that list.
    SELECT CustomerID FROM Customers WHERE Name LIKE '%a%'
) 
ORDER BY Amount;

-- NOT IN simply reverses the logic.
SELECT 
    OrderID, 
    Amount, 
    CustomerID 
FROM Orders 
WHERE CustomerID NOT IN (
    SELECT CustomerID FROM Customers WHERE Name LIKE '%a%'
) 
ORDER BY Amount;


-- ============================================================================
-- 6. CORRELATED SUBQUERIES (The "Loop")
-- ============================================================================
-- PURPOSE: A subquery that depends on the outer query to function.
-- RULE: It references a table from the outer query (e.g., 'c.CustomerID').
-- WARNING: These run row-by-row, so they can be slow on massive datasets.
-- SCENARIO: Get the customer details and count their specific orders.
SELECT 
    *, 
    (
        -- Notice 'c.CustomerID' inside the inner query. This makes it correlated!
        SELECT COUNT(*) 
        FROM Orders o 
        WHERE o.CustomerID = c.CustomerID
    ) AS TotalOrders 
FROM Customers c 
ORDER BY TotalOrders DESC;


-- ============================================================================
-- 7. EXISTS & NOT EXISTS (Boolean Checking)
-- ============================================================================
-- PURPOSE: Highly optimized way to check if a related record exists. 
-- RULE: Used almost exclusively with Correlated Subqueries. Returns True/False.
-- SCENARIO: Find orders placed by customers with an 'a' in their name.
SELECT 
    OrderID, 
    Amount, 
    CustomerID 
FROM Orders o 
WHERE EXISTS (
    -- 'SELECT 1' is standard practice here. We don't care about the actual data,
    -- we just want to know if a row simply *exists* that matches the conditions.
    SELECT 1 
    FROM Customers c 
    WHERE c.Name LIKE '%a%' AND c.CustomerID = o.CustomerID
) 
ORDER BY Amount;


-- ============================================================================
-- 8. THE 'ANY' & 'ALL' OPERATORS (Comparing against a list)
-- ============================================================================
-- PURPOSE: Advanced logic to compare a single value against a list of values.
--          Think of them as super-powered >, <, or = signs.

-- [ ANY ]
-- SCENARIO: Let's say Customer #5 is our standard benchmark. 
-- Find orders from ANY customer that are larger than *at least one* -- of Customer #5's orders. (Essentially: > MIN(Customer #5's orders)).
SELECT 
    OrderID, 
    Amount, 
    CustomerID 
FROM Orders 
WHERE Amount > ANY (
    SELECT Amount FROM Orders WHERE CustomerID = 5
);

-- [ ALL ]
-- SCENARIO: We want to find "Whale" orders. 
-- Find orders that are strictly larger than *every single order* -- ever placed by Customer #5. (Essentially: > MAX(Customer #5's orders)).
SELECT 
    OrderID, 
    Amount, 
    CustomerID 
FROM Orders 
WHERE Amount > ALL (
    SELECT Amount FROM Orders WHERE CustomerID = 5
);