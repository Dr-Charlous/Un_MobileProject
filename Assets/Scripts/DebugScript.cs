using TMPro;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _textUi;
    [SerializeField] CharaControl _chara;
    
    float _deltaTime = 0;
    float _timer = 0;

    private void Start()
    {
        DebugText($"Nothing", 0);
        DebugText("X", 2);
    }

    private void Update()
    {
        TimerDisplay();
    }

    private void FixedUpdate()
    {
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        DebugText(Mathf.Ceil(fps).ToString() + " FPS", 3);
    }

    void TimerDisplay()
    {
        _timer += Time.deltaTime;

        float distance = ((_chara.TargetInitialPos + _chara.TargetPos) - _chara.Obj.position).magnitude;

        DebugText($"Lerp : {Mathf.Round(distance * 100) / 100}\n{Mathf.Round(_timer * 10) / 10}", 1);
    }

    public void DebugText(string txt, int value)
    {
        _textUi[value].text = txt;
    }
}
