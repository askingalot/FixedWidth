using System;
using System.Collections.Generic;
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

            var reader = new Reader(9, 6, 10, 10);
            var records = File.ReadAllLines(dataFile)
                .Select(line =>
                    new Record
                    {
                        FirstName = reader.GetString(line, 0),
                        LastName = reader.GetString(line, 1),
                        BirthDate = reader.GetDatetime(line, 2, "yyyy-MM-dd"),
                        DeathDate = reader.GetDatetime(line, 3, "yyyy-MM-dd"),
                    }
                ).ToList();

            records.ForEach(Console.WriteLine);
        }
    }


    public class Reader
    {
        private FileLayout Layout;

        public Reader(FileLayout layout)
        {
            Layout = layout;
        }

        public Reader(params int[] sizes) : this(new FileLayout(sizes)) { }
        public Reader(IEnumerable<int> sizes) : this(new FileLayout(sizes)) { }

        public string GetString(string record, int fieldIndex) =>
            GetString(record, Layout.FieldData[fieldIndex].Start, Layout.FieldData[fieldIndex].Length);

        public DateTime GetDatetime(string record, int fieldIndex, string format)
        {
            var str = GetString(record, fieldIndex);
            return DateTime.ParseExact(str, format, CultureInfo.InvariantCulture);
        }


        private string GetString(string record, int start, int length) =>
            record.Substring(start, length).Trim();

    }

    public class FileLayout
    {
        public (int Start, int Length)[] FieldData { get; }

        public FileLayout(params int[] sizes)
        {
            if (sizes == null || sizes.Length == 0)
            {
                throw new ArgumentException(nameof(sizes), 
                    $"{nameof(sizes)} must contain at least one element.");
            }

            FieldData = new(int, int)[sizes.Length];
            var start = 0;
            for(var i=0; i<sizes.Length; i++)
            {
                FieldData[i].Start = start;
                FieldData[i].Length = sizes[i];
                start += sizes[i];
            }
        }

        public FileLayout(IEnumerable<int> sizes) : this(sizes.ToArray()) { }
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