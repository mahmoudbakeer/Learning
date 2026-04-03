using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDvsT_SQL
{
    public class DAEmployee
    {
        public static string ConnectionString = "Server=.;Database=C21_DB1;Trusted_Connection=True;";

        public static void openFirstConnection()
        {
            using (SqlConnection connection = new(ConnectionString))
            {

                connection.Open();

            }
        }
        public static DataTable GetEmployees()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Employees2;";
            using (SqlConnection connection = new(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public static bool UpdateEmployeeSalary(Employee emp)
        {
            if(emp == null) return false;
            else
            {
                int rowAffected = 0;
                string query = "UPDATE Employees2 set Salary = @Salary where ID = @ID;";
                using (SqlConnection connection = new(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Salary",emp.Salary);
                        command.Parameters.AddWithValue("@ID",emp.ID);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
                return (rowAffected > 0);
            }
        }
        public static int UpdateEmployeesSalaryUsingTSQL()
        {
            int rowAffected = 0;
            string query = """
                        UPDATE Employees2
                        SET Salary = CASE 
                                        WHEN Department = 'Sales' THEN
                                            CASE 
                                                WHEN PerformanceRating > 90 THEN Salary * 1.15
                                                WHEN PerformanceRating BETWEEN 75 AND 90 THEN Salary * 1.10
                                                ELSE Salary * 1.05
                                            END
                                        WHEN Department = 'HR' THEN
                                            CASE 
                                                WHEN PerformanceRating > 90 THEN Salary * 1.10
                                                WHEN PerformanceRating BETWEEN 75 AND 90 THEN Salary * 1.08
                                                ELSE Salary * 1.04
                                            END
                                        ELSE
                                            CASE 
                                                WHEN PerformanceRating > 90 THEN Salary * 1.08
                                                WHEN PerformanceRating BETWEEN 75 AND 90 THEN Salary * 1.06
                                                ELSE Salary * 1.03
                                            END
                                    END;
                        """;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                }
            }
            return rowAffected;
        }
    }
}
