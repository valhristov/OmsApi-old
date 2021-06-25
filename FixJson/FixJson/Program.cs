using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FixJson
{
    class Program
    {
        private static readonly Regex _apiExamplesFixer = new("(\"example\"\\:\\s*?\\[)(?<wrong>['\\w,\\s]+)+(\\])");

        static void Main(string[] args)
        {
            var path = args.ElementAtOrDefault(0);
            if (string.IsNullOrWhiteSpace(path) ||
                !File.Exists(path))
            {
                Console.WriteLine("No file provided or file does not exist.");
                return;
            }

            var content = File.ReadAllText(path);

            var json = Fix(content);

            var document = JsonSerializer.Deserialize<object>(json);

            File.WriteAllText(path, JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true }));
        }

        private static string Fix(string json) =>
            // Some field examples have invalid JSON, this removes them
            _apiExamplesFixer.Replace(json, "\"example\":[]");
    }
}
