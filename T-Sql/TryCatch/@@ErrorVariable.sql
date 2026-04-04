
	use C21_DB1



	-- we will assume that there is an DepartmentID = 1 then it will throw an error saying that the primary key violated
	insert into Departments (DepartmentID , Name) values(1 ,'Hadi');
	declare @ErrorNumber INT = @@Error;


	-- if not zero then error has ocurred
	if @ErrorNumber <> 0
		begin	
			print 'Error Ocurred The Error Number is : ' + cast(@ErrorNumber as varchar);
		end

-- using the @@Error variable to handle the error is old way now we can use try and catch 
-- but it is important to know 
-- its limitation that it will only return the final error has ocurred because if there in new error the variable will be overwritten and return the last error number
