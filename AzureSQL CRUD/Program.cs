using System;
using System.Data.SqlClient;

namespace AzureSQL_CRUD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SQL DB CRUD OPERATIONS");
            try
            {
                Program p = new Program();
                p.StartADOConnection();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void StartADOConnection()
        {
            try
            {
                using(var connection = new SqlConnection(<"connectionstring">))
                {
                    connection.Open();
                    Console.WriteLine("Creating table with ADO");
                    
                    using(var command = new SwlCommand(ADO_CreateTables(), connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Number of rows affected : {rowsAffected}");

                    }

                    Console.WriteLine("========================================");


                    Console.WriteLine("Adding data to the tables with ADO");
                    using (var command = new SqlCommand(ADO_Inserts(), connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Number of rows affected : {rowsAffected}");
                    }

                    Console.WriteLine("========================================");


                    Console.WriteLine("Updating data with ADO");
                    using (var command = new SqlCommand(ADO_UpdateJoin(), connection))
                    {
                        command.Parameters.AddWithValue("@charpParmDepartmentName", "Accounting");
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Number of rows affected : {rowsAffected}");
                    }

                    Console.WriteLine("========================================");



                    Console.WriteLine("Deleting data from tables with ADO");
                    using (var command = new SqlCommand(ADO_DeleteJoin(), connection))
                    {
                        command.Parameters.AddWithValue("@charpParmDepartmentName", "Legal");
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Number of rows affected : {rowsAffected}");
                    }

                    Console.WriteLine("========================================");


                    Console.WriteLine("Reading data from tables with ADO");
                    using (var command = new SqlCommand(ADO_SelectEmployees(), connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader.GetGuid(0)} , " + $"{reader.GetString(1)} , " + $"{reader.GetInt32(2)} , " + $"{reader?.GetString(3)} , " + $"{reader?.GetString(4)}");
                            }
                        }
                    }

                    Console.WriteLine("===============================");

                    
                       

                }
            }
            
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        static string ADO_CreateTables()
        {
            return @"
            DROP TABLE IF EXISTS tabEmployee;
            DROP TABLE IF EXISTS tabEmployee;
            
            CREATE TABLE tabDepartment
            (
                DepartmentCode nchar(4)         not null        PRIMARY KEY,
                DepartmentName nvarchar(128)    not null        
            );
            
            CREATE TABLE tabEmployee
            (
                EmployeeGuid    uniqueIdentifier    not null    default NewId() PRIMARY KEY,
                EmployeeName    nvarchar(128)       not null,
                EmployeeLevel   int                 not null,
                DepartmentCode  nchar(4)            null
                REFERENCES tabDepartment (DepartmentCode)
            );
            ";
        }


        static string ADO_Inserts()
        {
            return @"
                INSERT INTO tabDepartment (DepartmentCode, DepartmentName)
                VALUES
                    ('acct', 'Accounting'),
                    ('hr', 'Humar Resources'),
                    ('legl', 'Legal');

                INSERT INTO tabEmployee (EmployeeName, EmployeeLevel, DepartmentCode)
                VALUES
                    ('Alison',  19, 'acct'),
                    ('Barbara', 17, 'hr'),
                    ('Carol',   21, 'legl')
                    ('Elle',    15, 'null');
            ";
        }

        static string ADO_SelectEmployees()
        {
            return @"
            SELECT
                emp1.EmployeeGuid,
                emp1.EmployeeName,
                emp1.EmployeeLevel,
                emp1.DepartmentCode,
                emp1.DepartmentName
            FROM
                tableEmployee   as emp1
            LEFT OUTER JOIN
                tableDepartment as dep ON dept.DepartmentCode = emp1.DepartmentCode
            ORDER BY
                EmployeeName;
         ";
        }

    }
}
