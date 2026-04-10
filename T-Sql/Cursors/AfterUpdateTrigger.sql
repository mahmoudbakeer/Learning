


use C21_DB1;

GO




create trigger trg_AfterUpdateStudent on Students
after update
as	
	
	begin
		if update(Grade)
			begin
				insert into StudentUpdateLog (StudentID , OldGrade , NewGrade )
				select i.StudentID , d.Grade as OldGrade , i.Grade as NewGrade from inserted i join deleted d on i.StudentID = d.StudentID;  
			end
	end;
