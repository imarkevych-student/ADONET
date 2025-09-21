using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_data_access.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public float Cost { get; set; }
        public string Producer { get; set; }
        public float Price { get; set; }
        public override string ToString()
        {
            return $"{Name,15}  {Type,15} {Quantity,5} {Cost,5} {Producer,15} {Price,5}";
        }

    }
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public override string ToString() => $"{Id}: {FullName}";
    }

    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public override string ToString() => $"{Id}: {FullName}";
    }
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public override string ToString()
        {
            return $"SaleId: {Id}, ProductId: {ProductId}, Price: {Price}, Quantity: {Quantity}, EmployeeId: {EmployeeId}, ClientId: {ClientId}";
        }
    }
}
