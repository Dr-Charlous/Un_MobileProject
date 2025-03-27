using TMPro;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    [SerializeField] SceneLoader _sceneLoad;
    [SerializeField] TextMeshProUGUI[] _textUi;
    [SerializeField] Transform _obj;
    [SerializeField] float _distanceInput = 1;
    [SerializeField] float _distanceMove = 1;
    [SerializeField][Range(0f, 1f)] float _lerpSpeed = 0.5f;

    Touch _touch;
    Vector2? _initialePos;
    Vector2? _endPos;
    Vector3 _targetInitialPos;
    Vector3 _targetPos;
    float _deltaTime = 0;
    float _timer = 0;

    private void Start()
    {
        _targetInitialPos = _obj.position;
        DebugText($"Nothing", 0);
        DebugText("X", 2);
    }

    private void Update()
    {
        HandleInput();
        TimerDisplay();

        _obj.position = Vector3.Lerp(_obj.position, _targetInitialPos + _targetPos, _lerpSpeed);
    }

    private void FixedUpdate()
    {
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        DebugText(Mathf.Ceil(fps).ToString() + " FPS", 3);
    }

    void MoveTargetSet()
    {
        Vector2 direction = Vector2.zero;

        if (_initialePos != null && _endPos != null)
        {
            direction = (Vector2)_endPos - (Vector2)_initialePos;

            if (direction != Vector2.zero && direction.magnitude >= _distanceInput)
            {
                Vector2Int intDirection = GetDirection(direction);

                if (_sceneLoad.GetPosDirection(intDirection))
                {
                    _targetInitialPos = _obj.position;
                    _targetPos = new Vector3(intDirection.x, _obj.position.y, intDirection.y) * _distanceMove;
                }

                _initialePos = null;
                _endPos = null;
            }
        }
    }

    Vector2Int GetDirection(Vector2 direction)
    {
        direction = direction.normalized;;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            direction = new Vector2(direction.x, 0).normalized;
        else
            direction = new Vector2(0, direction.y).normalized;

        return new Vector2Int((int)direction.x, (int)direction.y);
    }

    void HandleInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (_initialePos == null)
            {
                _obj.position = _targetInitialPos + _targetPos;
                _initialePos = Input.mousePosition;
                DebugText($"Mouse clic\n{_initialePos}", 0);
            }

            DebugText("O", 2);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition;
            DebugText($"Mouse release\n{_initialePos}\n{_endPos}", 0);

            MoveTargetSet();
            DebugText("X", 2);
        }
#else
        if (Input.touchCount == 0)
            return;

            _touch = Input.GetTouch(0);

        if (_touch.phase == TouchPhase.Began)
        {
            if (_initialePos == null)
            {
                _obj.position = _targetInitialPos + _targetPos;
                _initialePos = _touch.position;
                DebugText($"Touch clic\n{_initialePos}", 0);
            }

            DebugText("O", 2);
        }

        if (_touch.phase == TouchPhase.Ended)
        {
            _endPos = _touch.position;
            DebugText($"Touch release\n{_initialePos}\n{_endPos}", 0);

            MoveTargetSet();
            DebugText("X", 2);
        }
#endif
    }

    void TimerDisplay()
    {
        _timer += Time.deltaTime;

        float distance = ((_targetInitialPos + _targetPos) - _obj.position).magnitude;

        DebugText($"Lerp : {Mathf.Round(distance * 100) / 100}\n{Mathf.Round(_timer * 10) / 10}", 1);
    }

    void DebugText(string txt, int value)
    {
        _textUi[value].text = txt;
    }
}
