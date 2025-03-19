using System;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public enum Categories
    {
        Wall,
        Character
    }


    [SerializeField] TextAsset LD;
    [SerializeField] Repairtory[] Keys;

    Vector2Int Size = Vector2Int.zero;
    string[,] DataGrid;
    string[] _lines;

    private void Start()
    {
        CreateGridFromText(LD);
        FillGrid(DataGrid);
        DisplayGridDebug(DataGrid);
    }

    void CreateGridFromText(TextAsset ld)
    {
        if (ld == null)
            return;

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
        DataGrid = new string[Size.x, Size.y];
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
                            DataGrid[j, i] = "Wall";

                        if (item.Category == Categories.Character)
                            DataGrid[j, i] = "Character";

                        if (item.IsUp)
                            DataGrid[j, i] += " Up";

                        if (item.IsDown)
                            DataGrid[j, i] += " Down";

                        if (item.IsRight)
                            DataGrid[j, i] += " Right";

                        if (item.IsLeft)
                            DataGrid[j, i] += " Left";

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

    void DisplayGridDebug(string[,] grid)
    {
        string sentence = "";

        for (int i = 0; i < Size.y; i++)
        {
            for (int j = 0; j < Size.x; j++)
            {
                sentence += DataGrid[j, i] + "||";
            }
            sentence += "\n";
        }

        Debug.Log(sentence);
    }
}

[Serializable]
public class Repairtory
{
    public string Characters;
    public SceneLoader.Categories Category;
    public bool IsUp, IsDown, IsRight, IsLeft, IsPNJ, IsEnnemy, IsPlayer;
}