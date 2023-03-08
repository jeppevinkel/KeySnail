using System.Windows.Input;
using GregsStack.InputSimulatorStandard.Native;
using KeySnail.Enums;
using KeySnail.Utilities;

namespace KeySnail.Extensions;

public static class KeyConversionExtensions
{
    public static KeyTypes ToKeyType(this KeyboardHook.VKeys key)
    {
        return key switch
        {
            KeyboardHook.VKeys.KEY_A => KeyTypes.A,
            KeyboardHook.VKeys.KEY_B => KeyTypes.B,
            KeyboardHook.VKeys.KEY_C => KeyTypes.C,
            KeyboardHook.VKeys.KEY_D => KeyTypes.D,
            KeyboardHook.VKeys.KEY_E => KeyTypes.E,
            KeyboardHook.VKeys.KEY_F => KeyTypes.F,
            KeyboardHook.VKeys.KEY_G => KeyTypes.G,
            KeyboardHook.VKeys.KEY_H => KeyTypes.H,
            KeyboardHook.VKeys.KEY_I => KeyTypes.I,
            KeyboardHook.VKeys.KEY_J => KeyTypes.J,
            KeyboardHook.VKeys.KEY_K => KeyTypes.K,
            KeyboardHook.VKeys.KEY_L => KeyTypes.L,
            KeyboardHook.VKeys.KEY_M => KeyTypes.M,
            KeyboardHook.VKeys.KEY_N => KeyTypes.N,
            KeyboardHook.VKeys.KEY_O => KeyTypes.O,
            KeyboardHook.VKeys.KEY_P => KeyTypes.P,
            KeyboardHook.VKeys.KEY_Q => KeyTypes.Q,
            KeyboardHook.VKeys.KEY_R => KeyTypes.R,
            KeyboardHook.VKeys.KEY_S => KeyTypes.S,
            KeyboardHook.VKeys.KEY_T => KeyTypes.T,
            KeyboardHook.VKeys.KEY_U => KeyTypes.U,
            KeyboardHook.VKeys.KEY_V => KeyTypes.V,
            KeyboardHook.VKeys.KEY_W => KeyTypes.W,
            KeyboardHook.VKeys.KEY_X => KeyTypes.X,
            KeyboardHook.VKeys.KEY_Y => KeyTypes.Y,
            KeyboardHook.VKeys.KEY_Z => KeyTypes.Z,
            _ => KeyTypes.UNKNOWN
        };
    }

    public static KeyTypes ToKeyType(this Key key)
    {
        return key switch
        {
            Key.A => KeyTypes.A,
            Key.B => KeyTypes.B,
            Key.C => KeyTypes.C,
            Key.D => KeyTypes.D,
            Key.E => KeyTypes.E,
            Key.F => KeyTypes.F,
            Key.G => KeyTypes.G,
            Key.H => KeyTypes.H,
            Key.I => KeyTypes.I,
            Key.J => KeyTypes.J,
            Key.K => KeyTypes.K,
            Key.L => KeyTypes.L,
            Key.M => KeyTypes.M,
            Key.N => KeyTypes.N,
            Key.O => KeyTypes.O,
            Key.P => KeyTypes.P,
            Key.Q => KeyTypes.Q,
            Key.R => KeyTypes.R,
            Key.S => KeyTypes.S,
            Key.T => KeyTypes.T,
            Key.U => KeyTypes.U,
            Key.V => KeyTypes.V,
            Key.W => KeyTypes.W,
            Key.X => KeyTypes.X,
            Key.Y => KeyTypes.Y,
            Key.Z => KeyTypes.Z,
            _ => KeyTypes.UNKNOWN
        };
    }
    
    public static Key ToKey(this KeyTypes keyType)
    {
        return keyType switch
        {
            KeyTypes.A => Key.A,
            KeyTypes.B => Key.B,
            KeyTypes.C => Key.C,
            KeyTypes.D => Key.D,
            KeyTypes.E => Key.E,
            KeyTypes.F => Key.F,
            KeyTypes.G => Key.G,
            KeyTypes.H => Key.H,
            KeyTypes.I => Key.I,
            KeyTypes.J => Key.J,
            KeyTypes.K => Key.K,
            KeyTypes.L => Key.L,
            KeyTypes.M => Key.M,
            KeyTypes.N => Key.N,
            KeyTypes.O => Key.O,
            KeyTypes.P => Key.P,
            KeyTypes.Q => Key.Q,
            KeyTypes.R => Key.R,
            KeyTypes.S => Key.S,
            KeyTypes.T => Key.T,
            KeyTypes.U => Key.U,
            KeyTypes.V => Key.V,
            KeyTypes.W => Key.W,
            KeyTypes.X => Key.X,
            KeyTypes.Y => Key.Y,
            KeyTypes.Z => Key.Z,
            _ => Key.None
        };
    }

