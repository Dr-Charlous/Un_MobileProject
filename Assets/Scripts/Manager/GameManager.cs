using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public DebugScript DebugScript;
    public SceneLoader SceneLoader;
    public GameObject SlapUi;
    public Animator SlapAnim;
    public CamFollow CamFollow;
    public Scoring Score;
}
