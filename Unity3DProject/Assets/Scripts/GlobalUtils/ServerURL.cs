/*by Aizierjiang*/

public class ServerURL
{
    public static string BASE_URL = Toolip.GetHTTPBaseUrl();
    public static string WSS_URL = Toolip.GetWebSocketBaseUrl();

    public static readonly string NET_CHECK = BASE_URL + "net_check";
    public static readonly string REGISTER = BASE_URL + "register";
    public static readonly string LOGIN = BASE_URL + "log_in";
    public static readonly string CLIENT_LOG = BASE_URL + "client_log";
    public static readonly string IMG_BASE = BASE_URL + "img_base";
    public static readonly string NEW_SHOW = BASE_URL + "new_show";
    public static readonly string UPDATE_SHOW_STATE = BASE_URL + "update_show_state";
    public static readonly string GET_SHOWLIST = BASE_URL + "get_showlist"; // GET all the show list(back-end returns the online shows de facto) 
    public static readonly string GET_ONLINE_LIST = BASE_URL + "get_online"; // GET online user

    public static readonly string RANDOM_NAME = BASE_URL + "random_name";
    public static readonly string LOOK_UP = BASE_URL + "look_up";
}
