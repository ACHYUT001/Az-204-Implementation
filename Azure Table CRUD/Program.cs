using System;
using Microsoft.Azure.Cosmos.Table;

namespace Azure_Table_CRUD
{
        class Program
        {
            static CloudStorageAccount storageAccount;
            static CloudTableClient tableclient;
            static CloudTable employees;



            static void Main(string[] args)
            {

                //1. create connection to azure(via storage account) 2. create a table client 3. use table client to fetch the reference to the table
                storageAccount = CloudStorageAccount.Parse("<Storage Account Connection String");
                tableclient = storageAccount.CreateCloudTableClient();
                employees = tableclient.GetTableReference("tablecrud");

                InsertOp("John", "Parry", "Sage");
                InsertOp("Lyra", "Silvertongue", "Leader");
                InsertOp("Iorek", "Byrnison", "Leader");

                QueryOp();





            }


            public static void InsertOp(string firstname, string lastname, string designation)
            {
                Employees emp = new Employees(firstname, lastname, designation);

                TableOperation insertoperation = TableOperation.Insert(emp);
                employees.Execute(insertoperation);

            }

            public static void QueryOp()
            {
                TableQuery<Employees> query = new TableQuery<Employees>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Leader"));
                Console.WriteLine("Here are the name of your Leaders");
                foreach (Employees e in employees.ExecuteQuery(query))
                {
                    Console.WriteLine(e.RowKey);
                }
            }
        }

        public class Employees : TableEntity
        {
            public Employees(string firstname, string lastname, string designation)
            {

                this.PartitionKey = designation;
                this.RowKey = firstname + " " + lastname;

            }

            public Employees()
            {
            }
        }
}
