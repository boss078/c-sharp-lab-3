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
using System.Xml;

namespace DotNetLab2
{
	class XmlParser : IConfigurationProvider
  {
    CustomLogger customLogger = new CustomLogger("D:\\Study\\dotNetLabs\\DNL2_files\\log.txt");
    public Configuration Parse(string configPath)
    {
      string textToParse;
      try
      {
        using (var sr = new StreamReader(configPath))
        {
          XmlDocument xDoc = new XmlDocument();
          xDoc.Load(configPath);

          XmlElement xRoot = xDoc.DocumentElement;
          string targetPath = null;
          string sourcePath = null;

          customLogger.RecordEntry(xRoot.Name);

          foreach (XmlNode xnode in xRoot)
          {
            customLogger.RecordEntry(xnode.Name);
            switch (xnode.Name)
            {
              case "SourcePath":
                sourcePath = xnode.InnerText;
                break;
              case "TargetPath":
                targetPath = xnode.InnerText;
                break;
            }
          }
          if (targetPath == null || sourcePath == null)
            throw new IOException();

          Configuration config = new Configuration(sourcePath, targetPath);
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

