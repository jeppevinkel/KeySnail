using GregsStack.InputSimulatorStandard;

namespace KeySnail.Services;

public interface IInputSimulatorInstance
{
    InputSimulator Get();
}

public class InputSimulatorInstance : IInputSimulatorInstance
{
    private readonly InputSimulator _inputSimulator = new();

    public InputSimulator Get()
    {
        return _inputSimulator;
    }
}