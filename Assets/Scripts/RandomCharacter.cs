using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] _foot;
    [SerializeField] GameObject[] _legs;
    [SerializeField] GameObject[] _torsos;
    [SerializeField] GameObject[] _head;
    [SerializeField] GameObject[] _hair;

    private void Start()
    {
        RandomElement(_foot);
        RandomElement(_legs);
        RandomElement(_torsos);
        RandomElement(_head);
        RandomElement(_hair);
    }

    void RandomElement(GameObject[] objs)
    {
        int value = Random.Range(0, objs.Length);

        for (int i = 0; i < objs.Length; i++)
        {
            if (i != value)
                Destroy(objs[i]);
        }
    }
}
