using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLab5
{
	public class FileWorker
	{
		private string GetFilePath(string fileName)
		{
			string filePath = Environment.CurrentDirectory;
			filePath = filePath.Substring(0, filePath.IndexOf("bin")) + fileName;
			return filePath;
		}

		public string[] ReadFile(string fileName)
		{
			string filePath = GetFilePath(fileName);
			string[] stringData = File.ReadAllLines(filePath);

			return stringData;
		}

		public void WriteInFile(string fileName, string[] dataArray)
		{
			dataArray.ToList().ForEach(s => WriteInFile(fileName, s));
		}

		public void WriteInFile(string fileName, string data)
		{
			string filePath = GetFilePath(fileName);

			using (StreamWriter sw = new(filePath))
			{
				sw.WriteLine(data);
			}
		}
	}
}
