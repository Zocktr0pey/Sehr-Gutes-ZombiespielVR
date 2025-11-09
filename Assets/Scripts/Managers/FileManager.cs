using System;
using System.IO;
using UnityEngine;
using TMPro;

public class FileManager : MonoBehaviour
{
    [SerializeField] GameObject highScore;

    public static FileManager instance;
    public static FileManager Instance
    {
        get { return instance; }
    }

    string filePath;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        // Define file path (works across all platforms)
        filePath = Path.Combine(Application.persistentDataPath, "score.txt");
        highScore.GetComponent<TMP_Text>().text = "Highscore: " + ReadScore();

    }

    public int ReadScore()
    {
        if (File.Exists(filePath))
        {
            return Int32.Parse(File.ReadAllText(filePath));
        }
        else
        {
            WriteScore(0);
            return 0;
        }
    }

    public void WriteScore(int score)
    {
        File.WriteAllText(filePath, score.ToString());
    }
}
