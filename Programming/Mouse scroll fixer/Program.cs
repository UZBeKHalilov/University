using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

class MouseScrollMonitor
{
    // P/Invoke for mouse hook
    private const int WH_MOUSE_LL = 14;
    private static LowLevelMouseProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    // Variables to track scroll behavior
    private static long lastScrollTime = 0;
    private static string lastScrollDirection = "";
    private static int scrollCount = 0;

    public static void Main()
    {
        _hookID = SetHook(_proc);
        Console.WriteLine("Mouse scroll monitoring started. Press Ctrl+C to exit.");
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelMouseProc proc)
    {
        using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
        using (var curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_MOUSEWHEEL)
        {
            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            short scrollDelta = (short)((hookStruct.mouseData >> 16) & 0xFFFF);

            // Determine scroll direction (positive = up, negative = down)
            string direction = scrollDelta > 0 ? "Up" : "Down";
            long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; // Time in milliseconds

            // Check for rapid direction switches (e.g., up then down, or vice versa)
            if (lastScrollDirection != "" && lastScrollDirection != direction &&
                (currentTime - lastScrollTime < 50)) // 50ms window for rapid switches
            {
                Console.WriteLine($"Scroll error detected! Rapid switch from {lastScrollDirection} to {direction} at {currentTime}ms.");
                scrollCount = 0; // Reset counter after error
            }
            else if (currentTime - lastScrollTime < 100) // 100ms window for repeated scrolls
            {
                scrollCount++;
                if (scrollCount >= 10) // Adjust this threshold for "10 times" jumps
                {
                    Console.WriteLine($"Scroll error detected! {scrollCount} rapid {direction} scrolls at {currentTime}ms.");
                    scrollCount = 0;
                }
            }
            else
            {
                scrollCount = 1; // Reset if too much time has passed
            }

            lastScrollTime = currentTime;
            lastScrollDirection = direction;
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    // Struct and constants for mouse hook
    private struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    private const int WM_MOUSEWHEEL = 0x020A;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}