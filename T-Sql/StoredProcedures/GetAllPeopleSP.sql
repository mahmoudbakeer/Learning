



use C21_DB1;
go 

create procedure SP_GetAllPeople
as
	begin
		select * from People;
	end