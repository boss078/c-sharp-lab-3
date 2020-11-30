using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace DotNetLab2
{
	class JsonParser : IConfigurationProvider
	{
    CustomLogger customLogger = new CustomLogger("D:\\Study\\dotNetLabs\\DNL2_files\\log.txt");
		public Configuration Parse(string configPath)
		{
      string textToParse;
      try
      {
        using (var sr = new StreamReader(configPath))
        {
          textToParse = sr.ReadToEnd();
          Configuration config = JsonConvert.DeserializeObject<Configuration>(textToParse);
          return config;
        }
      }
      catch (IOException e)
      {
        customLogger.RecordEntry(e);
        return null;
      }
    }
	}
}
