using System.Collections.Generic;
using System.Linq;
using KeySnail.Config;
using KeySnail.Models;

namespace KeySnail.Services;

public interface IKeyBindStore
{
    List<HotkeyBind> GetAll();
    void Save(IEnumerable<HotkeyBind> hotkeyBinds);
}

public class DbKeyBindStore : IKeyBindStore
{
    public List<HotkeyBind> GetAll()
    {
        return CoreConfig.Instance.HotkeyBinds;
    }

    public void Save(IEnumerable<HotkeyBind> hotkeyBinds)
    {
        CoreConfig.Instance.HotkeyBinds = hotkeyBinds.ToList();
        CoreConfig.Save();
    }
}