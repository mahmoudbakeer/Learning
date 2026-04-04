


declare @StockQuantity INT;


set @StockQuantity = -5;




begin try

	if @StockQuantity < 0 
		begin
			throw 50001 , 'The stock quantity cannot be negative',1;
		end
	-- will be excuted only if the quantity is positive
	update Products set StockQuantity = @StockQuantity where ProductID = 1;
end try
begin catch 
	select
		ERROR_NUMBER() as ErrorNumber,
		ERROR_MESSAGE() as ErrorMessage ,
		ERROR_STATE() as ErrorState
end catch