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
  public partial class Service1 : ServiceBase
  {
    FileProcessor logger;
    public Service1()
    {
      InitializeComponent();
      this.CanStop = true;
      this.CanPauseAndContinue = true;
      this.AutoLog = true;
    }
    internal void TestStartupAndStop()
    {
      string[] fakeArgs = new String[0];
      this.OnStart(fakeArgs);
      Console.ReadLine();
      this.OnStop();
    }
    protected override void OnStart(string[] args)
    {
      ConfigurationProvider configProvider = new ConfigurationProvider();
      //configProvider.setParser(new JsonParser());
      //Configuration config = configProvider.Parse("D:\\Study\\dotNetLabs\\DNL2_files\\config.json");
      configProvider.setParser(new XmlParser());
      Configuration config = configProvider.Parse("D:\\Study\\dotNetLabs\\DNL2_files\\config.xml");
      if (config == null)
        return;

      logger = new FileProcessor(config);
      Thread loggerThread = new Thread(new ThreadStart(logger.Start));
      loggerThread.Start();
    }

    protected override void OnStop()
    {
      logger.Stop();
      Thread.Sleep(1000);
    }
  }

  class FileProcessor
  {
    FileSystemWatcher watcher;
    CustomLogger customLogger = new CustomLogger("D:\\Study\\dotNetLabs\\DNL2_files\\log.txt");
    object obj = new object();
    bool enabled = true;
    string sourcePath;
    string targetPath;

    public FileProcessor(Configuration config)
    {
      sourcePath = config.getSourcePath();
      targetPath = config.getTargetPath();
      customLogger.RecordEntry($"sourcePath: {sourcePath}, targetPath: {targetPath}");
      watcher = new FileSystemWatcher(sourcePath);
      watcher.IncludeSubdirectories = true;
      watcher.Created += OnCreated;
      watcher.Filter = "*.txt";
      watcher.EnableRaisingEvents = true;
    }

    public void Start()
    {
      watcher.EnableRaisingEvents = true;
      while (enabled)
      {
        Thread.Sleep(1000);
      }
    }
    public void Stop()
    {
      watcher.EnableRaisingEvents = false;
      enabled = false;
    }
    // создание файлов
    private void OnCreated(object sender, FileSystemEventArgs args)
    {
      customLogger.RecordEntry("onCreated fired!");
      string[] tempPath = args.FullPath.Replace(sourcePath, "").Split('\\');
      string year = tempPath[1];
      string month = tempPath[2];
      string day = tempPath[3];
      string fileName = tempPath[4];
      string saveFilePath = Path.Combine(targetPath, year, month, day);

      //RecordEntry(args.Name);
      string text = Read(args.FullPath);
      if (text == null) return;

      Directory.CreateDirectory(saveFilePath);
      text = Xor(text, "ineedmorepower");
      customLogger.RecordEntry("Encrypted as " + text);
      WriteCompressed(Path.Combine(saveFilePath, fileName), text);

      Directory.CreateDirectory(saveFilePath);
      text = ReadCompressed(Path.Combine(saveFilePath, fileName));
      text = Xor(text, "ineedmorepower");
      customLogger.RecordEntry("Decrypted as " + text);


      Directory.CreateDirectory(Path.Combine(targetPath, "archive", year, month, day));
      Write(Path.Combine(targetPath, "archive", year, month, day, fileName), text);
    }

    public void Write(string filePath, string text)
    {
      try
      {
        using (var sw = new StreamWriter(filePath))
        {
          sw.WriteLine(text);
        }
      }
      catch (IOException e)
      {
        customLogger.RecordEntry(e);
      }
      finally
      {
        customLogger.RecordEntry($"{Path.GetFileName(filePath)}: write successful");
      }
    }
    public string Read(string filePath)
    {
      try
      {
        using (var sr = new StreamReader(filePath))
        {
          string text = sr.ReadToEnd();
          return text;
        }
      }
      catch (IOException e)
      {
        customLogger.RecordEntry(e);
        return null;
      }
      finally
      {
        customLogger.RecordEntry($"{Path.GetFileName(filePath)}: read successful");
      }
    }
    public string ReadCompressed(string filePath)
    {
      FileStream fileStream = new FileStream(filePath, FileMode.Open);
      GZipStream compressedStream = new GZipStream(fileStream, CompressionMode.Decompress);
      StreamReader compressedReader = new StreamReader(compressedStream);

      using (fileStream)
      using (compressedStream)
      using (compressedReader)
      {
        try
        {
          string result = compressedReader.ReadToEnd();
          return result;
        }
        catch (Exception e)
        {
          customLogger.RecordEntry(e);
          return null;
        }
        finally
        {
          customLogger.RecordEntry($"{Path.GetFileName(filePath)}: compressed read successful");
        }
      }
    }
    public void WriteCompressed(string filePath, string text)
    {
      FileStream fileStream = new FileStream((filePath), FileMode.OpenOrCreate);
      GZipStream compressedStream = new GZipStream(fileStream, CompressionMode.Compress);
      StreamWriter compressedWriter = new StreamWriter(compressedStream);

      using (fileStream)
      using (compressedStream)
      using (compressedWriter)
      {
        try
        {
          compressedWriter.WriteLine(text);
        }
        catch (Exception e)
        {
          customLogger.RecordEntry(e);
        }
        finally
        {
          customLogger.RecordEntry($"{Path.GetFileName(filePath)}: compressed write successful");
        }
      }
    }
    public string Xor(string text, string key)
    {
      while (text.Length > key.Length)
        key += key;
      if (text.Length < key.Length)
        key = key.Substring(0, text.Length);

      char[] textCharArray = text.ToCharArray();
      char[] keyCharArray = key.ToCharArray();

      for (int i = 0; i < text.Length; i++)
        textCharArray[i] = (char)(textCharArray[i] ^ keyCharArray[i]);

      return new string(textCharArray);
    }

  }
}
