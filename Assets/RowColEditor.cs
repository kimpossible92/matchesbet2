using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class RowColEditor : EditorWindow
{
    int maxX,MaxY;
    private static RowColEditor window;
    public int levelToEdit=1;
    private int RowLevel;
    private int ColLevel;
    private int limitLeveltype;
    public int currentLevel;
    public int ltype;
    public int limit;//
    public int cLimit;
    public int t;
    public int score1, score2, score3;
    public int[] ingrcount=new int[2];
    public int[] ingr=new int[2], collectItems= new int[2]; private string fileName = "1.txt";
    SquareBlocks[] Blocksf= new SquareBlocks[81];
    string[,] GetVs;
    [MenuItem("Window/RowColEditor")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("levelTool", "change Level", "OK", "");
        window = (RowColEditor)EditorWindow.GetWindow(typeof(RowColEditor));
        window.Show();
    }
    bool isLoad = false;
    void OnFocus()
    {
        TextAsset text = (TextAsset)Resources.Load("" + currentLevel);
        if(text==null)
        {
            isLoad = false;
        }
        else
        {
            isLoad = true;

        }
    }
    void loadEditLevel(string txt)
    {
        string[] lengthtxt = txt.Split(new string[] { "\n" }, options: System.StringSplitOptions.RemoveEmptyEntries);
        int countLine = 0;
        foreach (string iline in lengthtxt)
        {
            if (iline.StartsWith("MODE "))
            {
                t = int.Parse(iline.Replace("MODE ", string.Empty).Trim());//
            }
            else if (iline.StartsWith("SIZE "))
            {
                string sizeString = iline.Replace("SIZE ", string.Empty).Trim();
                string[] sizes = sizeString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                maxX = int.Parse(sizes[0]);
                MaxY = int.Parse(sizes[1]);
            }
            else if (iline.StartsWith("LIMIT "))
            {
                string blockString = iline.Replace("LIMIT", string.Empty).Trim();
                string[] sizes = blockString.Split(new string[] { "/" }, System.StringSplitOptions.RemoveEmptyEntries);
                ltype = int.Parse(sizes[0]);
                limit = int.Parse(sizes[1]);
            }
            else if (iline.StartsWith("COLOR LIMIT "))
            {
                string blockString = iline.Replace("COLOR LIMIT", string.Empty).Trim();
                cLimit = int.Parse(blockString);//
            }
            else if (iline.StartsWith("STARS "))
            {
                string starsString = iline.Replace("STARS", string.Empty).Trim();
                string[] starNum = starsString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                score1 = int.Parse(starNum[0]);
                score2 = int.Parse(starNum[1]);
                score3 = int.Parse(starNum[2]);
            }
            else if (iline.StartsWith("COLLECT COUNT "))
            {
                string starsString = iline.Replace("COLLECT COUNT ", string.Empty).Trim();//
                string[] blocksNumbers = starsString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < blocksNumbers.Length; i++)
                {
                    ingrcount[i] = int.Parse(blocksNumbers[i]);
                }
            }
            else if (iline.StartsWith("COLLECT ITEMS "))
            {
                string collecString = iline.Replace("COLLECT ITEMS ", string.Empty).Trim();
                string[] blocksNumbers = collecString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < blocksNumbers.Length; i++)
                {
                    if (t == 2)
                        ingr[i] = int.Parse(blocksNumbers[i]);
                    else if (t == 1)
                        collectItems[i] = int.Parse(blocksNumbers[i]);
                }
            }
            /*else
            {                
                string[] st = iline.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < st.Length; i++)
                {
                    GetVs[countLine, i] = st[i];
                }
                countLine++;
            }*/
        }
    }
    void Initialize()
    {
        Blocksf = new SquareBlocks[maxX * MaxY];
        GetVs = new string[MaxY, maxX];
    }
    public void SaveLevel(string fileName)
    {
        string saveString = "";

        saveString += "MODE " + t;
        saveString += "\r\n";
        saveString += "SIZE " + maxX/MaxY;
        saveString += "\r\n";
        saveString += "LIMIT " + ltype+ "/" + limit;
        saveString += "\r\n";
        saveString += "COLOR LIMIT " + cLimit;
        saveString += "\r\n";
        saveString += "STARS " + score1 + "/" + score2 + "/" + score3;
        saveString += "\r\n";
        saveString += "COLLECT COUNT " + ingrcount[0] + "/" + ingrcount[1];
        saveString += "\r\n";
        if (t == 1)
            saveString += "COLLECT ITEMS " + ingr[0] + "/" + ingr[1];
        else if (t == 3)
            saveString += "COLLECT ITEMS " + collectItems[0] + "/" + collectItems[1];
        else
            saveString += "COLLECT ITEMS " + ingr[0] + "/" + ingr[1];
        saveString += "\r\n";
        /*for(int row = 0; row < MaxY; row++)
        {
            for(int col = 0; col < maxX; col++)
            {
                saveString += GetVs[row, col];
                if (col < (maxX - 1))
                    saveString += " ";
            }
            if (row < (MaxY - 1))
                saveString += "\r\n";
        }*/
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string activeDir = Application.dataPath + @"/Resources/";
            string newPath = System.IO.Path.Combine(activeDir, levelToEdit + ".txt");
            StreamWriter sw = new StreamWriter(newPath);
            sw.Write(saveString);
            sw.Close();
        }
        AssetDatabase.Refresh();
    }
    private void OnGUI()
    {
        if (levelToEdit < 1)
            levelToEdit = 1;
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
       
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(50) });
        GUILayout.EndHorizontal();
        try
        {
            string changeLvl = GUILayout.TextField("" + levelToEdit, new GUILayoutOption[] { GUILayout.Width(125) });
            if (int.Parse(changeLvl) != levelToEdit)
            {
                if (LoadDataFromLocal(int.Parse(changeLvl)))
                    levelToEdit = int.Parse(changeLvl);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            string changeRow = GUILayout.TextField("" + maxX, new GUILayoutOption[] { GUILayout.Width(50) });
            if (int.Parse(changeRow) != maxX)
            {
                if (maxX != (int.Parse(changeRow)))
                    maxX = int.Parse(changeRow);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            string changeCol = GUILayout.TextField("" + MaxY, new GUILayoutOption[] { GUILayout.Width(50) });
            if (int.Parse(changeCol) != MaxY)
            {
                if (MaxY != (int.Parse(changeCol)))
                    MaxY = int.Parse(changeCol);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            string changelType = GUILayout.TextField("" + ltype, new GUILayoutOption[] { GUILayout.Width(50) });
            if (int.Parse(changelType) != ltype)
            {
                if (ltype != (int.Parse(changelType)))
                    ltype = int.Parse(changelType);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            string changelimit = GUILayout.TextField("" + limit, new GUILayoutOption[] { GUILayout.Width(50) });
            if (int.Parse(changelimit) != limit)
            {
                if (limit != (int.Parse(changelimit)))
                    limit = int.Parse(changelimit);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            string changecLimit = GUILayout.TextField("" + cLimit, new GUILayoutOption[] { GUILayout.Width(50) });
            if (int.Parse(changecLimit) != cLimit)
            {
                if (cLimit != (int.Parse(changecLimit)))
                    cLimit = int.Parse(changecLimit);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            string changescore1 = GUILayout.TextField("" + score1, new GUILayoutOption[] { GUILayout.Width(50) });
            string changescore2 = GUILayout.TextField("" + score2, new GUILayoutOption[] { GUILayout.Width(50) });
            string changescore3 = GUILayout.TextField("" + score3, new GUILayoutOption[] { GUILayout.Width(50) });
            if (int.Parse(changescore1) != score1)
            {
                if (score1 != (int.Parse(changescore1)))
                    score1 = int.Parse(changescore1);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            if (int.Parse(changescore2) != score2)
            {
                if (score2 != (int.Parse(changescore2)))
                    score2 = int.Parse(changescore2);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            if (int.Parse(changescore3) != score3)
            {
                if (score3 != (int.Parse(changescore3)))
                    score3 = int.Parse(changescore3);
                if (!fileName.Contains(".txt"))
                    fileName += ".txt";
                SaveLevel(fileName);
            }
            for (int i = 0; i < ingrcount.Length; i++) {
                string changeingr = GUILayout.TextField("" + ingrcount[i], new GUILayoutOption[] { GUILayout.Width(50) });//
                if (int.Parse(changeingr) != ingrcount[i])
                {
                    if (ingrcount[i] != (int.Parse(changeingr)))
                        ingrcount[i] = int.Parse(changeingr);
                    if (!fileName.Contains(".txt"))
                        fileName += ".txt";
                    SaveLevel(fileName);
                }
            }
            /*for(int j = 0; j < MaxY; j++)
            {
                string changeline = GUILayout.TextField("" + linesi[j], new GUILayoutOption[] { GUILayout.Width(150) });
                if (changeline != linesi[j])
                {
                    if (linesi[j] != changeline)linesi[j] = changeline;
                    if (!fileName.Contains(".txt"))
                        fileName += ".txt";
                    SaveLevel(fileName);
                }
            }*/
            GUILayout.BeginHorizontal();

        }
        catch (Exception e)
        {
            throw;
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<<", new GUILayoutOption[] { GUILayout.Width(50) }))
        {
            //linesi.Clear();
            levelToEdit--;
            if (levelToEdit < 1)
                levelToEdit = 1;
            if (!LoadDataFromLocal(levelToEdit))
                levelToEdit++;
        }
        if (GUILayout.Button(">>", new GUILayoutOption[] { GUILayout.Width(50) }))
        {
            //linesi.Clear();
            levelToEdit++;
            if (!LoadDataFromLocal(levelToEdit))
                levelToEdit--;
        }
        if (GUILayout.Button("Save", new GUILayoutOption[] { GUILayout.Width(50) }))
        {

        }
    }
    public bool LoadDataFromLocal(int currentLevel)
    {
        TextAsset mapText = Resources.Load("" + currentLevel) as TextAsset;
        if (mapText == null)
        {
            return false;
        }
        loadEditLevel(mapText.text);
        return true;
    }
}