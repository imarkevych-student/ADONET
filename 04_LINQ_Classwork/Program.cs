using System;
using System.Linq;
using System.Collections.Generic;


namespace _04_LINQ_Classwork
{
    class Program
    {
        static void Main()
        {
            // Завдання 1: Позитивні числа по зростанню
            var numbers1 = new[] { -5, 3, 0, 7, -2, 10 };
            var positiveSorted = numbers1.Where(n => n > 0).OrderBy(n => n).ToList();

            // Завдання 2: Кількість і середнє двозначних позитивних
            var numbers2 = new[] { 5, 12, 99, -15, 8, 23 };
            var twoDigitPositives = numbers2.Where(n => n >= 10 && n <= 99);
            int count = twoDigitPositives.Count();
            double average = twoDigitPositives.Average();

            // Завдання 3: Високосні роки по зростанню
            var years = new[] { 1999, 2000, 2004, 1900, 2020, 2100 };
            var leapYears = years
                .Where(y => (y % 4 == 0 && y % 100 != 0) || (y % 400 == 0))
                .OrderBy(y => y)
                .ToList();

            // Завдання 4: Максимальне парне значення
            var numbers4 = new[] { 3, 8, 15, 22, 7, 10 };
            int maxEven = numbers4.Where(n => n % 2 == 0).Max();

            // Завдання 5: Додати три знаки оклику
            var strings5 = new[] { "Hello", "World", "LINQ" };
            var modified = strings5.Select(s => s + "!!!").ToList();

            // Завдання 6: Строки з певним символом
            char symbol = 'a';
            var strings6 = new[] { "apple", "banana", "cherry", "kiwi" };
            var containsSymbol = strings6.Where(s => s.Contains(symbol)).ToList();

            // Завдання 7: Групування по довжині рядка
            var strings7 = new[] { "cat", "dog", "elephant", "ant", "lion" };
            var grouped = strings7.GroupBy(s => s.Length);
       
            Console.WriteLine("Завдання 1: " + string.Join(", ", positiveSorted));
            Console.WriteLine($"Завдання 2: Кількість = {count}, Середнє = {average:F2}");
            Console.WriteLine("Завдання 3: " + string.Join(", ", leapYears));
            Console.WriteLine("Завдання 4: Максимальне парне = " + maxEven);
            Console.WriteLine("Завдання 5: " + string.Join(", ", modified));
            Console.WriteLine("Завдання 6: " + string.Join(", ", containsSymbol));
            Console.WriteLine("Завдання 7:");
            foreach (var group in grouped)
            {
                Console.WriteLine($"  Довжина {group.Key}: {string.Join(", ", group)}");
            }
        }

    }
}
