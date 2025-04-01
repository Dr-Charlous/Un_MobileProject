using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Slap : MonoBehaviour
{
    [SerializeField] CharaControl _chara;
    [SerializeField] SceneLoader _loader;
    [SerializeField] AudioSource _source;
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _ui;
    [SerializeField] float _time;

    Coroutine _coroutine;

    private void Start()
    {
        _ui.SetActive(false);
    }

    public void SwitchUi()
    {
        _ui.SetActive(_chara.CanMove);
        _chara.CanMove = !_chara.CanMove;
    }

    public void SlapAction()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(Action(_time));
    }

    IEnumerator Action(float time)
    {
        _anim.SetTrigger("Slap");
        _source.Play();
        yield return new WaitForSeconds(time);
        SwitchUi();
        _loader.KillPeople(_loader.PlayerPos);
        _coroutine = null;
    }
}