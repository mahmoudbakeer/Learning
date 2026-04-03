
declare @PointsEarned INT;
declare @CustomerID INT;
declare @CurrentYear INT = Year(GETDATE());
declare @AmountSpent Decimal(10,2);



set @CurrentYear = 2023;
set @CustomerID =1 ;
select @AmountSpent = sum(Amount) from Purchases where Year(PurchaseDate) =  and CustomerID = @CustomerID;

set @PointsEarned = cast(@AmountSpent/10 as int);

update Customers 
set LoyaltyPoints = LoyaltyPoints + @PointsEarned where CustomerID = @CustomerID;

print 'The Customer Purchases report for year ' + cast(@CurrentYear as varchar);
print 'Customer ID is : ' + cast(@CustomerID as varchar);
print 'Total Amount Purchases is : ' + cast(@AmountSpent as varchar);
print 'Earned Points is : ' + cast(@PointsEarned as varchar);

