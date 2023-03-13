using System;
using System.Threading;

namespace KeySnail.Models;

public class HotkeyEventData
{
    public CancellationTokenSource TokenSource;
    public DateTime StartTime;

    public HotkeyEventData(CancellationTokenSource tokenSource, DateTime? startTime = null)
    {
        TokenSource = tokenSource;

        StartTime = startTime ?? DateTime.Now;
    }
}