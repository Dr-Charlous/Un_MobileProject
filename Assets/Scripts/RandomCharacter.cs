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

        //Not time for shader displace texture :'(
        //int expression = Random.Range(1, 8);
        //int color = Random.Range(1, 6);

        //int foot = Random.Range(1, 8);
        //int leg = Random.Range(1, 8);
        //int torso = Random.Range(1, 8);
        //int head = Random.Range(1, 8);
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
