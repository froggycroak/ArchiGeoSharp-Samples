using System.Text;
using ArchiGeoSharp.Samples;

Console.OutputEncoding = Encoding.UTF8;

string? id = args.Length > 0 ? args[0].Trim() : null;

if (string.IsNullOrWhiteSpace(id) || id is "-h" or "--help" or "help" or "list")
{
    SampleCatalog.PrintMenu();
    if (string.IsNullOrWhiteSpace(id) || id is "list")
        return 0;
    return 0;
}

if (!SampleCatalog.TryRun(id, out string? error))
{
    Console.Error.WriteLine(error);
    Console.Error.WriteLine();
    SampleCatalog.PrintMenu();
    return 1;
}

return 0;
