using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public string m_levelName;
    public int m_levelCount;
    public List<string> m_level;

    [HideInInspector] public int m_width;
    [HideInInspector] public int m_height;

    public void SetLevel(string levelName, int levelCount, List<string> level)
    {
        m_levelName = levelName;
        m_levelCount = levelCount;
        m_level = level;

        m_height = m_level.Count;
        

        // Setting the width to the length of the longest row
        int maxWidth = 0;
        for(int i = 0; i < m_level.Count; i++)
            if(m_level[i].Length > maxWidth)
                maxWidth = m_level[i].Length;

        m_width = maxWidth;
    }
}

public class LevelManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera cam;

    [Header("Prefabs")]
    [SerializeField] GameObject wall;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject player;
    [SerializeField] GameObject box;
    [SerializeField] GameObject point;

    [Header("Level Loader")]
    [SerializeField] TextAsset levelsToLoad;
    [SerializeField] bool loadLevels;
    
    [SerializeField] List<Level> levels = new List<Level>();
    [HideInInspector] public int m_currentLevel = 0;

    public static LevelManager Instance;

    UIInfo m_ui;
    List<Box> m_boxes;

    public static event Action OnLevelStarted;
    public static event Action OnLevelComplete;

    private void Awake()
    {
        if(Instance != null)
            Debug.LogError("Multiple LevelManager scripts found!. Make sure there is only one!");

        Instance = this;

        m_ui = GetComponent<UIInfo>();

        OnLevelStarted = null;
        OnLevelComplete = null;
    }

    private void Start()
    {
        GenerateLevel(levels[m_currentLevel]);

        Box.OnSolve += IsLevelComplete;
        m_boxes = new List<Box>();
    }

    public void RegisterBox(Box box) => m_boxes.Add(box);

    private void OnValidate()
    {
        if(loadLevels)
        {
            LoadLevels();
            loadLevels = false;
        }
    }

    void LoadLevels()
    {
        levels.Clear();

        // Reading the file
        string text = levelsToLoad.text;
        string[] v_levels = text.Split(new char[] { ';' });

        // Loading all the levels and setting the required settings correctly
        for(int i = 0; i < v_levels.Length - 1; i++)
        {
            Level level = new Level();
            levels.Add(level);

            string[] lines = v_levels[i].Split(new char[] { '\n' });

            string levelName = "";
            List<string> levelRows = new List<string>();

            for(int j = 0; j < lines.Length; j++)
            {
                string line = lines[j];

                if(line.Contains(":"))
                {
                    levelName = line.Remove(line.Length - 2);
                    continue;
                }

                if(line.Contains(";"))
                {
                    levelRows.Add(line.Remove(line.Length - 1));
                    continue;
                }

                if(line.Length <= 1)
                    continue;

                levelRows.Add(line);
            }

            level.SetLevel(levelName, i + 1, levelRows);
        }

        print("Levels loaded!");
    }

    public void Restart()
    {
        m_boxes = new List<Box>();
        DestroyLevel();
        GenerateLevel(levels[m_currentLevel]);
    }

    public void DestroyLevel()
    {
        for(int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }

    public void GenerateLevel(Level level)
    {
        SetCameraSize(level);

        // A math equasion to get the correct position where pieces start to generate on
        Vector2 startPos = new Vector2(((float)-level.m_width + 2) / 2,
            -((float)-level.m_height + 1) / 2);

        // Looping through each character of the level rows and columns
        for(int y = 0; y < level.m_level.Count; y++)
        {
            for(int x = 0; x < level.m_level[y].Length; x++)
            {
                Vector2 position = startPos + new Vector2(x, -y);
                char c = level.m_level[y][x];

                // Generating the correct piece
                switch(c)
                {
                    case '#': GeneratePiece(wall, position); break;
                    case 'P': GeneratePiece(player, position); goto case '-';
                    case 'X': GeneratePiece(box, position); goto case '-';
                    case '*': GeneratePiece(point, position); goto case '-';
                    case 'O': GeneratePiece(point, position); goto case 'X';
                    case '-': GeneratePiece(ground, position); break;
                }
            }
        }

        m_ui.SetLevel(levels[m_currentLevel]);
        OnLevelStarted?.Invoke();
        Player.LevelStarted = true;
    }

    void GeneratePiece(GameObject piece, Vector2 position) =>
        Instantiate(piece, position, Quaternion.identity, transform);

    // Setting the the camera size to best fit the size of the level
    void SetCameraSize(Level level)
    {
        if(level.m_width >= level.m_height)
        {
            cam.orthographicSize = Mathf.RoundToInt((float)level.m_height / 2 + 0.5f) + 1;
            return;
        }

        cam.orthographicSize = Mathf.RoundToInt((float)level.m_width / 2 + 0.5f) + 1;
    }

    void IsLevelComplete()
    {
        for(int i = 0; i < m_boxes.Count; i++)
            if(!m_boxes[i].m_solved)
                return;

        Player.LevelStarted = false;
        OnLevelComplete?.Invoke();
    }

    public bool LoadNextLevel()
    {
        if(m_currentLevel >= levels.Count - 1)
            return false;

        m_currentLevel++;
        DestroyLevel();
        GenerateLevel(levels[m_currentLevel]);

        return true;
    }
}