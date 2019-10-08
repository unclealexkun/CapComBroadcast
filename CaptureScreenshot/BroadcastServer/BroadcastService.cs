using CapturingScreenshots;
using NLog;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace BroadcastServer
{
	public partial class BroadcastService : ServiceBase
	{
		private static string pathScreenshots = Directory.GetCurrentDirectory() + "\\Screenshots";

		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Timer timer;

		public BroadcastService()
		{
			InitializeComponent();
			if (!Directory.Exists(pathScreenshots)) Directory.CreateDirectory(pathScreenshots);
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
			logger.Trace($"Событие в {e.SignalTime}.{e.SignalTime.Millisecond}");

			try
			{
				var screenCapture = new ScreenCapture();
				using (var image = screenCapture.CaptureScreen())
				{
					image.Save($"{pathScreenshots}\\{e.SignalTime.ToString("yyyyMMddTHHmmssffff")}.bmp", ImageFormat.Bmp);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}
		}
	}
}
