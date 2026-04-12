USE Shop_Database;
GO

-- ============================================================================
-- MASTERING COMMON TABLE EXPRESSIONS (CTEs)
-- ============================================================================
-- PURPOSE: CTEs (using the 'WITH' keyword) create temporary, named result sets. 
--          They make complex queries highly readable by separating the "math" 
--          from the final "presentation" (the JOINS).

-- ----------------------------------------------------------------------------
-- 1. THE FOUNDATION CTE (Standalone Aggregation)
-- ----------------------------------------------------------------------------
-- We start the chain with the 'WITH' keyword. This CTE calculates the total 
-- purchase amount for every customer. Because we define it here at the top, 
-- the database only has to do this heavy math once!
WITH CTE_CUSTOMER_PURCHASES AS (
    SELECT 
        CustomerID, 
        SUM(Amount) AS TotalPurchase
    FROM Orders 
    GROUP BY CustomerID
), 

-- ----------------------------------------------------------------------------
-- 2. CHAINING MULTIPLE CTEs 
-- ----------------------------------------------------------------------------
-- To add another CTE, DO NOT write 'WITH' again. Just use a comma.
-- This CTE calculates the single largest order each customer has ever placed.
CTE_MAX_AMOUNT_ORDER AS (
    SELECT 
        CustomerID, 
        MAX(Amount) AS MaxAmount
    FROM Orders 
    GROUP BY CustomerID
), 

-- ----------------------------------------------------------------------------
-- 3. NESTED CTEs (Referencing previous CTEs + Window Functions)
-- ----------------------------------------------------------------------------
-- The true power of CTEs: This block queries the FIRST CTE directly to 
-- calculate a rank based on the total purchases we already figured out.
CTE_RANKED_CUSTOMERS AS (	
    SELECT 
        CustomerID, 
        RANK() OVER(ORDER BY TotalPurchase DESC) AS RankOfCustomer 
    FROM CTE_CUSTOMER_PURCHASES
),

-- ----------------------------------------------------------------------------
-- 4. SEGMENTATION CTE (Using CASE Statements)
-- ----------------------------------------------------------------------------
-- Again, we read from the first CTE. We use a CASE statement to categorize 
-- our customers into tiers based on their spending. This is a very common 
-- real-world data analysis pattern.
CTE_SEGMENT_CUSTOMER AS (
    SELECT 
        CustomerID, 
        CASE
            WHEN TotalPurchase > 800 THEN 'High'
            WHEN TotalPurchase > 600 THEN 'Medium'
            ELSE 'Low'
        END AS CustomerLevel
    FROM CTE_CUSTOMER_PURCHASES
)

-- ============================================================================
-- THE MAIN QUERY (The Presentation Layer)
-- ============================================================================
-- Notice how clean this is! We aren't doing any math here. We are simply 
-- taking the 'Customers' table and snapping our four perfectly formatted 
-- puzzle pieces onto it using LEFT JOINs. 
SELECT 
    c.*, 
    ctp.TotalPurchase, 
    ctm.MaxAmount, 
    crc.RankOfCustomer, 
    csc.CustomerLevel
FROM Customers c
LEFT JOIN CTE_CUSTOMER_PURCHASES ctp 
    ON c.CustomerID = ctp.CustomerID
LEFT JOIN CTE_MAX_AMOUNT_ORDER ctm
    ON c.CustomerID = ctm.CustomerID
LEFT JOIN CTE_RANKED_CUSTOMERS crc
    ON c.CustomerID = crc.CustomerID
LEFT JOIN CTE_SEGMENT_CUSTOMER csc 
    ON c.CustomerID = csc.CustomerID
ORDER BY 
    TotalPurchase DESC;