class Message
{
    public string argument;
    public string text;
}

class Messages
{
    public List<Message> repository = new();

    public Messages()
    {
        Message helpMsg = new();
        helpMsg.argument = "-H, --help";
        helpMsg.text = "Display this help";
        repository.Add(helpMsg);

        Message setMsg = new();
        setMsg.argument = "-S, --set";
        setMsg.text = "Set the key value pair. Usage: giveme -S <key> <value>";
        repository.Add(setMsg);

        Message listMsg = new();
        listMsg.argument = "-L, --list";
        listMsg.text = "List all stored keys.";
        repository.Add(listMsg);

        Message printMsg = new();
        printMsg.argument = "-P, --print";
        printMsg.text = "Prints the value to the console. Usage: giveme <key> -P";
        repository.Add(printMsg);
    }

    public override string ToString()
    {
        string output = "\x1b[1mgiveme\x1b[0m — stores key/value pairs. " +
                        "Inserts a value\n\t into the Wayland clipboard on demand.\n\n";

        foreach (var item in repository)
        {
            output += $"{item.argument}\t{item.text}\n";
        }

        return output;
    }
}