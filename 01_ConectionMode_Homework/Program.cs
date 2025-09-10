using Microsoft.Data.SqlClient;
using System.Text;

namespace SportShopConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                                        Initial Catalog=SportShop;
                                        Integrated Security=True;
                                        TrustServerCertificate=True";

            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Підключення до SportShop успішне!");

            Console.WriteLine("\nОберіть дію:");
            Console.WriteLine("1. Всі товари");
            Console.WriteLine("2. Всі працівники");
            Console.WriteLine("3. Продажі певного працівника");
            Console.WriteLine("4. Продажі на суму більше X");
            Console.WriteLine("5. Найдорожча/найдешевша покупка клієнта");
            Console.WriteLine("6. Найперша продажа працівника");
            Console.WriteLine("0. Вихід");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ShowAll(sqlConnection, "Products"); break;
                case "2": ShowAll(sqlConnection, "Employees"); break;
                case "3": SalesByEmployee(sqlConnection); break;
                case "4": SalesAboveAmount(sqlConnection); break;
                case "5": MinMaxSaleByClient(sqlConnection); break;
                case "6": FirstSaleByEmployee(sqlConnection); break;
                case "0": return;
            }

            sqlConnection.Close();
        }

        static void ShowAll(SqlConnection conn, string table)
        {
            string query = $"SELECT * FROM {table}";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($" {reader.GetName(i),15}");
            Console.WriteLine("\n" + new string('-', 80));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($" {reader[i],15}");
                Console.WriteLine();
            }
        }

        static void SalesByEmployee(SqlConnection conn)
        {
            Console.Write("Введіть ПІБ працівника: ");
            string fullName = Console.ReadLine();

            string query = $@"
                SELECT s.Id, p.Name AS Product, s.Price, s.Quantity
                FROM Salles s
                JOIN Products p ON s.ProductId = p.Id
                JOIN Employees e ON s.EmployeeId = e.Id
                WHERE e.FullName = N'{fullName}'";

            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\nПродажі працівника:");
            while (reader.Read())
                Console.WriteLine($"ID: {reader["Id"]}, Товар: {reader["Product"]}, Сума: {reader["Price"]}, Кількість: {reader["Quantity"]}");
        }

        static void SalesAboveAmount(SqlConnection conn)
        {
            Console.Write("Введіть мінімальну суму: ");
            string amount = Console.ReadLine();

            string query = $@"SELECT * FROM Salles WHERE Price > {amount}";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine($"\nПродажі на суму більше {amount}:");
            while (reader.Read())
                Console.WriteLine($"ID: {reader["Id"]}, Сума: {reader["Price"]}, Кількість: {reader["Quantity"]}");
        }

        static void MinMaxSaleByClient(SqlConnection conn)
        {
            Console.Write("Введіть ПІБ клієнта: ");
            string fullName = Console.ReadLine();

            string queryMax = $@"
                SELECT TOP 1 * FROM Salles s
                JOIN Clients c ON s.ClientId = c.Id
                WHERE c.FullName = N'{fullName}'
                ORDER BY s.Price DESC";

            string queryMin = $@"
                SELECT TOP 1 * FROM Salles s
                JOIN Clients c ON s.ClientId = c.Id
                WHERE c.FullName = N'{fullName}'
                ORDER BY s.Price ASC";

            using SqlCommand cmdMax = new SqlCommand(queryMax, conn);
            using SqlDataReader readerMax = cmdMax.ExecuteReader();

            Console.WriteLine("\nНайдорожча покупка:");
            if (readerMax.Read())
                Console.WriteLine($"Сума: {readerMax["Price"]}, Кількість: {readerMax["Quantity"]}");
            readerMax.Close();

            using SqlCommand cmdMin = new SqlCommand(queryMin, conn);
            using SqlDataReader readerMin = cmdMin.ExecuteReader();

            Console.WriteLine("Найдешевша покупка:");
            if (readerMin.Read())
                Console.WriteLine($"Сума: {readerMin["Price"]}, Кількість: {readerMin["Quantity"]}");
        }

        static void FirstSaleByEmployee(SqlConnection conn)
        {
            Console.Write("Введіть ПІБ працівника: ");
            string fullName = Console.ReadLine();

            string query = $@"
                SELECT TOP 1 * FROM Salles s
                JOIN Employees e ON s.EmployeeId = e.Id
                WHERE e.FullName = N'{fullName}'
                ORDER BY s.Id ASC";

            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\nНайперша продажа:");
            if (reader.Read())
                Console.WriteLine($"Сума: {reader["Price"]}, Кількість: {reader["Quantity"]}");
        }
    }
}