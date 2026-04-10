




use C21_DB1;
go 



create trigger trg_AfterInsetStudent on Students
after insert
as 
	begin
		insert into StudentInsertLog (StudentID , Name  , Subject, Grade) -- donot hardcode the values , means don't use the values (col , col , ...) here
		select StudentID , Name , Subject ,Grade from inserted;
	end;