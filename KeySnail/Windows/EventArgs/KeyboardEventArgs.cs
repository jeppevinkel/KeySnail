using KeySnail.Utilities;
using KeySnail.Windows.Enums;

namespace KeySnail.Windows.EventArgs;

public class KeyboardEventArgs : System.EventArgs
{
    public KeyEvent EventType { get; }
    public KeyboardHook.VKeys Key { get; }
    public bool PreventDefault { get; set; } = false;

    public KeyboardEventArgs(KeyEvent eventType, KeyboardHook.VKeys key)
    {
        EventType = eventType;
        Key = key;
    }
}