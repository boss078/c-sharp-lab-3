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

namespace DotNetLab2
{
	class CustomLogger
	{
    string logPath;
    public CustomLogger(string logPath)
		{
      this.logPath = logPath;
		}
    public void RecordEntry(object arg)
    {
        using (StreamWriter writer = new StreamWriter(logPath, true))
        {
          if (arg.GetType().Equals(typeof(Exception)))
          {
            Exception castedArg = (Exception)arg;
            writer.WriteLine(String.Format("Exception happend! Type: {0}, Message: {1}",
                castedArg.GetType(), castedArg.Message));
          }
          else
            writer.WriteLine(arg);
          writer.Flush();
        }
      
    }
    
  }
}
