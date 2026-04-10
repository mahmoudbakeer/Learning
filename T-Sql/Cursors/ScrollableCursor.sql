



use C21_DB1;
go

-- scroll is a bi-directional way to get the rows from the set
-- can be used with dynamic or static cursor
-- delcare the dynamic scroll
Declare dynamic_cursor Cursor Dynamic scroll for
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
		
		-- fetch the previous row from the set result
		print 'Previous row : '
		fetch prior from dynamic_cursor into @StudentID , @Name , @Grade;
		print 'The Student ID : ' + cast(@StudentID as varchar) + ' and Name : ' + cast(@Name as varchar) + ' has Grade : ' + cast(@Grade as varchar);
		
-- close the cursor
close dynamic_cursor;
-- deallocate the cursor
deallocate dynamic_cursor;