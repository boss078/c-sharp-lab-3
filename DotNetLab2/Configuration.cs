using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLab2
{
	class Configuration
	{
		private string sourcePath;
		private string targetPath;
		public Configuration(string sourcePath, string targetPath)
		{
			this.sourcePath = sourcePath;
			this.targetPath = targetPath;
		}
		public string getSourcePath()
		{
			return sourcePath;
		}
		public string getTargetPath()
		{
			return targetPath;
		}
	}
}
