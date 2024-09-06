# The rules of writing scripts


#### Author: Aizierjiang 



1. The accessibility of the life cycle methods which belongs to Unity 3D itself should not be stated. 

    ```C#
    void Awake(){}
    void OnEnable(){}
    void Start(){}
    ...
    ```


2. The accessibility of the methods created by the developers should all be stated. 

    ```C#
    private void myOwnMethod(){}
    protected string myOwnProtectedMethod(){}
    public bool myOwnPublicMethod(){} 
    ...
    ```


3. All the variables should be initialized when defined and should state its accessibility.

   ```C#
   public string myNane = "Aizierjiang";
   private int mMyNumber = 0;
   protected bool _isProgramer = true;
   ...
   ```


4. The variables should be written in camel-case and the first letter should always be in lower case.
    ```C#
    public string alexNane = "Aizierjiang";
    protected bool isProgramer = true;
    private int mMyNumber = 0;
    ...
    ```


5. The names of public functions or methods should not start with lowercased letters.
    ```C#
    public void MyMethod(){}
    public bool YourMethod(){}
    public string HisMethod(){}
    ...
    ```


6. The names of private functions or methods should start with lower-cased letters.

    ```C#
    private void myOwnMethod(){}
    private int yourOwnMethod(){}
    private string hisMethod(){}
    ...
    ```


7. The names of protected functions or methods should start with upper-case letters.

   ```c#
   protected string MyProtectedMethod(){}
   protected int YourProtectedMethod(){}
   protected bool HisProtected(){}
   ...
   ```


8. The names of protected properties should start with "_".

   ```C#
   protected string _myProtectedProperty = "Soft";
   protected int _yourProtectedProperty = 0;
   protected bool _hisProtectedProperty = false;
   ...
   ```


9. Private members including properties and fields should always start with letter "m".
   
    ```C#
    private int mMyNumber = 0;
    private string mMyName = "Aizierjiang";
    private bool mIsProgramer = true;
    ...
    ```


11. Interface name should start with letter "I".
    
    ```C#
    interface IMyInterface{}
    interface IChecker{}
    ...
    ```


12. Static variables should start with letter "s".

    ```C#
    public static int sMyNumber = 0;
    protected static bool sIsNumber = false;
    private static string sHisNumber = "null";
    ...
    ```


13. The static constant variables should all be written in capitalized styles and "_" can be used to connect each vocabularies.

    ```C#
    public static const int PI = 3,1415;
    protected static const bool IS_PI = false;
    private static const string SERVER_URL = "http://127.0.0.1/";
    ...
    ```


14. The abbreviations should start with upper-cased letter and other letters should be lower-cased if it is not publicly known to all with its all-upper-cased abbreviated type.

    ```C#
    private int pwdGetter(int name, bool isVIP){};
    public string GetAccount(int name, string pwd){};
    protected bool MsgSender(){};
    public static void HTTPRequestSender(){};
    ...
    ```


15. Underlines or bars are not acceptable among the names of non-static and non-constant property names, fields, functions or methods except the cases mentioned above. 


<br><br><br>

<p align="right">by Aizierjiang Aiersilan</p>
<p align="right">8th Sep, 2020</p>