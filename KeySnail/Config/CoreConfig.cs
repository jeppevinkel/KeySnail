using System.Collections.Generic;
using GregsStack.InputSimulatorStandard.Native;
using KeySnail.Models;

namespace KeySnail.Config;

public class CoreConfig : BaseConfig<CoreConfig>
{
    public List<HotkeyBind> HotkeyBinds  { get; set; } = new ();
}