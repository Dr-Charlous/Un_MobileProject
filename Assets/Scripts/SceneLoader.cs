using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public GameObject Player;

    [SerializeField] Slap _slap;
    [SerializeField] TextAsset LD;
    [SerializeField] Directory[] Keys;
    [SerializeField] Vector2Int _playerPos;

    Vector2Int Size = Vector2Int.zero;
    string[] _lines;

    private void Start()
    {
        DataGrid = CreateGridFromText(LD);
        FillGrid(DataGrid);
        DisplayGridDebug(DataGrid);

        Player.transform.position = new Vector3(_playerPos.x, Player.transform.position.y, _playerPos.y);
        Player.SetActive(true);
    }

    string[,] CreateGridFromText(TextAsset ld)
    {
        if (ld == null)
            return null;

        _lines = ld.text.Split("\n");

        //Size Y
        Size.y = _lines.Length;

        //Size X
        for (int k = 0; k < _lines.Length; k++)
        {
            var xLength = _lines[k].Split(" ");

            if (xLength.Length > Size.x)
                Size.x = xLength.Length;
        }

        //Grid Initialize Length
        return new string[Size.x, Size.y];
    }

    void FillGrid(string[,] grid)
    {
        for (int i = 0; i < Size.y; i++)
        {
            string[] lineDetail = _lines[i].Split(" ");

            for (int j = 0; j < Size.x; j++)
            {
                foreach (var item in Keys)
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
                            Instantiate(PnjPrefab, new Vector3(j, 0, Size.y - i - 1), Quaternion.identity);
                        }

                        if (item.IsEnnemy)
                        {
                            DataGrid[j, i] += " Ennemy";
                            Instantiate(VigilPrefab, new Vector3(j, 0, Size.y - i - 1), Quaternion.identity);
                        }

                        if (item.IsPlayer)
                        {
                            DataGrid[j, i] += " Player";
                            _playerPos = new Vector2Int(j, Size.y - i - 1);
                        }
                    }
                }
            }
        }
    }

    void DisplayGridDebug(string[,] grid)
    {
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                PrefabJspQuoi pref = Instantiate(PrefabObjTest, new Vector3(x, 0, y), Quaternion.identity, transform).GetComponent<PrefabJspQuoi>();
                var mapData = grid[x, Size.y - y - 1];

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
                        //    _playerPos = new Vector2Int(x, Size.y - y - 1);

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

    public bool GetPosDirection(Vector2Int moveDirection)
    {
        string line = DataGrid[_playerPos.x, Size.y - _playerPos.y - 1];
        //Debug.Log($"{line} / {moveDirection}\n{_playerPos}");
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

    public bool IsPnjThere(Vector2Int moveDirection)
    {
        string line = DataGrid[_playerPos.x, Size.y - _playerPos.y - 1];
        string[] lines = line.Split(' ');

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "PNJ")
                _slap.SwitchUi();
        }

        if (GetPosDirection(moveDirection))
            return true;
        else
            return false;
    }
}

[Serializable]
public class Directory
{
    public string Characters;
    public SceneLoader.Categories Category;
    public bool IsUp, IsDown, IsRight, IsLeft, IsPNJ, IsEnnemy, IsPlayer;
}