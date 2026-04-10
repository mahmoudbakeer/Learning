

use C21_DB1;
go


create function dbo.GetTopPerformingStudents()
returns @Result Table
(
	StudentID int primary key,
	Name nvarchar(100),
	Subject nvarchar(100),
	Grade int 
)
as 
		begin
			insert into @Result (StudentID , Name , Subject , Grade)
			 SELECT TOP 3 StudentID, Name, Subject, Grade
			 FROM Students
			 ORDER BY Grade DESC
									return;
		end