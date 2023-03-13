using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GregsStack.InputSimulatorStandard;
using KeySnail.Enums;
using KeySnail.Extensions;
using KeySnail.Models;
using KeySnail.Utilities;
using KeySnail.Windows;
using KeySnail.Windows.Enums;
using Prism.Commands;
using Prism.Mvvm;

namespace KeySnail.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private InputSimulator _inputSimulator = null;
    private KeyboardHook _keyboardHook = null;
    private Services.IKeyBindStore _keyBindStore = null;
    
    private string _toggleButton = "Disable";
    private string _activeWindow = string.Empty;

    public string ToggleButton
    {
        get => _toggleButton;
        set => SetProperty(ref _toggleButton, value);
    }

    private bool _hotkeyEnabled = true;

    public bool HotkeyEnabled
    {
        get => _hotkeyEnabled;
        set
        {
            SetProperty(ref _hotkeyEnabled, value);

            ToggleButton = value == true ? "Disable" : "Enable";
        }
    }

    public MainWindowViewModel(Services.IInputSimulatorInstance inputSimulatorInstance,
        Services.IKeyBindStore keyBindStore,
        Services.IKeyboardService keyboardServiceInstance)
    {
        _inputSimulator = inputSimulatorInstance.Get();
        _keyboardHook = keyboardServiceInstance.Get();
        _keyBindStore = keyBindStore;

        KeyBinds.Clear();
        List<HotkeyBind> list = _keyBindStore.GetAll();
        foreach (HotkeyBind item in list)
            KeyBinds.Add(item);

        _keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyDown);
        _keyboardHook.KeyUp += new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyUp);

        _keyboardHook.Install();

        Application.Current.Exit += (sender, args) =>
        {
            _keyboardHook.KeyDown -= new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyDown);
            _keyboardHook.KeyUp -= new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyUp);
            _keyboardHook.Uninstall();
        };

        WindowsApiStuff.Init();
        WindowsApiStuff.OnWindowChanged += (sender, args) =>
        {
            WriteLog($"Active window changed to {args.NewWindowTitle}");
            _activeWindow = args.NewWindowTitle;

            foreach (var keyBind in KeyBinds)
            {
                keyBind.IsActive = CompareWindowStrings(_activeWindow, keyBind.ActiveWindow);
                WriteLog(keyBind.FromKey + "->" + keyBind.ToKey + ": " + keyBind.IsActive);
            }
        };

        // HashSet<KeyboardHook.VKeys> keysBeingPress = new HashSet<KeyboardHook.VKeys>();
        // HashSet<KeyboardHook.VKeys> keysBeingRelease = new HashSet<KeyboardHook.VKeys>();

        // WindowsApiStuff.OnKeyboardEvent += (sender, args) =>
        // {
        //     if (args.EventType == KeyEvent.KEY_DOWN)
        //     {
        //         if (!keysBeingPress.Contains(args.Key))
        //         {
        //             WriteLog($"Swallowed {args.Key.ToKeyType()} DOWN");
        //             keysBeingPress.Add(args.Key);
        //             args.PreventDefault = true;
        //             PressKeyWithDelay(args.Key.ToKeyType(), 2);
        //             return;
        //         }
        //         
        //         WriteLog($"Pressing {args.Key.ToKeyType()}");
        //         keysBeingPress.Remove(args.Key);
        //     }
        //     
        //     if (args.EventType == KeyEvent.KEY_UP)
        //     {
        //         if (!keysBeingRelease.Contains(args.Key))
        //         {
        //             WriteLog($"Swallowed {args.Key.ToKeyType()} UP");
        //             keysBeingRelease.Add(args.Key);
        //             args.PreventDefault = true;
        //             ReleaseKeyWithDelay(args.Key.ToKeyType(), 2);
        //             return;
        //         }
        //
        //         WriteLog($"Releasing {args.Key.ToKeyType()}");
        //         keysBeingRelease.Remove(args.Key);
        //     }
        //     
        // };
    }

    public ObservableCollection<HotkeyBind> KeyBinds { get; } =
        new();

    public ObservableCollection<string> Log { get; } = new();

    private Dictionary<KeyboardHook.VKeys, List<HotkeyBind>> _pressedKeys = new();

    private DelegateCommand? _commandAddKeyBind = null;

    public DelegateCommand CommandAddKeyBind =>
        _commandAddKeyBind ??= new DelegateCommand(CommandAddKeyBindExecute);

    private DelegateCommand? _commandSave = null;

    public DelegateCommand CommandSave =>
        _commandSave ??= new DelegateCommand(CommandSaveExecute);

    private DelegateCommand? _commandToggleProgram = null;

    public DelegateCommand CommandToggleProgram =>
        _commandToggleProgram ??= new DelegateCommand(CommandToggleProgramExecute);

    private void CommandAddKeyBindExecute()
    {
        KeyBinds.Add(new HotkeyBind());
    }

    private void CommandSaveExecute()
    {
        _keyBindStore.Save(KeyBinds);
        WriteLog($"Saving...");
    }

    private void CommandToggleProgramExecute()
    {
        HotkeyEnabled = !HotkeyEnabled;
        WriteLog(HotkeyEnabled ? "Key binds enabled!" : "Key binds disabled!");
    }

    private void WriteLog(object content)
    {
        Log.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {content}");
    }

    private Dictionary<KeyTypes, HotkeyEventData> _cancellationTokens = new();

    private void KeyboardHook_KeyDown(KeyboardHook.VKeys key)
    {
        if (_pressedKeys.ContainsKey(key))
            return;
        
        var keyType = key.ToKeyType();

        var relevantKeyBinds = KeyBinds.Where(keyBind => keyBind.IsActive && keyBind.FromKey == keyType).ToList();
        if (HotkeyEnabled && relevantKeyBinds.Count > 0)
        {
            var tokenSource = new CancellationTokenSource();
            _cancellationTokens.Add(keyType, new HotkeyEventData(tokenSource));
            
            foreach (var keyBind in relevantKeyBinds)
            {
                // _inputSimulator.Keyboard.KeyDown(keyBind.ToKey.ToVirtualKeyCode());
                PressKeyWithDelay(keyBind.ToKey, keyBind.Delay, tokenSource.Token);
            }
        }
        
        _pressedKeys.Add(key, relevantKeyBinds);
            
        WriteLog($"Pressed {key.ToString()}");
    }

    private void KeyboardHook_KeyUp(KeyboardHook.VKeys key)
    {
        var keyType = key.ToKeyType();
        var cancelled = false;
        if (_cancellationTokens.TryGetValue(keyType, out HotkeyEventData hotkeyEventData))
        {
            var duration = DateTime.Now.Subtract(hotkeyEventData.StartTime);
            WriteLog($"{keyType} released after {duration.TotalMilliseconds}ms");

            if (duration.TotalMilliseconds < 250)
            {
                hotkeyEventData.TokenSource.Cancel();
                cancelled = true;
                WriteLog($"Cancelled the press of {keyType}");
            }
            _cancellationTokens.Remove(keyType);
        }
        
        if (!cancelled && HotkeyEnabled && _pressedKeys.TryGetValue(key, out var relevantKeyBinds))
        {
            foreach (var keyBind in relevantKeyBinds)
            {
                // _inputSimulator.Keyboard.KeyUp(keyBind.ToKey.ToVirtualKeyCode());
                ReleaseKeyWithDelay(keyBind.ToKey, keyBind.Delay);
            }
        }
        
        _pressedKeys.Remove(key);

        WriteLog($"Released {key.ToString()}");
    }

    private async Task PressKeyWithDelay(KeyTypes key, float delaySeconds, CancellationToken? cancellationToken = null)
    {
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));

        if (cancellationToken?.IsCancellationRequested ?? false)
        {
            return;
        }
        
        _inputSimulator.Keyboard.KeyDown(key.ToVirtualKeyCode());
    }

    private async Task ReleaseKeyWithDelay(KeyTypes key, float delaySeconds, CancellationToken? cancellationToken = null)
    {
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        
        if (cancellationToken?.IsCancellationRequested ?? false)
        {
            return;
        }
        
        _inputSimulator.Keyboard.KeyUp(key.ToVirtualKeyCode());
    }

    private bool CompareWindowStrings(string windowTitle, string? needle)
    {
        if (needle is null || needle.Length == 0 || windowTitle.Contains(needle))
        {
            return true;
        }
        
        return false;
    }
}