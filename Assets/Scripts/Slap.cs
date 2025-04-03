using System.Collections;
using UnityEngine;

public class Slap : MonoBehaviour
{
    [SerializeField] CharaControl _chara;
    [SerializeField] Animator _anim;
    [SerializeField] AudioSource _source;
    [SerializeField] float _time;

    Coroutine _coroutine;

    private void Start()
    {
        GameManager.Instance.SlapUi.SetActive(false);
    }

    public void SwitchUi()
    {
        GameManager.Instance.SlapUi.SetActive(_chara.CanMove);
        _chara.CanMove = !_chara.CanMove;
    }

    public void SlapAction(Vector2Int pos)
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(Action(_time, pos)); ;
    }

    IEnumerator Action(float time, Vector2Int pos)
    {
        GameManager.Instance.SlapAnim.SetTrigger("Slap");
        _anim.SetTrigger("Slap");
        _source.Play();
        yield return new WaitForSeconds(time);
        SwitchUi();
        GameManager.Instance.SceneLoader.KillPeople(pos);
        _coroutine = null;
    }
}