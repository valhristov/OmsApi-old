using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FixJson
{
    class Program
    {
        private static readonly Regex _apiExamplesFixer = new("(\"example\"\\:\\s*?\\[)(?<wrong>['\\w,\\s]+)+(\\])");
        private static readonly Regex _apiOperationIdFixer = new("(\"operationId\"\\:\\s*?\"\\w+?)(_\\d+?)(\",)");

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

            // Debugger.Launch();

            var json = FixExamples(content);
            json = FixOperationIds(json);

            if (json.Contains("GET_"))
            {
                Debugger.Launch();
            }
            var document = JsonSerializer.Deserialize<object>(json);
            var indented = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(path, indented);
        }

        private static string FixExamples(string json) =>
            // Some field examples have invalid JSON, this removes them
            _apiExamplesFixer.Replace(json, "\"example\":[]");

        private static string FixOperationIds(string json) =>
            // Some field examples have invalid JSON, this removes them
            _apiOperationIdFixer.Replace(json, "$1$3");
    }
}
