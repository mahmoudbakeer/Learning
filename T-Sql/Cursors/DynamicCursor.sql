



use C21_DB1;
go

-- dynamic cursor used when the real time data needed , means any update delete insert change on result set will be reflected on cursor
-- not like static

-- delcare the Dynamic as static
Declare dynamic_cursor Cursor Dynamic for
select StudentID , Name , Grade from Students;

-- opent the cursor
open dynamic_cursor;

declare @StudentID INT ;
declare @Name varchar(100);
declare @Grade INT;

-- fetch the next row from the set result
fetch next from dynamic_cursor into @StudentID , @Name , @Grade;

-- if status zero then that's mean there is rows left in result set
while @@FETCH_STATUS = 0
	begin
		-- report generation
		print 'The Student ID : ' + cast(@StudentID as varchar) + ' and Name : ' + cast(@Name as varchar) + ' has Grade : ' + cast(@Grade as varchar);
		
		-- fetch the next row from the set result
		fetch next from dynamic_cursor into @StudentID , @Name , @Grade;
	end;

-- close the cursor
close dynamic_cursor;
-- deallocate the cursor
deallocate dynamic_cursor;