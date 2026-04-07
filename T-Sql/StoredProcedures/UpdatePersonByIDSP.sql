



use C21_DB1;
go


create procedure SP_UpdatePerson
		@PersonID INT ,
		@FirstName nvarchar(100),
		@LastName nvarchar(100),
		@Email nvarchar(100)
as 
	begin
		update People 
			set 
				FirstName = @FirstName,
				LastName = @LastName,
				Email = @Email
			where PersonID = @PersonID;
	end
