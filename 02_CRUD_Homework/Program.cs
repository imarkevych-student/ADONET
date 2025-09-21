using _02_CRUD_Interface;
using _03_data_access;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Text;

namespace _02_CRUD_Homework
{
    internal class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SportShop"].ConnectionString;

        static void Main(string[] args)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            Console.OutputEncoding = Encoding.UTF8;
            using (SportShopDb db = new SportShopDb(connectionString))
            {

                
                Console.WriteLine("Оберіть дію:");
                Console.WriteLine("1. Додати нову продажу");
                Console.WriteLine("2. Всі продажі за період");
                Console.WriteLine("3. Остання покупка клієнта");
                Console.WriteLine("4. Видалити працівника або клієнта");
                Console.WriteLine("5. Найуспішніший працівник");
                Console.WriteLine("0. Вихід");

                switch (Console.ReadLine())
                {
                    case "1": db.CreateSale(); break;
                    case "2": db.ReadSalesByPeriod(); break;
                    case "3": db.ReadLastPurchaseByClient(); break;
                    case "4": db.DeletePerson(); break;
                    case "5": db.ReadTopEmployee(); break;
                }
            }

            
        }

        

    }
}
