using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textScore;
    [SerializeField] GameObject _victoryUi;
    [SerializeField] Vector3 _camVictorySpeed;
    [SerializeField] int _score = 0;

    private void Start()
    {
        _victoryUi.SetActive(false);
    }

    public void UpdateScore(int value)
    {
        int count = GameManager.Instance.SceneLoader.Characters.Count;

        _score += value;
        _textScore.text = $"{_score} / {count}";

        if (_score >= count)
            Victory();
    }

    void Victory()
    {
        var player = GameManager.Instance.DebugScript.Chara.GetComponentInChildren<Animator>();
        player.SetBool("IsVictorious", true);
        _victoryUi.SetActive(true);
        GameManager.Instance.CamFollow.Target = player.transform;
        GameManager.Instance.CamFollow.Speed = _camVictorySpeed;
    }
}
