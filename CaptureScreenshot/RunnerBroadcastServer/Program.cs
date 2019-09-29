using System;
using System.ServiceProcess;
using BroadcastServer;

namespace RunnerBroadcastServer
{
	class Program
	{
		static void Main(string[] args)
		{
			var service = new BroadcastService();
			if (Environment.UserInteractive)
			{
				service.Start();
				Console.WriteLine("Press any key to stop program...");
				while (!Console.KeyAvailable)
				{
				}
				service.Stop();
			}
			else
			{
				ServiceBase.Run(service);
			}
		}
	}
}
