using System;
using System.Drawing;
using System.Drawing.Imaging;
using CaptureScreenshot.WinAPI;

namespace CaptureScreenshot
{
	public class ScreenCapture
	{
		public Image CaptureScreen()
		{
			return CaptureWindow(User32.GetDesktopWindow());
		}
	}
}
