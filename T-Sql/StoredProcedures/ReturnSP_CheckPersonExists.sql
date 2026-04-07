



use C21_DB1;
go

create procedure SP_CheckPersonExist
		@PersonID INT
as
		begin
			if Exists ( Select * from People where PersonID = @PersonID)
				begin
					return 1;
				end
			else
				begin
					return 0;
				end
		end