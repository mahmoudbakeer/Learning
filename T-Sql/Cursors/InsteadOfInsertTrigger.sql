




use C21_DB1;
go



Create trigger trg_InsteadOfInsertinView on StudentView
instead of insert
as
		begin
			
			insert into PersonInfo (StudentID , Name , Adress)
			select StudentID , Name , Adress from inserted;

			-- 
			INSERT INTO AcademicInfo ( StudentID , Subject, Grade)
			SELECT StudentID, Subject, Grade FROM inserted;
		end;
