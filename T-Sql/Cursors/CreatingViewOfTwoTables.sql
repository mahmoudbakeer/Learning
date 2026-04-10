




use C21_DB1;
go 

CREATE TABLE PersonInfo
(
	StudentID int primary key identity,
	Name nvarchar(100) not null,
	Adress nvarchar(100) not null
);


CREATE TABLE AcademicInfo 
(	
	StudentID int primary key,
	Subject nvarchar(100) not null,
	Grade int not null,
	 Foreign key (StudentID ) references PersonInfo(StudentID)
);





go
create view StudentView as select P.StudentID , P.Name , A.Subject , A.Grade from PersonInfo P join AcademicInfo A on P.StudentID = A.StudentID;