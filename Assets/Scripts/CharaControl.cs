using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaControl : MonoBehaviour
{
    public Transform Obj;
    public Vector3 TargetInitialPos;
    public Vector3 TargetPos;
    public bool CanMove = true;

    [SerializeField] Inputs _inputs;
    [SerializeField] Slap _slap;
    [SerializeField] float _distanceInput = 1;
    [SerializeField] float _distanceMove = 1;
    [SerializeField][Range(0f, 1f)] float _lerpSpeed = 0.5f;

    Touch _touch;
    Vector2? _initialePos;
    Vector2? _endPos;
    Vector2Int _charaPos;
    bool _isTouchBeginAlreadyUsed = false;
    bool _isTouchEndAlreadyUsed = false;

    private void Start()
    {
        _charaPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        TargetInitialPos = Obj.position;
    }

    private void Update()
    {
        if (_inputs != null)
        {
            _inputs.HandleInputs();
            HandleInputs();
        }
        else
        {
            //BotInputs();
        }

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
                Vector2Int intDirection;
                if (_inputs != null)
                    intDirection = _inputs.GetDirection(direction);
                else
                {
                    direction = direction.normalized;

                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                        direction = new Vector2(direction.x, 0).normalized;
                    else
                        direction = new Vector2(0, direction.y).normalized;

                    intDirection = new Vector2Int((int)direction.x, (int)direction.y);
                }

                var sceneLoad = GameManager.Instance.SceneLoader;

                //Move
                if (sceneLoad.GetPosDirection(intDirection, _charaPos) && CanMove)
                {
                    TargetInitialPos = Obj.position;
                    TargetPos = new Vector3(intDirection.x, Obj.position.y, intDirection.y) * _distanceMove;

                    _charaPos += intDirection;
                }

                //Slap
                if (sceneLoad.IsPnjThere(intDirection, _charaPos, _slap) && !CanMove)
                {
                    _slap.SlapAction(_charaPos);
                }

                _initialePos = null;
                _endPos = null;
            }
        }
    }

    void HandleInputs()
    {
#if UNITY_EDITOR
        if (_inputs.IsTouch)
        {
            if (_initialePos == null)
            {
                Obj.position = TargetInitialPos + TargetPos;
                _initialePos = Input.mousePosition;
            }
        }

        if (!_inputs.IsTouch)
        {
            _endPos = Input.mousePosition;
            MoveTargetSet();
        }
#else
        if (_inputs.IsTouch)
        {
            if (_initialePos == null)
            {
                Obj.position = TargetInitialPos + TargetPos;
                _initialePos = _inputs.Touch.position;
            }

            _isTouchEndAlreadyUsed = false;
        }

        if (!_inputs.IsTouch && !_isTouchEndAlreadyUsed)
        {
            _endPos = _inputs.Touch.position;
            MoveTargetSet();
            _isTouchEndAlreadyUsed = true;
        }
#endif
    }

    void BotInputs()
    {
        _initialePos = Vector2.zero;
        _endPos = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));

        MoveTargetSet();
    }
}
