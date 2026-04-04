




---- Create Accounts Table
--CREATE TABLE Accounts (
--    AccountID INT PRIMARY KEY,
--    Balance DECIMAL(10, 2)
--);


---- Create Transactions Table
--CREATE TABLE Transactions (
--    TransactionID INT PRIMARY KEY IDENTITY(1,1),
--    FromAccount INT,
--    ToAccount INT,
--    Amount DECIMAL(10, 2),
--    Date DATETIME
--);


---- Insert Sample Data into Accounts
--INSERT INTO Accounts (AccountID, Balance) VALUES (1, 500.00); -- Account 1
--INSERT INTO Accounts (AccountID, Balance) VALUES (2, 300.00); -- Account 2




-- Transaction:
-- A transaction is a sequence of one or more SQL operations executed as a single logical unit of work.
-- It ensures that either all operations are completed successfully (COMMIT),
-- or none of them are applied if an error occurs (ROLLBACK).

-- ACID Properties:
-- ACID is a set of properties that guarantee reliable and consistent database transactions:

-- 1. Atomicity:
--    Ensures that all operations in a transaction are completed successfully.
--    If any operation fails, the entire transaction is rolled back.

-- 2. Consistency:
--    Ensures that the database remains in a valid state before and after the transaction.
--    All rules, constraints, and relationships must be preserved.

-- 3. Isolation:
--    Ensures that transactions are executed independently of each other.
--    Intermediate states of a transaction are not visible to other transactions.

-- 4. Durability:
--    Ensures that once a transaction is committed, its changes are permanently saved,
--    even in the case of system failure.

		use C21_DB1;
	
	
-- transaction

	begin transaction;

begin try
    -- debit
    update Accounts 
    set Balance = Balance - 100 
    where AccountID = 1;

    if @@ROWCOUNT = 0
        THROW 50001, 'Source account not found', 1;

    -- credit
    update Accounts 
    set Balance = Balance + 100 
    where AccountID = 2; -- try to change one of the AccountID and it will throw an error change it other than 1 , 2 

    if @@ROWCOUNT = 0
        THROW 50002, 'Destination account not found', 1;

    -- log transaction
    INSERT INTO Transactions (FromAccount, ToAccount, Amount, Date) 
    VALUES (1, 2, 100, GETDATE());

    -- (this check is optional here, INSERT normally affects 1 row)
    if @@ROWCOUNT = 0
        THROW 50003, 'Transaction insert failed', 1;

    COMMIT;
end try
begin catch
    ROLLBACK;

    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_MESSAGE() AS ErrorMessage;
end catch


	
