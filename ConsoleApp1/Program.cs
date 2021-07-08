using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        public static void CreateTable(CanTable obj)
        {
            var type = obj.GetType();
            string connectionString = ConfigurationManager.ConnectionStrings["Car"].ConnectionString;
            Console.WriteLine(type.Name);
            using SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            Console.WriteLine(connection.State);
            command.Connection = connection;
            command.CommandText =
            "CREATE TABLE Cars" +
           $"(Id INT  NOT NULL)";
            command.ExecuteNonQuery();
            foreach (var prop in type.GetProperties())
            {
                command.CommandText = "USE Car";
                command.ExecuteNonQuery();
                if (prop.PropertyType == typeof(string))
                {
                    command.CommandText = $"ALTER TABLE Cars ADD {prop.Name} NVARCHAR(255)";
                    command.ExecuteNonQuery();
                }
                if (prop.PropertyType == typeof(double))
                {
                    command.CommandText = $"ALTER TABLE Cars ADD {prop.Name} NVARCHAR(100)";
                    command.ExecuteNonQuery();
                }
                Console.WriteLine(prop.Name);
            }
        }

        static void Main(string[] args)
        {
            List<Car> Cars = new List<Car>()
            {
                new Car
                {
                    Id = 1,
                    Model = "Grand-Cherokee",
                    Vendor = "Jeep",
                    EngineVolume = 5.7
                },
                new Car
                {
                    Id = 2,
                    Model = "CX-5",
                    Vendor = "Mazda",
                    EngineVolume = 2.7
                },
                new Car()
                {
                    Id = 3,
                    Model = "Lancer Evolution VII",
                    Vendor = "Mitsubishi",
                    EngineVolume = 2.4
                },
                new Car()
                {
                    Id = 4,
                    Model = "Brabus",
                    Vendor = "Mercedes-Benz",
                    EngineVolume = 7.2
                }
            };
            //CreateTable();
            Car car = new Car();
            var connectionString = ConfigurationManager.ConnectionStrings["Car"].ConnectionString;
            CreateTable(car);
            using SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            foreach (var item in Cars)
            {
                command.CommandText = $"INSERT INTO Cars(Id,Model,Vendor,EngineVolume) VALUES('{item.Id}' , '{item.Model}', '{item.Vendor}', '{item.EngineVolume}')";
                command.ExecuteNonQuery();
                Console.WriteLine("OK");
            }
            connection.Close();
        }
    }
}
