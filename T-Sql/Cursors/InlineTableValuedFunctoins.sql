



use C21_DB1;
go




create function dbo.GetTableForSubject(@Subject varchar(100))
returns table -- returns not return 
as
return (select * from Students where Subject = @Subject);