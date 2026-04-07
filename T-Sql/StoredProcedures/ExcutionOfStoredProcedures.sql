



use C21_DB1;
go

--declare @NewPersonID INT ;
--exec SP_AddNewPerson @FirstName = 'Mahmoud' , @LastName = 'Bakir' , @Email = 'mahmoud@gmailcom' ,@PersonID  = @NewPersonID;

--select * from People;

--declare @IsFound INT;
--exec @IsFound = SP_CheckPersonExist @PersonID  = 1;

--print @IsFound; -- 1 -> means found , 0 -> means not found

--exec SP_DeletePerson @PersonID = 2;

--select * from People;

exec SP_GetAllPeople; -- equal to select * from People;

exec SP_GetPersonByID @PersonID = 1;

exec SP_UpdatePerson @PersonID = 1 ,  @FirstName = 'Sami' , @LastName = 'Bakir' , @Email = 'Sami@gmailcom'

exec SP_GetPersonByID @PersonID = 1;



