use C21_DB1;

-- Insert sample data into Sales
CREATE TABLE Sales (
    SaleID INT PRIMARY KEY,
    SaleDate DATE,
    SaleAmount DECIMAL(10, 2)
);

-- Inserting sample data into Sales
INSERT INTO Sales (SaleID, SaleDate, SaleAmount) VALUES (1, '2023-06-01', 150.00);
INSERT INTO Sales (SaleID, SaleDate, SaleAmount) VALUES (2, '2023-06-03', 200.00);
INSERT INTO Sales (SaleID, SaleDate, SaleAmount) VALUES (3, '2023-06-05', 50.00);
INSERT INTO Sales (SaleID, SaleDate, SaleAmount) VALUES (4, '2023-07-10', 300.00);
INSERT INTO Sales (SaleID, SaleDate, SaleAmount) VALUES (5, '2023-07-11', 250.00);

