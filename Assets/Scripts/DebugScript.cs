using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public GameObject Prefab;
    [SerializeField] LayerMask _layerMask;
    Touch _touch;

    void Awake()
    {
        Debug.Log("Bonjour :)");
    }

    private void Update()
    {
        RaycastHit hit;
        _touch = Input.GetTouch(0);

        if (_touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(_touch.position);

            if (Physics.Raycast(ray, out hit, _layerMask))
            {
                Instantiate(Prefab, hit.point, Quaternion.identity);
            }
        }
    }
}
