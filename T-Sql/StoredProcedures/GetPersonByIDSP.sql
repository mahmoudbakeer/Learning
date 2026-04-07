


use C21_DB1;
go 

create procedure SP_GetPersonByID
		@PersonID INT
as 
	begin
		select * from People where PersonID = @PersonID;
	end