using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GregsStack.InputSimulatorStandard;
using KeySnail.Enums;
using KeySnail.Extensions;
using KeySnail.Models;
using KeySnail.Utilities;
using Prism.Commands;
using Prism.Mvvm;

namespace KeySnail.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private InputSimulator _inputSimulator = null;
    private KeyboardHook _keyboardHook = null;
    private Services.IKeyBindStore _keyBindStore = null;
    
    private string _toggleButton = "Disable";

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

    private void KeyboardHook_KeyDown(KeyboardHook.VKeys key)
    {
        if (_pressedKeys.ContainsKey(key))
            return;

        var relevantKeyBinds = KeyBinds.Where(keyBind => keyBind.FromKey == key.ToKeyType()).ToList();
        if (HotkeyEnabled && relevantKeyBinds.Count > 0)
        {
            foreach (var keyBind in relevantKeyBinds)
            {
                // _inputSimulator.Keyboard.KeyDown(keyBind.ToKey.ToVirtualKeyCode());
                PressKeyWithDelay(keyBind.ToKey, keyBind.Delay);
            }
        }
        
        _pressedKeys.Add(key, relevantKeyBinds);
            
        WriteLog($"Pressed {key.ToString()}");
    }

    private void KeyboardHook_KeyUp(KeyboardHook.VKeys key)
    {
        if (HotkeyEnabled && _pressedKeys.TryGetValue(key, out var relevantKeyBinds))
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

    private async Task PressKeyWithDelay(KeyTypes key, float delaySeconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        _inputSimulator.Keyboard.KeyDown(key.ToVirtualKeyCode());
    }

    private async Task ReleaseKeyWithDelay(KeyTypes key, float delaySeconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        _inputSimulator.Keyboard.KeyUp(key.ToVirtualKeyCode());
    }
}