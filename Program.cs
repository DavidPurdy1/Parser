using System.IO; 
using System;
using System.Configuration;
using System.Data.SqlClient;



namespace FileParser{
    
    class Parser{
        public System.Data.CommandType CommandType { get; set; }

        static void Main(string[] args){
           foreach(string file in Directory.EnumerateFiles(@"C:\Automation\failedTestsDocuments")){
                Console.WriteLine(file);
                string[] lines = File.ReadAllLines(file);

                foreach(string line in lines){

                }
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
            }   
        }
        public void command(Int64 i, SqlConnection sqlConn, String guid){
            try{
                SqlCommand sqlCommand = null;
                sqlCommand = new SqlCommand(spName, sqlConn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@id", id));
                sqlCommand.CommandTimeout = 60;
                SqlDataReader sqlDataReader = null; 
            }
        }



}
