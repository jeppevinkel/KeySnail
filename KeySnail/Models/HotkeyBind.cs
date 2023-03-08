using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows.Input;
using GregsStack.InputSimulatorStandard.Native;
using KeySnail.Enums;

namespace KeySnail.Models;

public class HotkeyBind : INotifyPropertyChanged
{
    public KeyTypes FromKey { get; set; } = KeyTypes.NONE;
    public KeyTypes ToKey { get; set; } = KeyTypes.NONE;
    public float Delay { get; set; } = 0;
    public string ActiveWindow { get; set; } = string.Empty;
    [JsonIgnore] private bool _isActive = true;

    [JsonIgnore]
    public bool IsActive
    {
        get => _isActive;
        set => SetField(ref _isActive, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}