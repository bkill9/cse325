// A little bit of confusion with the sales report. Wasn't sure if I was
// supposed to use "sales.json" or "salestotals.json". I went with
// "sales.json" since that's what the Microsoft tutorial was using.

// I also had to separate this file from the rest of the project files
// in order to push my work to my GitHub.

using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Globalization;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Data.Common;
using System.Text.Json.Nodes;
using System.Text.Json;

var salesFiles = FindFiles("stores");
GenerateSalesReport(salesFiles);

foreach (var file in salesFiles)
{
    Console.WriteLine(file);
}

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        // The file name will contain the full path, so only check the end of it
        if (file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

void GenerateSalesReport(IEnumerable<string> salesFiles)
{
    decimal totalSales = 0;

    foreach (var file in salesFiles)
    {
        string jsonContent = File.ReadAllText(file);
        using JsonDocument doc = JsonDocument.Parse(jsonContent);
        decimal storeSales = doc.RootElement.GetProperty("Total").GetDecimal();
        // Console.WriteLine(storeSales);

        totalSales += storeSales;
    }

    StringBuilder report = new StringBuilder();
    report.AppendLine("Sales Summary");
    report.AppendLine("-------------------------");
    report.AppendLine($"Total Sales: {totalSales:C}");
    report.AppendLine();
    report.AppendLine("Details:");

    foreach (var file in salesFiles)
    {
        string jsonContent = File.ReadAllText(file);
        using JsonDocument doc = JsonDocument.Parse(jsonContent);
        decimal storeSales = doc.RootElement.GetProperty("Total").GetDecimal();

        report.AppendLine($"{file}: {storeSales:C}");
    }

    File.WriteAllText("summary.txt", report.ToString());
}
