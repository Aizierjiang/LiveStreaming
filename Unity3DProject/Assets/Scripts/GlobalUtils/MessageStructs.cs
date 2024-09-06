using LitJson;

/*by Aizierjiang*/


class UserState
{
    public static readonly string Online = "Online";
    public static readonly string Chatting = "Chatting";
    public static readonly string Offline = "Offline";
    public static readonly string Pushing = "Pushing";
}
class CallResult
{
    public static readonly string Accepted = "Accepted";
    public static readonly string Rejected = "Rejected";
    public static readonly string StandStill = "StandStill";
}


//JsonData gReqExample = new JsonData();
//gReqExample["HeartBeat"] = true;
//gReqExample["ResponseCode"] = "NULL";
//gReqExample["UserState"] = UserState.Online;
//gReqExample["Calling"] = false;
//gReqExample["Called"] = false;
//gReqExample["CallResult"] = CallResult.StandStill;
//gReqExample["CallerUUID"] = JsonMapper.ToObject(PageManager.uuidFromHTTPServer)["fullName"].ToJson().ToString();
//gReqExample["CalleeUUID"] = "GetFromList";
//gReqExample["CallerStreamAddress"] = "rtsp://alexander:soft@" + Toolip.GetMyIPAddress() + ":8554";
//gReqExample["CalleeStreamAddress"] = "Got from list || Got from called resonse";