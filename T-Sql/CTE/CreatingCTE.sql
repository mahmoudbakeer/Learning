



use C21_DB1;
go

-- subquery
select * from (
	select EmployeeID , Name , Sales from Employees6 where Department = 'Sales'
) Sales;





-- cte

with Sales as
(select EmployeeID , Name , Sales from Employees6 where Department = 'Sales')
select * from Sales;


with AggregationOfSales as
(	
	select Department ,  sum(Sales) as Total from Employees6 group by Department
)

select * from AggregationOfSales;