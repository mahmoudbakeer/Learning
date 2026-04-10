



use C21_DB1;
go



-- delcare the cursor as static
Declare static_cursor Cursor static for
select StudentID , Name , Grade from Students;

-- opent the cursor
open static_cursor;

declare @StudentID INT ;
declare @Name varchar(100);
declare @Grade INT;

-- fetch the next row from the set result
fetch next from static_cursor into @StudentID , @Name , @Grade;

-- if status zero then that's mean there is rows left in result set
while @@FETCH_STATUS = 0
	begin
		-- report generation
		print 'The Student ID : ' + cast(@StudentID as varchar) + ' and Name : ' + cast(@Name as varchar) + ' has Grade : ' + cast(@Grade as varchar);
		
		-- fetch the next row from the set result
		fetch next from static_cursor into @StudentID , @Name , @Grade;
	end;

-- close the cursor
close static_cursor;
-- deallocate the cursor
deallocate static_cursor;