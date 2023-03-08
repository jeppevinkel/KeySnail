using System.Windows.Input;
using GregsStack.InputSimulatorStandard.Native;
using KeySnail.Enums;

namespace KeySnail.Models;

public class HotkeyBind
{
    public KeyTypes FromKey { get; set; } = KeyTypes.NONE;
    public KeyTypes ToKey { get; set; } = KeyTypes.NONE;
    public float Delay { get; set; } = 0;
}