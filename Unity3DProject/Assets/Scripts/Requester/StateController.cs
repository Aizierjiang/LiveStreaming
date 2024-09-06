using UnityEngine;

/*by Aizierjiang*/

public class StateController : MonoBehaviour
{
    private static object m_mutex = new object();
    private static bool m_initialized = false;
    private static StateController stateController = null;

    // Singleton in thread-safe-mode
    public static StateController Instance
    {
        get
        {
            if (!m_initialized)
            {
                lock (m_mutex)
                {

                    if (stateController == null)
                    {
                        stateController = new StateController();
                        m_initialized = true;
                    }
                }
            }
            return stateController;
        }
    }

    private string m_state;
    public string State
    {
        get
        {
            return m_state;
        }
        set
        {
            lock (m_mutex)
            {
                m_state = value;
            }
        }
    }
}
