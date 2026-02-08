public class GameManager : Singleton<GameManager>
{

    private bool m_isPaused = false;

    public bool IsBlocked
    {
        get
        {
            var blocked = 
                m_isPaused ||
                Manager_Dialog.Instance.IsShowing;

            return blocked;
        }
    }

    private void Pause()
    {
        m_isPaused = true;
    }

    private void Unpause()
    {
        m_isPaused = false;
    }

    void OnEnable()
    {
        Manager_Events.GameManager.Pause += Pause;
        Manager_Events.GameManager.Unpause += Unpause;
    }

    void OnDisable()
    {
        Manager_Events.GameManager.Pause -= Pause;
        Manager_Events.GameManager.Unpause -= Unpause;
    }

}
