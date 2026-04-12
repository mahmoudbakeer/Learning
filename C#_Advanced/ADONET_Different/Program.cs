using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

// --- 1. INITIAL CONFIGURATION SETUP ---
var conf = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Using GetSection().Value as requested for the connection string named 'constr'
string connectionString = conf.GetSection("constr").Value
    ?? throw new InvalidOperationException("Connection string 'constr' not found.");

Console.WriteLine($"Connection Established: {connectionString}\n");

// ==========================================
// SECTION 1: SELECT ALL (TEXT QUERY)
// ==========================================
Console.WriteLine("--- Section 1: Fetching All Wallets ---");
using (SqlConnection conn = new SqlConnection(connectionString))
{
    string query = @"SELECT * FROM Wallets";
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
        cmd.CommandType = CommandType.Text;
        try
        {
            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Best Practice: Create new instance inside the loop
                    var wallet = new MyWallet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Holder = reader.GetString(reader.GetOrdinal("Holder")),
                        Balance = reader.GetDecimal(reader.GetOrdinal("Balance"))
                    };
                    Console.WriteLine(wallet.ToString());
                }
            }
        }
        catch (SqlException ex) { Console.WriteLine($"Selection Error: {ex.Message}"); }
    }
}

// ==========================================
// SECTION 2: INSERT DATA (TEXT QUERY)
// ==========================================
Console.WriteLine("\n--- Section 2: Inserting New Wallet (Text) ---");
using (SqlConnection conn = new SqlConnection(connectionString))
{
    string query = @"INSERT INTO WALLETS (Holder, Balance) VALUES (@Holder, @Balance)";
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
        cmd.CommandType = CommandType.Text;
        // Best Practice: Use parameters to prevent SQL Injection
        cmd.Parameters.AddWithValue("@Holder", "Hadi");
        cmd.Parameters.AddWithValue("@Balance", 5000.00m);

        try
        {
            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Insert Success. Rows Affected: {rows}");
        }
        catch (SqlException ex) { Console.WriteLine($"Insert Error: {ex.Message}"); }
    }
}

// ==========================================
// SECTION 3: INSERT USING STORED PROCEDURE (AddWallet)
// ==========================================
Console.WriteLine("\n--- Section 3: Add Wallet (Stored Procedure) ---");
using (SqlConnection conn = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = new SqlCommand("AddWallet", conn))
    {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Holder", "Mahmoud");
        cmd.Parameters.AddWithValue("@Balance", 7500.50m);

        try
        {
            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"SP AddWallet Success. Rows Affected: {rows}");
        }
        catch (SqlException ex) { Console.WriteLine($"SP AddWallet Error: {ex.Message}"); }
    }
}

// ==========================================
// SECTION 4: SELECT USING STORED PROCEDURE (GetAllWallets)
// ==========================================
Console.WriteLine("\n--- Section 4: Get All (Stored Procedure) ---");
using (SqlConnection conn = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = new SqlCommand("GetAllWallets", conn))
    {
        cmd.CommandType = CommandType.StoredProcedure;
        try
        {
            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var wallet = new MyWallet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Holder = reader.GetString(reader.GetOrdinal("Holder")),
                        Balance = reader.GetDecimal(reader.GetOrdinal("Balance"))
                    };
                    Console.WriteLine(wallet.ToString());
                }
            }
        }
        catch (SqlException ex) { Console.WriteLine($"SP Select Error: {ex.Message}"); }
    }
}

// ==========================================
// SECTION 5: UPDATE DATA (TEXT QUERY)
// ==========================================
Console.WriteLine("\n--- Section 5: Updating Wallet ---");
using (SqlConnection conn = new SqlConnection(connectionString))
{
    string query = @"UPDATE WALLETS SET Balance = @NewBalance WHERE Id = @Id";
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@NewBalance", 9999.99m);
        cmd.Parameters.AddWithValue("@Id", 1);

        try
        {
            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Update Success. Rows Affected: {rows}");
        }
        catch (SqlException ex) { Console.WriteLine($"Update Error: {ex.Message}"); }
    }
}

// ==========================================
// SECTION 6: DELETE DATA (TEXT QUERY)
// ==========================================
Console.WriteLine("\n--- Section 6: Deleting Wallet ---");
using (SqlConnection conn = new SqlConnection(connectionString))
{
    string query = @"DELETE FROM WALLETS WHERE Id = @Id";
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@Id", 3); // Deleting wallet with ID 3

        try
        {
            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Delete Success. Rows Affected: {rows}");
        }
        catch (SqlException ex) { Console.WriteLine($"Delete Error: {ex.Message}"); }
    }
}