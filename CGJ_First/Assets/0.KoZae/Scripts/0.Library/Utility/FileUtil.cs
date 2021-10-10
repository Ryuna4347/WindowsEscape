using System.IO;
using UnityEngine;

namespace KZLib
{
    public static class FileUtil
    {
        public static string ReadDataFromFile(string _filename)
        {
            string path = GetFilePath(_filename);

            if(File.Exists(path))
            {
                var file = new FileStream(path,FileMode.Open,FileAccess.Read);
                var stream = new StreamReader(file);
                var data = stream.ReadToEnd();

                stream.Close();
                file.Close();

                return data;
            }
            else
            {
                return null;
            }
        }

        public static void WriteDataToFile(string _data,string _fileName)
        {
            var file = new FileStream(GetFilePath(_fileName),FileMode.Create,FileAccess.Write);
            var writer = new StreamWriter(file);

            writer.WriteLine(_data);

            writer.Close();
            file.Close();
        }

        public static string GetFilePath(string _fileName)
        {
            var path = Application.dataPath;
            path = path.Substring(0,path.LastIndexOf('/'));

            return Path.Combine(path,_fileName);
        }
    }
}