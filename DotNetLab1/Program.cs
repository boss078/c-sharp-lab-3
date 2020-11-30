using System;
using System.Collections.Generic;
using System.Runtime;

namespace DotNetLab1
{
	
	class Program
	{
		static void Main(string[] args)
		{
			FileManager fm = new FileManager("D:\\Study\\dotNetLabs\\DNL1_files\\");
			while (true)
			{
				Console.Write(fm.getPath() + ">");
				string cmd_raw = Console.ReadLine();
				string[] cmd = cmd_raw.Split(" ");
				string result;
				switch (cmd[0]) {
					case "ls":
						fm.PrintAllInThisFolder();
						break;
					case "touch":
						fm.CreateFile(cmd[1]);
						break;
					case "rm":
						fm.DeleteFile(cmd[1]);
						break;
					case "find":
						fm.FindFilePath(cmd[1]);
						break;
					case "cd":
						fm.AddToPath(cmd[1]);
						break;
					case "wrt":
						fm.Write(cmd[1], cmd[2]);
						break;
					case "rd":
						result = fm.Read(cmd[1]);
						if (result != null)
							Console.WriteLine(result);
						break;
					case "wrtc":
						fm.Write(cmd[1], cmd[2]);
						break;
					case "rdc":
						result = fm.ReadCompressed(cmd[1]);
						if (result != null)
							Console.WriteLine(result);
						break;
					case "exit":
						return;
				}
			}
		}
	}
}
