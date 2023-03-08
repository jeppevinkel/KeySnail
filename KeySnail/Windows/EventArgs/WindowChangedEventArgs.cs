namespace KeySnail.Windows.EventArgs;

public class WindowChangedEventArgs : System.EventArgs
{
    public string NewWindowTitle { get; private set; }

    public WindowChangedEventArgs(string newWindowTitle)
    {
        NewWindowTitle = newWindowTitle;
    }
}