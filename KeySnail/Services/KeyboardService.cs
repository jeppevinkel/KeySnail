using KeySnail.Utilities;

namespace KeySnail.Services;

public interface IKeyboardService
{
    KeyboardHook Get();
}

public class KeyboardServiceInstance : IKeyboardService
{
    private readonly KeyboardHook _keyboardHook = new();

    public KeyboardHook Get()
    {
        return _keyboardHook;
    }
}