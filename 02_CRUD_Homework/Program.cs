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
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Оберіть дію:");
            Console.WriteLine("1. Додати нову продажу");
            Console.WriteLine("2. Всі продажі за період");
            Console.WriteLine("3. Остання покупка клієнта");
            Console.WriteLine("4. Видалити працівника або клієнта");
            Console.WriteLine("5. Найуспішніший працівник");
            Console.WriteLine("0. Вихід");

            switch (Console.ReadLine())
            {
                case "1": CreateSale(); break;
                case "2": ReadSalesByPeriod(); break;
                case "3": ReadLastPurchaseByClient(); break;
                case "4": DeletePerson(); break;
                case "5": ReadTopEmployee(); break;
            }
        }

        static void CreateSale()
        {
            Console.Write("ID товару: "); int productId = int.Parse(Console.ReadLine());
            Console.Write("Ціна продажу: "); decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Кількість: "); int quantity = int.Parse(Console.ReadLine());
            Console.Write("ID працівника: "); int employeeId = int.Parse(Console.ReadLine());
            Console.Write("ID клієнта: "); int clientId = int.Parse(Console.ReadLine());

            using SqlConnection conn = new SqlConnection(connectionString);
            string query = @"INSERT INTO Salles (ProductId, Price, Quantity, EmployeeId, ClientId)
                             VALUES (@productId, @price, @quantity, @employeeId, @clientId)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            cmd.Parameters.AddWithValue("@clientId", clientId);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Додано {rows} продажу.");
        }

        static void ReadSalesByPeriod()
        {
            Console.Write("Початкова дата (yyyy-MM-dd): "); DateTime start = DateTime.Parse(Console.ReadLine());
            Console.Write("Кінцева дата (yyyy-MM-dd): "); DateTime end = DateTime.Parse(Console.ReadLine());

            using SqlConnection conn = new SqlConnection(connectionString);
            string query = @"SELECT s.Id, p.Name, s.Price, s.Quantity, e.FullName, c.FullName
                             FROM Salles s
                             JOIN Products p ON s.ProductId = p.Id
                             JOIN Employees e ON s.EmployeeId = e.Id
                             JOIN Clients c ON s.ClientId = c.Id
                             WHERE s.Id IN (
                                 SELECT Id FROM Salles
                                 WHERE s.SaleDate BETWEEN @start AND @end
                             )";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("\nПродажі за період:");
            while (reader.Read())
                Console.WriteLine($"ID: {reader["Id"]}, Товар: {reader["Name"]}, Сума: {reader["Price"]}, Кількість: {reader["Quantity"]}, Продавець: {reader["FullName"]}, Клієнт: {reader[5]}");
        }

        static void ReadLastPurchaseByClient()
        {
            Console.Write("ПІБ клієнта: "); string fullName = Console.ReadLine();

            using SqlConnection conn = new SqlConnection(connectionString);
            string query = @"
                SELECT TOP 1 s.Price, s.Quantity, p.Name
                FROM Salles s
                JOIN Clients c ON s.ClientId = c.Id
                JOIN Products p ON s.ProductId = p.Id
                WHERE c.FullName = @fullName
                ORDER BY s.Id DESC";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@fullName", fullName);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                Console.WriteLine($"Остання покупка: {reader["Name"]}, Сума: {reader["Price"]}, Кількість: {reader["Quantity"]}");
            else
                Console.WriteLine("Немає покупок.");
        }

        static void DeletePerson()
        {
            Console.Write("Видалити (employee/client): "); string type = Console.ReadLine().ToLower();
            Console.Write("ID: "); int id = int.Parse(Console.ReadLine());

            string table = type == "employee" ? "Employees" : "Clients";

            using SqlConnection conn = new SqlConnection(connectionString);
            string query = $"DELETE FROM {table} WHERE Id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Видалено {rows} запис(ів) з {table}.");
        }

        static void ReadTopEmployee()
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = @"
                SELECT TOP 1 e.FullName, SUM(s.Price * s.Quantity) AS TotalSales
                FROM Salles s
                JOIN Employees e ON s.EmployeeId = e.Id
                GROUP BY e.FullName
                ORDER BY TotalSales DESC";

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                Console.WriteLine($"Найуспішніший працівник: {reader["FullName"]}, Сума продажів: {reader["TotalSales"]}");
        }

    }
}
