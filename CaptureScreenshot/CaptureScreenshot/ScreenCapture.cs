using System;
using System.Drawing;
using System.Drawing.Imaging;
using CaptureScreenshot.WinAPI;

namespace CaptureScreenshot
{
	/// <summary>
	/// Функции для захвата всего экрана или определенного окна и сохранения его в файл
	/// </summary>
	public class ScreenCapture
	{
		/// <summary>
		/// Снимок экрана всего рабочего стола
		/// </summary>
		/// <returns>Объект Image, содержащий снимок</returns>
		public Image CaptureScreen()
		{
			return CaptureWindow(User32.GetDesktopWindow());
		}

		/// <summary>
		/// Снимок экрана всего рабочего стола
		/// </summary>
		/// <param name="handle">handle</param>
		/// <returns>Объект Image, содержащий снимок</returns>
		public Image CaptureWindow(IntPtr handle)
		{
			// Получить hDC целевого окна
			IntPtr hdcSrc = User32.GetWindowDC(handle);
			// Получить размер
			User32.RECT windowRect = new User32.RECT();
			User32.GetWindowRect(handle, ref windowRect);
			int width = windowRect.right - windowRect.left;
			int height = windowRect.bottom - windowRect.top;
			// Создаём для копирования целевое контекстое устройство
			IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			// Создаём bitmap для копирования данных туда,
			// используя GetDeviceCaps для получения длины и ширины
			IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
			// select the bitmap object
			IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
			// bitblt перезапись
			GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
			// Восстановить выбранное
			GDI32.SelectObject(hdcDest, hOld);
			// Очистка 
			GDI32.DeleteDC(hdcDest);
			User32.ReleaseDC(handle, hdcSrc);
			// Получаем .NET image объект
			Image img = Image.FromHbitmap(hBitmap);
			// Освобождаем Bitmap объект
			GDI32.DeleteObject(hBitmap);
			return img;
		}

		/// <summary>
		/// Делает снимок экрана определенного окна и сохраняет его в файл
		/// </summary>
		/// <param name="handle">handle</param>
		/// <param name="filename">Имя файла</param>
		/// <param name="format">Формат изображения</param>
		public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
		{
			Image img = CaptureWindow(handle);
			img.Save(filename, format);
		}

		/// <summary>
		/// Делает снимок экрана всего рабочего стола и сохраняет его в файл
		/// </summary>
		/// <param name="filename">Имя файла</param>
		/// <param name="format">Формат изображения</param>
		public void CaptureScreenToFile(string filename, ImageFormat format)
		{
			Image img = CaptureScreen();
			img.Save(filename, format);
		}
	}
}
