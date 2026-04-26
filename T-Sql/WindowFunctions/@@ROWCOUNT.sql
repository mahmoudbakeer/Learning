





	use C21_DB1 




	update Employees2 set Salary = Salary * 1.1;

	-- show the number of rows affected by last statement 
	select @@ROWCOUNT as RowsAffected;



	-- just like @@ERROR function the @@ROWCOUNT will only return the rowsAffected by last statement 