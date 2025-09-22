namespace _06_ShopsDb
{
    using System;
    using System.Configuration;
    using Microsoft.EntityFrameworkCore;

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ShopDb"].ConnectionString;

            using var context = new ShopDbContext(connectionString);

            context.Database.Migrate();

            Console.WriteLine("Початкові дані:");

            foreach (var country in context.Countries.Include(c => c.Cities))
            {
                Console.WriteLine($"{country.Name}");
                foreach (var city in country.Cities)
                    Console.WriteLine($"   {city.Name}");
            }

            foreach (var category in context.Categories)
                Console.WriteLine($"Категорія: {category.Name}");

            foreach (var position in context.Positions)
                Console.WriteLine($"Посада: {position.Name}");

            Console.WriteLine("\nІніціалізація завершена.");
        }
    }
}
