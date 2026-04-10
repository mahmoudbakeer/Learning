




use C21_DB1;
go

SELECT Name, Subject,dbo.CalculateAverage(Subject) as AverageGrade FROM Students; -- use dbo. means call the schema 