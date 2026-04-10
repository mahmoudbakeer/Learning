



use C21_DB1;
go

-- Use the funcion to get the values
select * from dbo.GetTableForSubject('Math');

-- Use the function to Get average of the grades
select AVG(Grade) as AverageGrade from dbo.GetTableForSubject('Science');