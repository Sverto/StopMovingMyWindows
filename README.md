# Stop Moving My Windows
A tray application created to stop Windows from rearranging windows when the screen was turned off after idle time.

## How it works
The application hooks into the display power system notification and
- Stores the window positions when the screen turns off
- Restores the window positions when the screen turns back on

## Requires
.NET Framework 4.7.2