using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using KeySnail.Utilities;
using KeySnail.Windows.Enums;
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

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    // [DllImport("user32.dll")]
    // private static extern 

    private static bool _initialized = false;
    private static readonly WinEventDelegate WindowChangedInternalHandler = WinEventProc;

    #region PublicFacingEvents

    public delegate void WindowChangedHandler(object? sender, WindowChangedEventArgs eventArgs);

    public static event WindowChangedHandler OnWindowChanged = delegate { };

    public delegate void KeyboardEventHandler(object? sender, KeyboardEventArgs eventArgs);

    public static event KeyboardEventHandler OnKeyboardEvent = delegate { };

    #endregion

    #region HookVariables

    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _keyboardHookId = IntPtr.Zero;

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

    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr) KeyEvent.KEY_DOWN || wParam == (IntPtr) WM_KEYUP))
        {
            var vkCode = (KeyboardHook.VKeys) Marshal.ReadInt32(lParam);

            var eventArgs = new KeyboardEventArgs((KeyEvent) wParam, vkCode);
            
            OnKeyboardEvent(null, eventArgs);
            
            // Debug.WriteLine(eventArgs.PreventDefault);

            if (eventArgs.PreventDefault)
            {
                return (IntPtr) 1;
            }

            // if (vkCode == KeyboardHook.VKeys.KEY_H)
            // {
            //     Debug.WriteLine("Swallow!");
            //     return (IntPtr) 1;
            // }
        }

        return CallNextHookEx(_keyboardHookId, nCode, wParam, lParam);
    }

    public static void Init()
    {
        if (_initialized) return;

        _keyboardHookId = SetKeyboardHook(_proc);

        SetWinEventHook((uint) Events.SYSTEM_FOREGROUND, (uint) Events.SYSTEM_FOREGROUND, IntPtr.Zero,
            WindowChangedInternalHandler, 0, 0,
            (uint) Events.OUT_OF_CONTEXT);

        Application.Current.Exit += (sender, args) => { UnhookWindowsHookEx(_keyboardHookId); };
    }

    private static IntPtr SetKeyboardHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx((int) IdHook.WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
        uint dwEventThread, uint dwmsEventTime);

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);
}