﻿using System.Diagnostics;
using Newtonsoft.Json;


class Program
{
    static string dataFilePath = string.Empty;
    static string dataFileName = "give_me_items.json";
    static Dictionary<string, string> data = new();
    static Messages msg = new();

    private static string CheckWindowSystem()
    {
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
        process.StartInfo.ArgumentList.Add("-c");
        process.StartInfo.ArgumentList.Add("echo $XDG_SESSION_TYPE");
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output.Trim();
    }

    private static void ReadOrCreateFile()
    {
        if (!File.Exists(dataFilePath + dataFileName))
        {
            File.WriteAllText(dataFilePath + dataFileName, string.Empty);
        }
        else
        {
            string json = File.ReadAllText(dataFilePath + dataFileName);
            Dictionary<string, string>? dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (dict == null) { return; }

            data = dict;
            json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(dataFilePath + dataFileName, json);
        }
    }

    private static void SetValue(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Invalid amount of arguments.");
            return;
        }

        if (data.ContainsKey(args[1]))
        {
            Console.WriteLine($"The key [ {args[1]} ] already exists.");
            return;
        }

        string key = args[1];
        string value = args[2];
        data.Add(key, value);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(dataFilePath + dataFileName, json);
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

        string ws = CheckWindowSystem();

        if (ws == "wayland")
        {
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

        else if (ws == "x11")
        {
            TextCopy.ClipboardService.SetText(value);
        }
    }

    private static void UpdateValue(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Invalid amount of arguments.");
            return;
        }

        if (data.ContainsKey(args[1]))
        {
            data[args[1]] = args[2];
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(dataFilePath + dataFileName, json);
            Console.WriteLine("Updated!");
        }
        else
        {
            Console.WriteLine($"Key [ {args[1]} ] not found.");
        }
    }

    private static void DeleteValue(string[] args)
    {
        if (args.Length < 2) { return; }

        bool ok = data.Remove(args[1]);
        string text = ok ? "Deleted!" : $"Key [ {args[1]} ] not found.";
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(dataFilePath + dataFileName, json);
        Console.WriteLine(text);
    }

    private static void PrintAllKeys()
    {
        Console.WriteLine(string.Join(", ", data.Keys));
    }

    static void Main(string[] args)
    {
        if (args.Length == 0) { return; }

        dataFilePath = AppContext.BaseDirectory;

        ReadOrCreateFile();

        switch (args[0])
        {
            case "-D":
            case "--delete":
                DeleteValue(args);
                break;
            case "-S":
            case "--set":
                SetValue(args);
                break;
            case "-U":
            case "--update":
                UpdateValue(args);
                break;
            case "-L":
            case "--list":
                PrintAllKeys();
                break;
            case "-H":
            case "--help":
                Console.WriteLine(msg);
                break;
            default:
                GetValue(args);
                break;
        }
    }
}