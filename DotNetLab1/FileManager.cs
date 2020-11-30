using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Globalization;

namespace DotNetLab1
{
	class FileManager
	{
		public FileManager(string filePath)
		{
			currentPath = filePath;
		}
		private string currentPath;
		public void setPath(string newPath)
		{
			currentPath = newPath;
		}
		public void AddToPath(string addPath)
		{
			currentPath = Path.Combine(currentPath, addPath);
		}
		public string getPath()
		{
			return currentPath;
		}
		public string	Read(string fileName)
		{
			try
			{
				using(var sr = new StreamReader(Path.Combine(currentPath, fileName)))
				{
					string text = sr.ReadToEnd();
					return text;
				}
			}
			catch (IOException e)
			{
				Console.WriteLine("Error occured during file read");
				Console.WriteLine("Message: " + e.Message);
				return null;
			}
		}
		public void PrintAllInThisFolder() {
			string[] folders = Directory.GetDirectories(currentPath);
			string[] files = Directory.GetFiles(currentPath);
			Console.WriteLine("#Folders");
			if (folders.Length == 0)
				Console.WriteLine("#empty");
			else
				foreach(string folder in folders)
					Console.WriteLine(Path.GetFileName(folder));
			Console.WriteLine("#Files");
			if (files.Length == 0)
				Console.WriteLine("#empty");
			else
				foreach (string file in files)
					Console.WriteLine(Path.GetFileName(file));
		}

		public void Write(string fileName, string text)
		{
			try
			{
				using (var sw = new StreamWriter(Path.Combine(currentPath, fileName)))
				{
					sw.WriteLine(text);
				}
			}
			catch (IOException e)
			{
				Console.WriteLine("Error occured during file write");
				Console.WriteLine("Message: " + e.Message);
			}
		}
		public string ReadCompressed(string fileName) {
			FileStream fileStream = new FileStream(Path.Combine(currentPath, fileName), FileMode.Open);
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
					Console.WriteLine("Some unhandled shit happend");
					Console.WriteLine("Message: " + e.Message);
					return null;
				}
			}
		}
		public void WriteCompressed(string fileName, string text)
		{
			FileStream fileStream = new FileStream(Path.Combine(currentPath, fileName), FileMode.OpenOrCreate);
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
					Console.WriteLine("Some unhandled shit happend");
					Console.WriteLine("Message: " + e.Message);
				}
			}
		}
		public void Duplicate(string fileName, string copyName = "Copy")
		{
			string content;
			if (copyName == "Copy")
				copyName = Path.Combine(Path.GetFileNameWithoutExtension(fileName) + "-copy" + Path.GetExtension(fileName));
			content = Path.GetExtension(fileName) == ".gz" ? ReadCompressed(fileName) : Read(fileName);
			if (Path.GetExtension(fileName) == ".gz")
				WriteCompressed(copyName, content);
			else
				Write(copyName, content);
		}
		public void CreateFile(string fileName)
		{
			File.Create(Path.Combine(currentPath, fileName));
		}
		public void DeleteFile(string fileName)
		{
			File.Delete(Path.Combine(currentPath, fileName));
		}
		public void FindFilePath(string fileName)
		{
			string result =  findRecursivlyFile(fileName, currentPath);
			if (result != null)
				Console.WriteLine(result);
			else
				Console.WriteLine("#Not found");
		}
		private string findRecursivlyFile(string fileName, string currentSearchPath)
		{
			foreach (string path in Directory.GetFiles(currentSearchPath))
				if (Path.GetFileName(path) == fileName)
					return Path.GetFullPath(path);
			foreach (string path in Directory.GetDirectories(currentSearchPath))
			{
				string result = findRecursivlyFile(fileName, path);
				if (result != null)
					return result;
			}
			return null;
		}
	}
}
