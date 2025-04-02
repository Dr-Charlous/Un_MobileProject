using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CamFollow : MonoBehaviour
{
    public Transform Target;

    [SerializeField] Transform _pivot;

    private void FixedUpdate()
    {
        if (Target != null)
            _pivot.position = Target.position;
    }
}
