using UnityEngine;
using System;
public class ExceptionHandler
{
    /// <summary>
    /// 将返回值为空的方法作为参数传入，可以检测异常情况；如果方法具有返回值，则需要用空方法套住。
    /// </summary>
    /// <param name="action">需要传入的一个空方法</param>
    /// <param name="shouldThrow">是否要抛出异常，默认继续执行而不抛出异常。</param>
    /// <returns></returns>
    public static bool Assert(Action action, bool shouldThrow = false)
    {
        try
        {
            action();
            return true;
        }
        catch (Exception exception)
        {
            if (shouldThrow)
            {
                throw exception;
            }
            else
            {
                Debug.LogError($"Exception occurred: {exception}");
            }
            return false;
        }
    }

    public static bool ExceptionLog(Exception exception)
    {
        Debug.LogError($"Exception occurred {exception}");
        return false;
    }
}

