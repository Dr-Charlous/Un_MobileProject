using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaControl : MonoBehaviour
{
    public Transform Obj;
    public Vector3 TargetInitialPos;
    public Vector3 TargetPos;
    public bool CanMove = true;

    [SerializeField] DebugScript _debug;
    [SerializeField] SceneLoader _sceneLoad;
    [SerializeField] float _distanceInput = 1;
    [SerializeField] float _distanceMove = 1;
    [SerializeField][Range(0f, 1f)] float _lerpSpeed = 0.5f;

    Touch _touch;
    Vector2? _initialePos;
    Vector2? _endPos;

    private void Start()
    {
        TargetInitialPos = Obj.position;
    }

    private void Update()
    {
        HandleInput();

        Obj.position = Vector3.Lerp(Obj.position, TargetInitialPos + TargetPos, _lerpSpeed);
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

                //Move
                if (_sceneLoad.GetPosDirection(intDirection) && CanMove)
                {
                    TargetInitialPos = Obj.position;
                    TargetPos = new Vector3(intDirection.x, Obj.position.y, intDirection.y) * _distanceMove;
                }

                //Slap
                if (_sceneLoad.IsPnjThere(intDirection) && !CanMove)
                {
                    //Slap fonction
                }

                _initialePos = null;
                _endPos = null;
            }
        }
    }

    Vector2Int GetDirection(Vector2 direction)
    {
        direction = direction.normalized; ;

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
                Obj.position = TargetInitialPos + TargetPos;
                _initialePos = Input.mousePosition;
                _debug.DebugText($"Mouse clic\n{_initialePos}", 0);
            }

            _debug.DebugText("O", 2);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition;
            _debug.DebugText($"Mouse release\n{_initialePos}\n{_endPos}", 0);

            MoveTargetSet();

            _debug.DebugText("X", 2);
        }
#else
        if (Input.touchCount == 0)
            return;

            _touch = Input.GetTouch(0);

        if (_touch.phase == TouchPhase.Began)
        {
            if (_initialePos == null)
            {
                Obj.position = TargetInitialPos + TargetPos;
                _initialePos = _touch.position;
                _debug.DebugText($"Touch clic\n{_initialePos}", 0);
            }

            _debug.DebugText("O", 2);
        }

        if (_touch.phase == TouchPhase.Ended)
        {
            _endPos = _touch.position;
            _debug.DebugText($"Touch release\n{_initialePos}\n{_endPos}", 0);

            MoveTargetSet();

            _debug.DebugText("X", 2);
        }
#endif
    }
}
