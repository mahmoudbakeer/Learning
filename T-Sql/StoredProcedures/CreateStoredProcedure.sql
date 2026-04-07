
use C21_DB1;
go -- use go to avoid error related to starting of the batch


--a precompiled collection of one or more SQL statements stored directly in the database.
-- It functions as a reusable subroutine that can be executed repeatedly by name, often accepting input parameters to perform dynamic tasks.
create procedure SP_AddNewPerson
		@FirstName Nvarchar(100),		
		@LastName Nvarchar(100) ,
		@Email Nvarchar(50),
		@PersonID INT OUTPUT
as
begin

	Insert Into People (FirstName , LastName , Email ) values (@FirstName , @LastName , @Email) ;
	set @PersonID = SCOPE_IDENTITY();
end




