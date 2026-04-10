



USE C21_DB1;
GO


Create Function dbo.CalculateAverage(@Subject VARCHAR(100))  returns int -- returns not return -- returns datatype
as 
	begin
		declare @AverageGrade INT;
		select @AverageGrade = cast(AVG(Grade) as int ) from Students where Subject = @Subject;
		return @AverageGrade;
	end;