using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] VideoPlayer _video;
    [SerializeField] string _nextScene;

    private void Update()
    {
        if (_video != null && Mathf.Round((float)_video.time * 10f) / 10 >= Mathf.Round((float)_video.length * 10f) / 10 - 0.2f)
            SceneManager.LoadScene(_nextScene);
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
