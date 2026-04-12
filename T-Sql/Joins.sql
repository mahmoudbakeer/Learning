



use Shop_Database;
go





-- inner join getting only the matched results from both tables
USE Shop_Database;
GO

-- ============================================================================
-- SQL JOINS (Combining Data from Multiple Tables)
-- ============================================================================
-- PURPOSE: Joins are used to link tables together based on a common column 
--          (usually a Primary Key matching a Foreign Key, like CustomerID).

-- ----------------------------------------------------------------------------
-- 1. INNER JOIN (The "Perfect Match" Join)
-- ----------------------------------------------------------------------------
-- RULE: Both tables have equal standing. 
-- RESULT: Only returns rows where there is a strict match in BOTH tables.
-- SCENARIO: Get a list of customers who have actually placed orders, 
--           along with their order details.
SELECT 
    c.CustomerID, 
    c.Name, 
    o.OrderID, 
    o.Amount 
FROM Customers c
INNER JOIN Orders o
    ON o.CustomerID = c.CustomerID;

-- ----------------------------------------------------------------------------
-- 2. LEFT JOIN (The "Preserve the Left" Join)
-- ----------------------------------------------------------------------------
-- RULE: The First (Left) table is the main source and is completely preserved.
-- RESULT: Returns ALL rows from the Left table. If the Right table has a match, 
--         it attaches the data. If it doesn't match, it returns NULL (blank).
-- SCENARIO: Give me a master list of EVERY customer in our database, and attach 
--           their order data if they happen to have any.
SELECT 
    c.CustomerID, 
    c.Name, 
    o.OrderID, 
    o.Amount 
FROM Customers c
LEFT JOIN Orders o
    ON o.CustomerID = c.CustomerID;


-- ----------------------------------------------------------------------------
-- 3. RIGHT JOIN (The "Preserve the Right" Join)
-- ----------------------------------------------------------------------------
-- RULE: The exact reverse of a Left Join. The Second (Right) table is preserved.
-- RESULT: Returns ALL rows from the Right table. (Rarely used in practice, 
--         as most developers just flip the table order and use a LEFT JOIN).
-- SCENARIO: Give me a list of EVERY order in the system, and attach the 
--           customer name if we have one.
SELECT 
    c.CustomerID, 
    c.Name, 
    o.OrderID, 
    o.Amount 
FROM Customers c
RIGHT JOIN Orders o
    ON o.CustomerID = c.CustomerID;


-- ----------------------------------------------------------------------------
-- 4. FULL OUTER JOIN (The "Mashup" Join)
-- ----------------------------------------------------------------------------
-- RULE: Both tables are preserved. 
-- RESULT: Returns absolutely everything. It matches rows where it can, and 
--         puts NULLs on the left or the right wherever data is missing.
-- SCENARIO: We want a complete audit of our data to find orphaned orders 
--           AND customers who have never bought anything.
SELECT 
    c.CustomerID, 
    c.Name, 
    o.OrderID, 
    o.Amount 
FROM Customers c
FULL OUTER JOIN Orders o -- you can write full join without outer
    ON o.CustomerID = c.CustomerID;




