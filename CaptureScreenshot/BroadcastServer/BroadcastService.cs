using System;
using System.Drawing.Imaging;
using System.ServiceProcess;
using CaptureScreenshot;
using System.Timers;
using NLog;

namespace BroadcastServer
{
	public partial class BroadcastService : ServiceBase
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Timer timer;

		public BroadcastService()
		{
			InitializeComponent();
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
		}

		public void Stop()
		{
			logger.Info("Остановка службы.");
			timer.Dispose();
		}

		private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			logger.Trace($"Событие в {e.SignalTime}");

			try
			{
				timer.Stop();

				var screenCapture =  new ScreenCapture();
				var image = screenCapture.CaptureScreen();
				image.Save($"{DateTime.Now}.bmp", ImageFormat.Bmp);

				timer.Start();
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}
		}
	}
}
