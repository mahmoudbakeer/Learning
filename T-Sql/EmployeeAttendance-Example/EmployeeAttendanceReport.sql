



declare @PresentDays INT; 
declare @AbsentDays INT;
declare @LeaveDays INT;
declare @MonthDate INT;
declare @YearDate INT;
declare @EmployeeID INT;
declare @TotalAttendanceDays INT;


set @MonthDate = 7;
set @YearDate = 2023;
set @EmployeeID = 101;

set @TotalAttendanceDays = DAY(EOMONTH(DATEFROMPARTS(@YearDate , @MonthDate , 1)));
select @PresentDays = count(*) from EmployeeAttendance where EmployeeID = @EmployeeID and Month(AttendanceDate) = @MonthDate and Year(AttendanceDate) = @YearDate and Status = 'Present'; 
select @AbsentDays = count(*) from EmployeeAttendance where EmployeeID = @EmployeeID and Month(AttendanceDate) = @MonthDate and Year(AttendanceDate) = @YearDate and Status = 'Absent'; 
select @LeaveDays = count(*) from EmployeeAttendance where  EmployeeID = @EmployeeID and Month(AttendanceDate) = @MonthDate and Year(AttendanceDate) = @YearDate and Status = 'Leave'; 

print 'The Employee Monthly report with ID : ' + cast(@EmployeeID as varchar);
print 'The date of the report '+ cast(@YearDate as varchar) +' / '+ cast(@MonthDate as varchar);
print 'The total days of the month is ' + cast(@TotalAttendanceDays as varchar) ;
print 'The present days of the month is : ' + cast(@PresentDays as varchar);
print 'The Leave days of the month is : ' + cast(@LeaveDays as varchar);
print 'The Absent days of the month is : ' + cast(@AbsentDays as varchar);