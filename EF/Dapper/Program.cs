

// we want to use dapper to get the data
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Runtime.ExceptionServices;
// now with dapper 
// first thing is to connect to database
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
Console.WriteLine(CultureInfo.CurrentCulture);

// we want to connect on it 

Console.WriteLine(config.GetSection("constr").Value);


// now lets make in one string 
string connectionString = config.GetSection("constr").Value;


SqlConnection connection = new SqlConnection(connectionString);
string query = "Select Id , Holder , Balance from WALLETS;";

var res = connection.Query<Wallet>(query);

foreach (var item in res)
{
    Console.WriteLine(item);
}
