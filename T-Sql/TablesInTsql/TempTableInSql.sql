



-- a short-lived table used to store intermediate results during a session or transaction. 
--They are physically stored in a temporary database
--(like tempdb in SQL Server) and are automatically deleted when the session ends. 




-- It has two types one is Local Table and second is Global Table

-- # means local , ## means global
create table #TempEmployee
(
	EmployeeID int Primary Key,
	Name varchar(50),
	Salary decimal(10,2)
);



insert into #TempEmployee Values(1,'Hamed',50000); 
insert into #TempEmployee Values(2,'Sami',60000);


--drop table #TempEmployee

