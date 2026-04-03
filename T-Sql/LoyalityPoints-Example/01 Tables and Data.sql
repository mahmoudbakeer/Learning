
Use C21_DB1;

-- Creating Customers table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(50),
    LoyaltyPoints INT
);

-- Inserting sample data into Customers
INSERT INTO Customers (CustomerID, Name, LoyaltyPoints) VALUES (1, 'John Doe', 0);
INSERT INTO Customers (CustomerID, Name, LoyaltyPoints) VALUES (2, 'Jane Smith', 0);
INSERT INTO Customers (CustomerID, Name, LoyaltyPoints) VALUES (3, 'Emily White', 0);

-- Creating Purchases table
CREATE TABLE Purchases (
    PurchaseID INT PRIMARY KEY,
    CustomerID INT,
    PurchaseDate DATE,
    Amount DECIMAL(10, 2),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Inserting sample data into Purchases
INSERT INTO Purchases (PurchaseID, CustomerID, PurchaseDate, Amount) VALUES (1, 1, '2023-07-01', 120.00);
INSERT INTO Purchases (PurchaseID, CustomerID, PurchaseDate, Amount) VALUES (2, 2, '2023-07-02', 60.00);
INSERT INTO Purchases (PurchaseID, CustomerID, PurchaseDate, Amount) VALUES (3, 1, '2023-07-03', 30.00);
-- Add more data as needed
