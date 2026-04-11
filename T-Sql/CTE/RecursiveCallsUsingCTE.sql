



use C21_DB1;

go



-- recursive calls using cte


with RecursiveCall as
(
-- the anchor member 
-- means the initialization statement
	select 1 as number
	
	-- the loop count from one to 10 
	union all
	select number + 1 from RecursiveCall where number < 10
) 
select * from RecursiveCall;