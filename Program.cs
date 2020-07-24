using System.IO; 
using System;
using System.Configuration;
using System.Data; 
using System.Data.SqlClient;
/** 
* Needs to be ran to transfer data over to sql tables from the files, can be ran in the console by just using dotnet run 
* Author: David Purdy Date:7-24-20
*/ 
namespace FileParser{
    class Program{
        static void Main(string[] args){
            var connectionString = ConfigurationManager.AppSettings.Get("DBConnection"); 
            SqlConnection connection = new SqlConnection(connectionString); 

            TestData data = new TestData();
            new Parser().ParseFile(data, connection); 

            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }        
    }
    public class Parser{
         public void AddToTestRunTable(TestData data, SqlConnection connection){

            //running the command
            SqlCommand command = new SqlCommand();
            connection.Open();
            command.CommandTimeout = 60;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "spAddTestRunData";

            //adding the values to the parameter
            command.Parameters.Add("testerName", SqlDbType.NVarChar, 50).Value = data.CreatedBy;  
            command.Parameters.Add("applicationName", SqlDbType.NVarChar, 50).Value = data.ApplicationName;
            command.Parameters.Add("applicationVersion", SqlDbType.NVarChar, 50).Value = data.ApplicationVersion;
            command.Parameters.Add("testsFailed", SqlDbType.SmallInt).Value = data.TestsFailed;
            command.Parameters.Add("testsPassed", SqlDbType.SmallInt).Value = data.TestsPassed;
            command.Parameters.Add("createdDate", SqlDbType.DateTime).Value = data.CreatedDate;
            command.Parameters.Add("createdBy", SqlDbType.NVarChar, 50).Value = data.CreatedBy;
            command.Parameters.Add("modifiedBy", SqlDbType.NVarChar, 50).Value = data.CreatedBy;
            command.Parameters.Add("modifiedDate", SqlDbType.DateTime, 50).Value = data.CreatedDate;

            command.ExecuteNonQuery();
            connection.Close();
        }
        public void ParseFile(TestData data, SqlConnection connection){
            foreach(string file in Directory.EnumerateFiles(ConfigurationManager.AppSettings.Get("FileLocation"))){
                Console.WriteLine(file);
                string[] lines = File.ReadAllLines(file);
                foreach(var line in lines){
                     Console.WriteLine(line);
                }
                
                data.CreatedDate = DateTime.Parse(lines[1].Substring(0, lines[1].Length-2));
                data.TestsPassed = int.Parse(lines[2].Substring(14)); 
                data.TestsFailed = int.Parse(lines[3].Substring(14)); 
                data.CreatedBy = lines[4].Substring(8); 
                data.ApplicationVersion = lines[5].Substring(13); 
                data.ApplicationName = lines[6].Substring(10); 

                data.TestsPassed = 0; 
                data.TestsFailed = 0; 
                for (int i = 7; i < lines.Length; i++){
                    if(lines[i].Substring(0,8).Equals("passed| ")){
                        data.TestsPassed++; 
                    }else{
                        data.TestsFailed++; 
                    } 
                }
                AddToTestRunTable(data, connection);
            }
        }
    }
    public class TestData{
        public int TestsFailed { get; set; }
        public int TestsPassed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageLocation { get; set; }
        public string ApplicationName{ get; set; }
        public string ApplicationVersion { get; set; }
    } 
}
