






use C21_DB1;
go




create trigger trg_InsteadOfDeleteStudent on Students
instead of delete
as 
		begin
			Update S set S.IsActive = 0 from Students S join deleted D on D.StudentID = S.StudentID;
		end;