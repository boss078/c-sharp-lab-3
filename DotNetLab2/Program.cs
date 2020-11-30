using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLab2
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if (args.FirstOrDefault()?.ToUpper() == "/CONSOLE")
			{
				RunAsConsole();
			}
			else
			{
				RunAsService();
			}
		}
		private static void RunAsConsole()
		{
			Service1 service1 = new Service1();
			service1.TestStartupAndStop();
		}
		private static void RunAsService()
		{
			/* Warning: Don't load the object graph or 
			 * initialize anything in here. 
			 * 
			 * Initialize everything in TestService.StartService() instead
			 */
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
								new Service1()
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
