using System;
using System.Collections;
using System.Runtime.InteropServices;

/*by Aizierjiang*/

public class CuctomizedException : Exception
{
    protected ExceptionResult exceptionResult;

    public ExceptionResult GetExceptionResult()
    {
        return exceptionResult;
    }


    public class ExceptionResult
    {
        public string resultStatus { get; set; }
        public string resultMsg { get; set; }
    }
}


public class InputException : CuctomizedException
{
    public InputException(string resultStatus, string resultMsg) 
    {
        base.exceptionResult = new ExceptionResult { resultMsg = resultMsg, resultStatus = resultStatus };
    }
}
