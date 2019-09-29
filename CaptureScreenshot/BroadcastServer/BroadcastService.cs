using System;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceProcess;
using CapturingScreenshots;
using System.Timers;
using NLog;

namespace BroadcastServer
{
	public partial class BroadcastService : ServiceBase
	{
		private static string path = Directory.GetCurrentDirectory() + "\\Screenshots";

		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Timer timer;

		public BroadcastService()
		{
			InitializeComponent();
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		}

		protected override void OnStart(string[] args)
		{
			Start();
		}

		protected override void OnStop()
		{
			Stop();
		}

		public void Start()
		{
			logger.Info("Запуск службы...");

			logger.Trace("Инициализация таймера");
			var interval = Properties.Settings.Default.Interval;
			timer = new Timer(interval.TotalMilliseconds);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;

			timer.Start();
		}

		public void Stop()
		{
			timer.Stop();
			logger.Info("Остановка службы.");
			timer.Dispose();
		}

		private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			logger.Trace($"Событие в {e.SignalTime}");

			try
			{
				var screenCapture = new ScreenCapture();
				var image = screenCapture.CaptureScreen();
				image.Save($"{path}\\{DateTime.Now}.bmp", ImageFormat.Bmp);
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}
		}
	}
}
