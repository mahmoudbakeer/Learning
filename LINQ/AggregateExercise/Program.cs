

List<int> numbers = [1, 2, 4, 5, 7, 96, 65];

// this will result the summation of all numbers in the list
var res = numbers.Aggregate((acc , nxt) => acc + nxt);

Console.WriteLine($"The summation is : {res}");
// the initial value of the accumaltor is the first element in the sequence
// if you want to specify the output you have to set it on the first argument

var resWithInitial = numbers.Aggregate(0, (acc, nxt) => acc + nxt);
Console.WriteLine($"The result with initial value of aggregation is : {resWithInitial}");
var resWithInitialAndResult = numbers.Aggregate(0 , (acc, nxt) => acc + nxt, result => -1 * result);
Console.WriteLine($"The negative summation is : {resWithInitialAndResult}");

