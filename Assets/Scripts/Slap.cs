using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slap : MonoBehaviour
{
    [SerializeField] CharaControl _chara;
    [SerializeField] GameObject _ui;

    public void SwitchUi()
    {
        _ui.SetActive(!_ui.activeSelf);
        _chara.CanMove = _ui.activeSelf;
    }
}