    public static KeyboardHook.VKeys ToVKey(this KeyTypes keyType)
    {
        return keyType switch
        {
            KeyTypes.A => KeyboardHook.VKeys.KEY_A,
            KeyTypes.B => KeyboardHook.VKeys.KEY_B,
            KeyTypes.C => KeyboardHook.VKeys.KEY_C,
            KeyTypes.D => KeyboardHook.VKeys.KEY_D,
            KeyTypes.E => KeyboardHook.VKeys.KEY_E,
            KeyTypes.F => KeyboardHook.VKeys.KEY_F,
            KeyTypes.G => KeyboardHook.VKeys.KEY_G,
            KeyTypes.H => KeyboardHook.VKeys.KEY_H,
            KeyTypes.I => KeyboardHook.VKeys.KEY_I,
            KeyTypes.J => KeyboardHook.VKeys.KEY_J,
            KeyTypes.K => KeyboardHook.VKeys.KEY_K,
            KeyTypes.L => KeyboardHook.VKeys.KEY_L,
            KeyTypes.M => KeyboardHook.VKeys.KEY_M,
            KeyTypes.N => KeyboardHook.VKeys.KEY_N,
            KeyTypes.O => KeyboardHook.VKeys.KEY_O,
            KeyTypes.P => KeyboardHook.VKeys.KEY_P,
            KeyTypes.Q => KeyboardHook.VKeys.KEY_Q,
            KeyTypes.R => KeyboardHook.VKeys.KEY_R,
            KeyTypes.S => KeyboardHook.VKeys.KEY_S,
            KeyTypes.T => KeyboardHook.VKeys.KEY_T,
            KeyTypes.U => KeyboardHook.VKeys.KEY_U,
            KeyTypes.V => KeyboardHook.VKeys.KEY_V,
            KeyTypes.W => KeyboardHook.VKeys.KEY_W,
            KeyTypes.X => KeyboardHook.VKeys.KEY_X,
            KeyTypes.Y => KeyboardHook.VKeys.KEY_Y,
            KeyTypes.Z => KeyboardHook.VKeys.KEY_Z,
            _ => KeyboardHook.VKeys.NONAME
        };
    }

    public static VirtualKeyCode ToVirtualKeyCode(this KeyTypes keyType)
    {
        return keyType switch
        {
            KeyTypes.A => VirtualKeyCode.VK_A,
            KeyTypes.B => VirtualKeyCode.VK_B,
            KeyTypes.C => VirtualKeyCode.VK_C,
            KeyTypes.D => VirtualKeyCode.VK_D,
            KeyTypes.E => VirtualKeyCode.VK_E,
            KeyTypes.F => VirtualKeyCode.VK_F,
            KeyTypes.G => VirtualKeyCode.VK_G,
            KeyTypes.H => VirtualKeyCode.VK_H,
            KeyTypes.I => VirtualKeyCode.VK_I,
            KeyTypes.J => VirtualKeyCode.VK_J,
            KeyTypes.K => VirtualKeyCode.VK_K,
            KeyTypes.L => VirtualKeyCode.VK_L,
            KeyTypes.M => VirtualKeyCode.VK_M,
            KeyTypes.N => VirtualKeyCode.VK_N,
            KeyTypes.O => VirtualKeyCode.VK_O,
            KeyTypes.P => VirtualKeyCode.VK_P,
            KeyTypes.Q => VirtualKeyCode.VK_Q,
            KeyTypes.R => VirtualKeyCode.VK_R,
            KeyTypes.S => VirtualKeyCode.VK_S,
            KeyTypes.T => VirtualKeyCode.VK_T,
            KeyTypes.U => VirtualKeyCode.VK_U,
            KeyTypes.V => VirtualKeyCode.VK_V,
            KeyTypes.W => VirtualKeyCode.VK_W,
            KeyTypes.X => VirtualKeyCode.VK_X,
            KeyTypes.Y => VirtualKeyCode.VK_Y,
            KeyTypes.Z => VirtualKeyCode.VK_Z,
            _ => VirtualKeyCode.NONAME,
        };
    }
}