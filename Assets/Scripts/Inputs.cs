using UnityEngine;

public class Inputs : MonoBehaviour
{
    public Touch Touch;
    public Transform TargetCam;
    public bool IsTouch = false;

    Vector2? _initialePos;
    Vector2? _endPos;
    bool _haveEnnemiesPlayed = true;

    public Vector2Int GetDirection(Vector2 direction)
    {
        direction = direction.normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            direction = new Vector2(direction.x, 0).normalized;
        else
            direction = new Vector2(0, direction.y).normalized;

        return new Vector2Int((int)direction.x, (int)direction.y);
    }

    public void HandleInputs()
    {
        var debug = GameManager.Instance.DebugScript;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            IsTouch = true;

            debug.DebugText($"Mouse clic\n{Input.mousePosition}", 0);
            debug.DebugText("O", 2);

            _haveEnnemiesPlayed = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsTouch = false;

            debug.DebugText($"Mouse release\n{_initialePos}\n{_endPos}", 0);
            debug.DebugText("X", 2);

            if (!_haveEnnemiesPlayed)
            {
                GameManager.Instance.SceneLoader.EnnemiesTrun();
                _haveEnnemiesPlayed = true;
            }
        }
        #else
        if (Input.touchCount == 0)
            return;

        Touch = Input.GetTouch(0);

        if (Touch.phase == TouchPhase.Began)
        {
            IsTouch = true;

            debug.DebugText($"Touch clic\n{Touch.position}", 0);
            debug.DebugText("O", 2);

            _haveEnnemiesPlayed = false;
        }

        if (Touch.phase == TouchPhase.Ended)
        {
            IsTouch = false;

            debug.DebugText($"Touch release\n{_initialePos}\n{_endPos}", 0);
            debug.DebugText("X", 2);

            if (!_haveEnnemiesPlayed)
            {
                GameManager.Instance.SceneLoader.EnnemiesTrun();
                _haveEnnemiesPlayed = true;
            }
        }
#endif
    }
}