using System;
using UnityEngine;
using static UnityEditor.Progress;

public class SceneLoader : MonoBehaviour
{
    public enum Categories
    {
        Wall,
        Character
    }

    public string[,] DataGrid { get; private set; }
    public GameObject PrefabObjTest;

    [SerializeField] TextAsset LD;
    [SerializeField] Repairtory[] Keys;

    Vector2Int Size = Vector2Int.zero;
    string[] _lines;

    private void Start()
    {
        DataGrid = CreateGridFromText(LD);
        FillGrid(DataGrid);
        DisplayGridDebug(DataGrid);
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
                        if (item.Category == Categories.Wall)
                        {
                            DataGrid[j, i] = "Wall";

                            if (item.IsUp)
                                DataGrid[j, i] += " Up";

                            if (item.IsDown)
                                DataGrid[j, i] += " Down";

                            if (item.IsRight)
                                DataGrid[j, i] += " Right";

                            if (item.IsLeft)
                                DataGrid[j, i] += " Left";
                        }

                        if (item.Category == Categories.Character)
                        {
                            DataGrid[j, i] = "Character";

                            if (item.IsPNJ)
                                DataGrid[j, i] += " PNJ";

                            if (item.IsEnnemy)
                                DataGrid[j, i] += " Ennemy";

                            if (item.IsPlayer)
                                DataGrid[j, i] += " Player";
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
                PrefabJspQuoi pref = Instantiate(PrefabObjTest, new Vector3(x, 0, y), Quaternion.identity).GetComponent<PrefabJspQuoi>();
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

                        if (value >= 4)
                            pref.PrefabBloc.SetActive(true);

                        if (line[k] == "Character")
                            pref.PrefabCenter.SetActive(true);
                    }
                }
            }
        }
    }
}

[Serializable]
public class Repairtory
{
    public string Characters;
    public SceneLoader.Categories Category;
    public bool IsUp, IsDown, IsRight, IsLeft, IsPNJ, IsEnnemy, IsPlayer;
}