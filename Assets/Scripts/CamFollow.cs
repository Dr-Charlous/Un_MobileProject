using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Speed;

    [SerializeField] Transform _pivot;

    private void FixedUpdate()
    {
        if (Target != null)
            _pivot.position = Target.position;

        if (Speed != Vector3.zero)
        {
            _pivot.Rotate(Speed);
        }
    }
}
