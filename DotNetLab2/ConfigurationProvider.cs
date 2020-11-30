using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLab2
{
	class ConfigurationProvider
	{
		IConfigurationProvider parser;
		public void setParser(IConfigurationProvider parser)
		{
			this.parser = parser;
		}
		public Configuration Parse(string path)
		{
			return parser.Parse(path);
		}
	}
}
