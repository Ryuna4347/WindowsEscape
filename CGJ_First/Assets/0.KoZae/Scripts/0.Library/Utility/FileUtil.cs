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
                if (Application.platform == RuntimePlatform.WindowsEditor)
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
                    return SecurityUtility.DecryptData(File.ReadAllText(path),"Layer_Game");
                }
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

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                writer.WriteLine(_data);
            }
            else
            {
                writer.WriteLine(SecurityUtility.EncryptData(_data,"Layer_Game"));
            }

            writer.Close();
            file.Close();
        }

        public static string GetFilePath(string _fileName)
        {
            if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return Path.Combine(Application.persistentDataPath,_fileName);
            }
            else
            {
                var path = Application.dataPath;
                path = path.Substring(0,path.LastIndexOf('/'));

                return Path.Combine(path,_fileName);
            }
        }
    }
}