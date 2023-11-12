# giveme
A command-line utility that allows to store key/value pairs and insert values into the Wayland or X11 clipboard on demand.
## Dependencies
`wl-clipboard` for Wayland  
`xsel` for X11
## Usage
`giveme <key>`
| Argument | Description |  
|---|---|
-H, --help| Display this help
-S, --set | Set key value pair. Usage: `giveme -S <key> <value>`. Use double quotes for the value if you want to store several words. Example: `giveme -S <key> "<several words>"`.
-U, --update | Update value by key. Usage: giveme `-U <key> <value>`.
-L, --list | List all stored keys.
-P, --print | Print the value to the console. Usage: `giveme <key> -P`.
-D, --delete | Delete value by key. Usage: `giveme -D <key>`.
