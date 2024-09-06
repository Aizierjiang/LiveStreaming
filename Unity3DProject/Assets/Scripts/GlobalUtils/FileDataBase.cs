/*
 * 文件数据库v1.0
 * 用于保存下载资源
 * 根据需求进行以下功能点增加
 * 1、配置文件数据表
 * 2、异步IO
 * 3、批量并发
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using Logger;

public class FileDataBase
{
    private static FileDataBase m_instance = new FileDataBase();
#if UNITY_ANDROID && !UNITY_EDITOR
    private string m_dataBase = Application.persistentDataPath + "/appFolder/db/";
#else
    private string m_dataBase = "./obj/appFolderDB/";
#endif
    private ReaderWriterLockSlim writeLock = new ReaderWriterLockSlim();

    // private FileDataBase() { CheckAndCreateDir(m_dataBase); }

    // private void CheckAndCreateDir(string dir)
    // {
    //     try
    //     {
    //         if (!Directory.Exists(dir))
    //             Directory.CreateDirectory(dir);
    //     }
    //     catch (IOException e)
    //     {
    //         new Log(e.Message, LogType.Error);
    //     }
    // }

    public bool CheckFile(string name)
    {
        try
        {
            return File.Exists(name);
        }
        catch (IOException e)
        {
            return ExceptionHandler.ExceptionLog(e);
        }
    }

    public static FileDataBase Get() { return m_instance; }

    /// <summary>
    /// 保存文件，如果文件存在则不保存（除非开启overwrite）
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="byteArray">文件数据</param>
    /// <param name="overwrite">是否覆写</param>
    /// <returns></returns>
    public bool SaveFile(string fileName, byte[] byteArray, bool overwrite = false)
    {

        string url = GetFileUrl(fileName);
        bool fileExist = CheckFile(url);
        if (overwrite && fileExist)
        {
            File.Delete(url);
        }

        bool result = false;

        if (fileExist)
            return true;

        try
        {
            //防止异步操作的时候冲突
            writeLock.EnterWriteLock();
            using (FileStream fs = new FileStream(url, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
                result = true;
            }
        }
        catch (Exception e)
        {
            return ExceptionHandler.ExceptionLog(e);
        }
        finally
        {
            writeLock.ExitWriteLock();
        }
        return result;
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="fileUrl">文件名</param>
    /// <returns></returns>
    public byte[] LoadFile(string fileName)
    {
        string url = GetFileUrl(fileName);

        if (CheckFile(url) == false)
            return null;
        try
        {
            using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e}");
            return null;
        }
    }

    public string GetFileUrl(string name)
    {
        return m_dataBase + name;
    }

    public void ClearCache()
    {
        Directory.Delete(m_dataBase, true);
    }
}
