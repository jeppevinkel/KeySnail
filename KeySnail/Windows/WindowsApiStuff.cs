using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using KeySnail.Windows.EventArgs;

namespace KeySnail.Windows;

public static class WindowsApiStuff
{
    [DllImport("user32.dll")]
    private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
        WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    // [DllImport("user32.dll")]
    // private static extern 

    private static bool _initialized = false;
    private static readonly WinEventDelegate WindowChangedInternalHandler = WinEventProc;

    #region PublicFacingEvents

    public delegate void WindowChangedHandler(object? sender, WindowChangedEventArgs eventArgs);

    public static event WindowChangedHandler OnWindowChanged = delegate { };

    #endregion

    public static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        var buff = new StringBuilder(nChars);
        var handle = GetForegroundWindow();

        return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : string.Empty;
    }

    private static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
        uint dwEventThread, uint dwmsEventTime)
    {
        // var handle = GetForegroundWindow();


        switch ((Events) eventType)
        {
            case Events.SYSTEM_FOREGROUND:
                OnWindowChanged(null, new WindowChangedEventArgs(GetActiveWindowTitle()));
                break;
        }
    }

    public static void Init()
    {
        if (_initialized) return;

        SetWinEventHook((uint) Events.SYSTEM_FOREGROUND, (uint) Events.SYSTEM_FOREGROUND, IntPtr.Zero,
            WindowChangedInternalHandler, 0, 0,
            (uint) Events.OUT_OF_CONTEXT);
    }

    private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
        uint dwEventThread, uint dwmsEventTime);
}