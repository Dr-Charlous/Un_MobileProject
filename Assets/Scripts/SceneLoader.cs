using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public enum Categories
    {
        Wall,
        Character
    }

    public string[,] DataGrid { get; private set; }
    public GameObject PrefabObjTest;
    public GameObject PnjPrefab;
    public GameObject VigilPrefab;
    public GameObject PlayerPrefab;

    [SerializeField] TextAsset _ld;
    [SerializeField] Directory[] _keys;
    [SerializeField] Dictionary<string, GameObject> _characters = new();
    [SerializeField] Dictionary<string, GameObject> _ennemies = new();

    Vector2Int _size = Vector2Int.zero;
    string[] _lines;

    private void Start()
    {
        DataGrid = CreateGridFromText(_ld);
        FillGrid(DataGrid);
        DisplayGridDebug(DataGrid);
    }

    string[,] CreateGridFromText(TextAsset ld)
    {
        if (ld == null)
            return null;

        _lines = ld.text.Split("\n");

        //_size Y
        _size.y = _lines.Length;

        //_size X
        for (int k = 0; k < _lines.Length; k++)
        {
            var xLength = _lines[k].Split(" ");

            if (xLength.Length > _size.x)
                _size.x = xLength.Length;
        }

        //Grid Initialize Length
        return new string[_size.x, _size.y];
    }

    void FillGrid(string[,] grid)
    {
        for (int i = 0; i < _size.y; i++)
        {
            string[] lineDetail = _lines[i].Split(" ");

            for (int j = 0; j < _size.x; j++)
            {
                foreach (var item in _keys)
                {
                    if (lineDetail[j] == item.Characters)
                    {
                        DataGrid[j, i] = "";

                        if (item.Category == Categories.Wall)
                            DataGrid[j, i] += "Wall";

                        if (item.IsUp)
                            DataGrid[j, i] += " Up";

                        if (item.IsDown)
                            DataGrid[j, i] += " Down";

                        if (item.IsRight)
                            DataGrid[j, i] += " Right";

                        if (item.IsLeft)
                            DataGrid[j, i] += " Left";

                        if (item.Category == Categories.Character)
                            DataGrid[j, i] += " Character";

                        if (item.IsPNJ)
                        {
                            DataGrid[j, i] += " PNJ";
                            var pnj = Instantiate(PnjPrefab, new Vector3(j, 0, _size.y - i - 1), Quaternion.identity);
                            _characters.Add($"{j} {i}", pnj);

                            //Random rotation
                            pnj.transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 4) * 90);
                        }

                        if (item.IsEnnemy)
                        {
                            DataGrid[j, i] += " Ennemy";
                            _ennemies.Add($"{j} {i}", Instantiate(VigilPrefab, new Vector3(j, 0, _size.y - i - 1), Quaternion.identity));
                        }

                        if (item.IsPlayer)
                        {
                            DataGrid[j, i] += " Player";
                            var player = Instantiate(PlayerPrefab, new Vector3(j, 0, _size.y - i - 1), Quaternion.identity);
                            GameManager.Instance.DebugScript.Chara = player.GetComponentInChildren<CharaControl>();
                            GameManager.Instance.CamFollow.Target = player.GetComponentInChildren<Inputs>().TargetCam;
                        }
                    }
                }
            }
        }
    }

    void DisplayGridDebug(string[,] grid)
    {
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                PrefabJspQuoi pref = Instantiate(PrefabObjTest, new Vector3(x, 0, y), Quaternion.identity, transform).GetComponent<PrefabJspQuoi>();
                var mapData = grid[x, _size.y - y - 1];

                //Debug.Log($"{x}:{y} = {mapData}");

                if (mapData != null)
                {
                    string[] line = mapData.Split(' ');
                    int value = 0;

                    for (int k = 0; k < line.Length; k++)
                    {
                        if (line[k] == "Up")
                        {
                            pref.PrefabFront.SetActive(true);
                            value++;
                        }
                        if (line[k] == "Down")
                        {
                            pref.PrefabBack.SetActive(true);
                            value++;
                        }
                        if (line[k] == "Right")
                        {
                            pref.PrefabRight.SetActive(true);
                            value++;
                        }
                        if (line[k] == "Left")
                        {
                            pref.PrefabLeft.SetActive(true);
                            value++;
                        }

                        //if (line[k] == "Character" || line[k] == "PNJ" || line[k] == "Ennemy")
                        //    pref.PrefabCenter.SetActive(true);

                        //if (line[k] == "Player")
                        //    _charaPos = new Vector2Int(x, _size.y - y - 1);

                        if (value >= 4)
                        {
                            pref.PrefabBloc.SetActive(true);

                            pref.PrefabFront.SetActive(false);
                            pref.PrefabBack.SetActive(false);
                            pref.PrefabRight.SetActive(false);
                            pref.PrefabLeft.SetActive(false);
                            pref.PrefabCenter.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    public bool GetPosDirection(Vector2Int moveDirection, Vector2Int charaPos)
    {
        string line = DataGrid[charaPos.x, _size.y - charaPos.y - 1];
        //Debug.Log($"{line} / {moveDirection}\n{_charaPos}");
        string[] lines = line.Split(' ');
        bool value = true;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "Down" && moveDirection.y <= -1)
                value = false;
            if (lines[i] == "Up" && moveDirection.y >= 1)
                value = false;
            if (lines[i] == "Right" && moveDirection.x >= 1)
                value = false;
            if (lines[i] == "Left" && moveDirection.x <= -1)
                value = false;
        }

        return value;
    }

    public bool IsPnjThere(Vector2Int charaPos, Slap slap)
    {
        string line = DataGrid[charaPos.x, _size.y - charaPos.y - 1];
        string[] lines = line.Split(' ');
        bool value = false;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "PNJ")
                value = true;
            if (lines[i] == "Done")
                value = false;
        }

        if (value)
        {
            slap.SwitchUi();
            DataGrid[charaPos.x, _size.y - charaPos.y - 1] += " Done";
        }

        if (!value)
            return true;
        else
            return false;
    }

    public void KillPeople(Vector2Int pos)
    {
        string key = $"{pos.x} {_size.y - pos.y - 1}";

        if (_characters[key] == null)
            return;

        _characters[key].GetComponentInChildren<Animator>().SetBool("IsDead", true);
        //Destroy(_characters[key]);
        //_characters.Remove(key);
    }

    public void EnnemiesTrun()
    {
        foreach (var vigil in _ennemies)
        {
            vigil.Value.GetComponent<CharaControl>().BotInputs();
        }
    }
}

[Serializable]
public class Directory
{
    public string Characters;
    public SceneLoader.Categories Category;
    public bool IsUp, IsDown, IsRight, IsLeft, IsPNJ, IsEnnemy, IsPlayer;
}