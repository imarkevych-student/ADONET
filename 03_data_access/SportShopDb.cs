using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using _03_data_access.Models;

namespace _02_CRUD_Interface
{
    public class SportShopDb : IDisposable
    {
        private SqlConnection sqlConnection;
        private readonly string connectionString;
        public SportShopDb(string connectionString)
        {
            this.connectionString = connectionString;
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
        //C       R    U       D 
        //Create Read Update Delede
        public void Create_As_Insert(Product product)
        {
            string cmdText = @"INSERT INTO Products (Name, TypeProduct, Quantity, CostPrice, Producer, Price)
                       VALUES (@name, @type, @quantity, @cost, @producer, @price)";

            using SqlCommand command = new SqlCommand(cmdText, sqlConnection);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@type", product.Type);
            command.Parameters.AddWithValue("@quantity", product.Quantity);
            command.Parameters.AddWithValue("@cost", product.Cost);
            command.Parameters.AddWithValue("@producer", product.Producer);
            command.Parameters.AddWithValue("@price", product.Price);

            int rows = command.ExecuteNonQuery();
            Console.WriteLine($"{rows} rows affected!");
        }


        private List<Product> GetProductsByQuery(SqlDataReader reader)
        {         
            //Console.OutputEncoding = Encoding.UTF8;
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(
                    new Product()
                    {
                        Id = (int)reader[0],
                        Name = (string)reader[1],
                        Type = (string)reader[2],
                        Quantity = (int)reader[3],
                        Cost = (int)reader[4],
                        Producer = (string)reader[5],
                        Price = (int)reader[6]
                    });
            }
            reader.Close();
            return products;
        }
        public List<Product> Read_Get_All() 
        {
            string cmdText = $@"select * from Products";
            SqlCommand command = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            return GetProductsByQuery(reader);    
        }
        public List<Product> Get_By_Name(string _user_name)
        {
            string cmdText = $@"select * from Products where Name = @name";
            SqlCommand command = new SqlCommand(cmdText, sqlConnection);
            //command.Parameters.Add("name", System.Data.SqlDbType.NVarChar).Value = _user_name;
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "name",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Value = _user_name
            };
            command.Parameters.Add(parameter);  

            SqlDataReader reader = command.ExecuteReader();
            return GetProductsByQuery(reader);
        }
        public Product GetOne(int id)
        {
            string cmdText = @"SELECT * FROM Products WHERE Id = @id";
            using SqlCommand command = new SqlCommand(cmdText, sqlConnection);
            command.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader = command.ExecuteReader();
            return GetProductsByQuery(reader).FirstOrDefault()!;
        }


        public void Update(Product product)
        {
            string cmdText = @"UPDATE Products
                       SET Name = @name,
                           TypeProduct = @type,
                           Quantity = @quantity,
                           CostPrice = @cost,
                           Producer = @producer,
                           Price = @price
                       WHERE Id = @id";

            using SqlCommand command = new SqlCommand(cmdText, sqlConnection);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@type", product.Type);
            command.Parameters.AddWithValue("@quantity", product.Quantity);
            command.Parameters.AddWithValue("@cost", product.Cost);
            command.Parameters.AddWithValue("@producer", product.Producer);
            command.Parameters.AddWithValue("@price", product.Price);
            command.Parameters.AddWithValue("@id", product.Id);

            command.ExecuteNonQuery();
        }


        public void Delete(int id)
        {
            string cmdText = @"DELETE FROM Products WHERE Id = @id";
            using SqlCommand command = new SqlCommand(cmdText, sqlConnection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }



        public void Dispose()
        {
            sqlConnection.Close();  
        }

        public void CreateSale()
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

        public void ReadSalesByPeriod()
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

        public void ReadLastPurchaseByClient()
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

        public void DeletePerson()
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

        public void ReadTopEmployee()
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

        public List<Client> GetAllClients()
        {
            var clients = new List<Client>();
            string query = "SELECT Id, FullName FROM Clients";
            using (var cmd = new SqlCommand(query, sqlConnection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt32(0),
                        FullName = reader.GetString(1)                       
                    });
                }
            }
            return clients;
        }

        public List<Employee> GetAllEmployees()
        {
            string query = @"SELECT Id, FullName FROM Employees";
            using SqlCommand cmd = new SqlCommand(query, sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            List<Employee> employees = new();
            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1)
                });
            }
            return employees;
        }

        public List<Sale> GetAllSales()
        {
            string query = @"SELECT s.Id, s.ProductId, s.Price, s.Quantity, s.EmployeeId, s.ClientId
                     FROM Salles s";
            using SqlCommand cmd = new SqlCommand(query, sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            List<Sale> sales = new();
            while (reader.Read())
            {
                sales.Add(new Sale
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    Price = reader.GetDecimal(2),
                    Quantity = reader.GetInt32(3),
                    EmployeeId = reader.GetInt32(4),
                    ClientId = reader.GetInt32(5)
                });
            }
            return sales;
        }
    }
}
