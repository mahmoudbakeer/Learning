


use C21_DB1;
go


create trigger trg_UpdateStudentView on StudentView
instead of Update 
as
	begin
		if update(Name)
		begin
			Update PersonInfo  
			set Name = I.Name 
			from PersonInfo join inserted I on I.StudentID = PersonInfo.StudentID;
		end
		--
		if update(Grade) and update(Subject)
		begin
			update AcademicInfo
			set Grade = I.Grade
			, Subject = I.Subject from AcademicInfo join inserted I on I.StudentID = AcademicInfo.StudentID;
		end
	end;