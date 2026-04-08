


use C21_DB1;
go


-- Numbering the rows returned based on the marks
Select StudentID , Name , Subject ,Grade , ROW_NUMBER() over (Order by Grade desc) as RowNumber from Students ;
