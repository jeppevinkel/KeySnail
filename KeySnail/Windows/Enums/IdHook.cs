namespace KeySnail.Windows.Enums;

public enum IdHook
{
    WH_CALLWNDPROC = 4,
    WH_CALLWNDPROCRET = 12,
    WH_CBT = 5,
    WH_DEBUG = 9,
    WH_FOREGROUNDIDLE = 11,
    WH_GETMESSAGE = 3,
    WH_JOURNALPLAYBACK = 1,
    WH_JOURNALRECORD = 0,
    WH_KEYBOARD = 2,        // Installs a hook procedure that monitors keystroke messages.
    WH_KEYBOARD_LL = 13,    // Installs a hook procedure that monitors low-level keyboard input events.
    WH_MOUSE = 7,
    WH_MOUSE_LL = 14,
    WH_MSGFILTER = -1,
    WH_SHELL = 10,
    WH_SYSMSGFILTER = 6,
}