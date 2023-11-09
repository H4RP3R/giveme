using System.Diagnostics;
using Newtonsoft.Json;


class Program
{
    static string fileName = "give_me_items.json";
    static Dictionary<string, string> data = new();
    static Messages msg = new();

    private static void ReadOrCreateFile()
    {
        if (!File.Exists(fileName))
        {
            File.WriteAllText(fileName, string.Empty);
        }
        else
        {
            string json = File.ReadAllText(fileName);
            Dictionary<string, string>? dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (dict == null) { return; }

            foreach (var item in dict)
            {
                data.Add(item.Key, item.Value);
            }
            json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
    }

    private static void SetValue(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Invalid amount of arguments.");
            return;
        }
        string key = args[1];
        string value = args[2];
        data.Add(key, value);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(fileName, json);
        Console.WriteLine("Saved!");
    }

    private static void GetValue(string[] args)
    {
        // Printout the value if "-p" argument is present. Otherwise insert the value into the clipboard.
        string value = string.Empty;
        bool print = false;
        data.TryGetValue(args[0], out value);

        foreach (string arg in args)
        {
            if (arg == "-P" || arg == "--print")
            {
                print = true;
            }
        }

        if (print)
        {
            Console.WriteLine(value);
            return;
        }

        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };
        process.Start();
        process.StandardInput.WriteLineAsync($"wl-copy {value}");
    }

    private static void PrintAllKeys()
    {
        Console.WriteLine(string.Join(", ", data.Keys.ToArray()));
    }

    static void Main(string[] args)
    {
        if (args.Length == 0) { return; }

        ReadOrCreateFile();

        switch (args[0])
        {
            case "-S":
                SetValue(args);
                break;
            case "--set":
                SetValue(args);
                break;
            case "-L":
                PrintAllKeys();
                break;
            case "--list":
                PrintAllKeys();
                break;
            case "-H":
                Console.WriteLine(msg);
                break;
            case "--help":
                Console.WriteLine(msg);
                break;
            default:
                GetValue(args);
                break;
        }
    }
}