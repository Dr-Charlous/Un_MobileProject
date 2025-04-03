using UnityEngine;

public class CharaControl : MonoBehaviour
{
    public Transform MeshObj;
    public Vector3 TargetInitialPos;
    public Vector3 TargetPos;
    public bool CanMove = true;

    [SerializeField] Inputs _inputs;
    [SerializeField] Slap _slap;
    [SerializeField] float _distanceInput = 1;
    [SerializeField] float _distanceMove = 1;
    [SerializeField][Range(0f, 1f)] float _lerpSpeed = 0.5f;

    Vector2? _initialePos;
    Vector2? _endPos;
    Vector2Int _charaPos;
    bool _isTouchEndAlreadyUsed = false;

    private void Start()
    {
        _charaPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        TargetInitialPos = MeshObj.position;
    }

    private void Update()
    {
        if (_inputs != null)
        {
            _inputs.HandleInputs();
            HandleInputs();
        }

        MeshObj.position = Vector3.Lerp(MeshObj.position, TargetInitialPos + TargetPos, _lerpSpeed);
    }

    void MoveTargetSet()
    {
        Vector2 direction = Vector2.zero;
        var sceneLoad = GameManager.Instance.SceneLoader;

        if (_initialePos != null && _endPos != null)
        {
            direction = (Vector2)_endPos - (Vector2)_initialePos;

            if (direction != Vector2.zero && direction.magnitude >= _distanceInput)
            {
                Vector2Int intDirection;

                intDirection = _inputs.GetDirection(direction);

                //Move
                if (sceneLoad.GetPosDirection(intDirection, _charaPos) && CanMove)
                {
                    TargetInitialPos = MeshObj.position;
                    TargetPos = new Vector3(intDirection.x, MeshObj.position.y, intDirection.y) * _distanceMove;

                    //Rotation mesh
                    MeshObj.LookAt(MeshObj.position + new Vector3(intDirection.x, 0, intDirection.y) * _distanceMove);

                    _charaPos += intDirection;
                }

                //Slap
                if (_inputs != null)
                {
                    if (sceneLoad.IsPnjThere(_charaPos, _slap) && !CanMove)
                    {
                        _slap.SlapAction(_charaPos);
                    }
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
                MeshObj.position = TargetInitialPos + TargetPos;
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
                MeshObj.position = TargetInitialPos + TargetPos;
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

    public void BotInputs()
    {
        Vector2 direction = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) - Vector2.zero;

        if (direction != Vector2.zero)
        {
            Vector2Int intDirection;
            direction = direction.normalized;

            //Direction cross
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                direction = new Vector2(direction.x, 0).normalized;
            else
                direction = new Vector2(0, direction.y).normalized;

            intDirection = new Vector2Int((int)direction.x, (int)direction.y);

            //Move
            if (GameManager.Instance.SceneLoader.GetPosDirection(intDirection, _charaPos) && CanMove)
            {
                TargetInitialPos = MeshObj.position;
                TargetPos = new Vector3(intDirection.x, MeshObj.position.y, intDirection.y) * _distanceMove;

                //Rotation mesh
                MeshObj.LookAt(MeshObj.position + new Vector3(intDirection.x, 0, intDirection.y) * _distanceMove);

                _charaPos += intDirection;
            }
            else
                BotInputs();
        }
    }
}
