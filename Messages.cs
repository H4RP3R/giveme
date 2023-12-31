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
        setMsg.text = "Set the key value pair. Usage: giveme -S <key> <value>\n\t\t" +
                        "Use double quotes for the value if you want to store several\n\t\t" +
                        "words. Example: giveme -S <key> \"<several words>\"";
        repository.Add(setMsg);

        Message updateMsg = new();
        updateMsg.argument = "-U, --update";
        updateMsg.text = "Update value by key. Usage: giveme -U <key> <value>\n\t\t" +
                        "Use double quotes if you want to set several words\n\t\t" +
                        "as a value. Example: giveme -U <key> \"<several words>\"";
        repository.Add(updateMsg);

        Message listMsg = new();
        listMsg.argument = "-L, --list";
        listMsg.text = "List all stored keys.";
        repository.Add(listMsg);

        Message printMsg = new();
        printMsg.argument = "-P, --print";
        printMsg.text = "Prints the value to the console. Usage: giveme <key> -P";
        repository.Add(printMsg);

        Message delMsg = new();
        delMsg.argument = "-D, --delete";
        delMsg.text = "Delete value by key. Usage: giveme -D <key>";
        repository.Add(delMsg);
    }

    public override string ToString()
    {
        string output = "\x1b[1mgiveme\x1b[0m — stores key/value pairs. Inserts a value into the Wayland\n\t " +
                        "clipboard on demand. Usage: giveme <key>\n\n";

        foreach (var item in repository)
        {
            output += $"{item.argument}\t{item.text}\n";
        }

        return output;
    }
}