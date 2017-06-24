using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FixedWidth.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataFile = $"..{Path.DirectorySeparatorChar}data.txt";

            var records = File.ReadAllLines(dataFile)
                .Select(line =>
                    new Record
                    {
                        FirstName = line.Substring(0, 9).Trim(),
                        LastName = line.Substring(9, 6).Trim(),
                        BirthDate = DateTime.ParseExact(line.Substring(15, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DeathDate = DateTime.ParseExact(line.Substring(25, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    }
                ).ToList();

            records.ForEach(Console.WriteLine);
        }
    }

    public class Record
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DeathDate { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} was born on {BirthDate.ToString("dd/MM/yyyy")} and died on {DeathDate.ToString("dd/MM/yyyy")}";
        }
    }
}