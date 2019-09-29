using System;
using System.Runtime.InteropServices;

namespace CapturingScreenshots.WinAPI
{
	internal class User32
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}
		[DllImport("user32.dll")]
		internal static extern IntPtr GetDesktopWindow();
		[DllImport("user32.dll")]
		internal static extern IntPtr GetWindowDC(IntPtr hWnd);
		[DllImport("user32.dll")]
		internal static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
		[DllImport("user32.dll")]
		internal static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
	}
}
