using UnityEngine;
using System.IO;
using System;


public class OperateFile
{

    /// <summary>
    /// 将文件转换成byte[]数组
    /// </summary>
    /// <param name="fileUrl">文件路径文件名称</param>
    /// <returns>byte[]数组</returns>
    public static byte[] FileToByte(string fileUrl)
    {
        try
        {
            using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 将byte[]数组保存成文件
    /// </summary>
    /// <param name="byteArray">byte[]数组</param>
    /// <param name="fileName">保存至硬盘的文件路径</param>
    /// <returns></returns>
    public static bool ByteToFile(byte[] byteArray, string fileName)
    {
        return ExceptionHandler.Assert(() =>
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        });
    }



    /// <summary>
    ///读取StreamingAssets中的文件 
    /// </summary>
    /// <param name="fileName">StreamingAssets下的文件路径</param>
    /// <returns>读取到的字符串</returns>
    public static string GetTextFromStreamingAssetsFile(string fileName)
    {
        string localPath = "";
        //二次读取！--- 容易漏改。
        if (Application.platform == RuntimePlatform.Android)
        {
            localPath = Application.streamingAssetsPath + "/" + fileName;
        }
        else
        {
            localPath = "file:///" + Application.streamingAssetsPath + "/" + fileName;
        }

        WWW www = new WWW(localPath);

        if (www.error != null)
        {
            Debug.LogError("error while reading files : " + localPath);
            return ""; //读取文件出错
        }
        while (!www.isDone) { }
        Debug.Log("File content :  " + www.text);//www下面还有获取字节数组的属性

        return www.text;
    }

    /// <summary>
    ///读取StreamingAssets中的文件 
    /// </summary>
    /// <param name="fileName">StreamingAssets下的文件路径</param>
    /// <returns>读取到的字节数组</returns>
    public static byte[] GetBytesFromStreamingAssetsFile(string fileName)
    {
        string localPath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            localPath = Application.streamingAssetsPath + "/" + fileName;
        }
        else
        {
            localPath = "file:///" + Application.streamingAssetsPath + "/" + fileName;
        }

        WWW www = new WWW(localPath);

        if (www.error != null)
        {
            Debug.LogError("error while reading files : " + localPath);
            return null; //读取文件出错
        }
        while (!www.isDone) { }
        Debug.Log("File content :  " + www.text);//www下面还有获取字节数组的属性
        return www.bytes;
    }



    /// <summary>
    ///  创建文件。
    /// </summary>
    /// <param name="path">路径</param>
    public static bool CreatFile(string path)
    {
        return ExceptionHandler.Assert(() =>
        {
            if (!System.IO.File.Exists(path))
            {
                WriteFile(path);
            }
        });
    }

    /// <summary>
    /// 写入数据。
    /// </summary>
    /// <param name="path">路径，尽量包含文件的后缀名。</param>
    /// <param name="content">需要写入的数据。</param>
    public static bool WriteFile(string path, string content = "")
    {
        return ExceptionHandler.Assert(() =>
        { //当输入的路径中不包含后缀分隔符时默认创建名为data的文本文件
            if (!path.Split('/')[path.Split('/').Length - 1].Contains("."))
            {
                path += "/data.txt";
            }
            string str = content;
            //若是直接在文件里添加数据，则采用重写方法，将append设置为true
            var file = new StreamWriter(path, true);
            //var file = new StreamWriter(path);
            //file.WriteLine(str);
            file.Write(str);
            file.Close();
        });
    }


    /// <summary>
    /// 读取文件。
    /// </summary>
    /// <param name="path">路径，尽量包含文件的后缀名。</param>
    public string ReadFile(string path)
    {
        //当输入的路径中不包含后缀分隔符时默认读取名为data的文本文件
        if (!path.Split('/')[path.Split('/').Length - 1].Contains("."))
        {
            path += "/data.txt";
        }
        using (StreamReader file = new StreamReader(path))
        {
            string fileContents = file.ReadToEnd();
            // Debug.Log("读取文件内容：" + fileContents);
            file.Close();
            return fileContents;
        }
    }


    /// <summary>
    /// 删除文件。
    /// </summary>
    /// <param name="path">路径，尽量包含文件的后缀名。</param>
    public bool DeleteSpecifiedFile(string path)
    {
        try
        {
            //当输入的路径中不包含后缀分隔符时默认删除名为data的文本文件
            if (!path.Split('/')[path.Split('/').Length - 1].Contains("."))
            {
                path += "/data.txt";
            }
            File.Delete(path);
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e}.");
        }
        return !File.Exists(path);
    }



    /// <summary>
    /// 删除指定文件目录下的所有文件。
    /// </summary>
    /// <param name="fullPath">文件路径</param>
    public bool DeleteAllFile(string fullPath)
    {
        //获取指定路径下面的所有资源文件  然后进行删除
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            Debug.Log(files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                string FilePath = fullPath + "/" + files[i].Name;
                // print(FilePath);//---> in Unity Editor
                File.Delete(FilePath);
            }
            return true;
        }
        return false;
    }



    /// <summary>
    /// 获取文件大小。
    /// </summary>
    /// <param name="path">路径，尽量包含文件的后缀名。</param>
    /// <returns></returns>
    public string GetFileSize(string path)
    {
        try
        {
            //当输入的路径中不包含后缀分隔符时默认获取名为data的文本文件
            if (!path.Split('/')[path.Split('/').Length - 1].Contains("."))
            {
                path += "/data.txt";
            }
            long fileLongSize = 0;
            if (File.Exists(path))
            {
                fileLongSize = new FileInfo(path).Length;
                return CountSize(fileLongSize);
            }
            else
            {
                return "Relevant file does not exist!";
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e}");
            return "";
        }
    }

    /// <summary>
    /// 计算文件大小函数(保留两位小数),Size为字节大小。
    /// </summary>
    /// <param name="Size">初始文件大小</param>
    /// <returns></returns>
    public string CountSize(long Size)
    {
        string m_strSize = "";
        long FactSize = 0;
        FactSize = Size;
        if (FactSize < 1024.00)
            m_strSize = FactSize.ToString("F2") + " Byte";
        else if (FactSize >= 1024.00 && FactSize < 1048576)
            m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
        else if (FactSize >= 1048576 && FactSize < 1073741824)
            m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
        else if (FactSize >= 1073741824)
            m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
        return m_strSize;
    }
}
