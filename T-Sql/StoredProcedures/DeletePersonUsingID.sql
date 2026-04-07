



use C21_DB1;
go 




create procedure SP_DeletePerson
		@PersonID INT
as
		begin
				delete from People where PersonID = @PersonID; 
		end